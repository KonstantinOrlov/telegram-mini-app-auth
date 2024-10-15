using System.Collections.Specialized;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neftm.TelegramMiniApp.Authorization.Constants;
using Neftm.TelegramMiniApp.Authorization.Helpers;
using Neftm.TelegramMiniApp.Authorization.Models;

namespace Neftm.TelegramMiniApp.Authorization;

/// <summary>
/// Handles authentication for Telegram Mini App
/// </summary>
public class TmaAuthenticationHandler : AuthenticationHandler<TmaAuthenticationOptions>
{
#if NET8_0
	public TmaAuthenticationHandler(
		IOptionsMonitor<TmaAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) :
		base(options, logger, encoder) { }
#elif NET6_0_OR_GREATER
	public TmaAuthenticationHandler(
		IOptionsMonitor<TmaAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder,
		ISystemClock clock) :
		base(options, logger, encoder, clock) { }
#endif

	/// <summary>
	/// Handles authentication
	/// </summary>
	/// <returns>Authentication result.</returns>
	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
			return Task.FromResult(AuthenticateResult.Fail($"Missing Authorization header"));

		var token = Request.Headers[Options.TokenHeaderName].ToString();
		var tokenPrefix = token.Substring(0, TmaDefaults.TokenPrefix.Length).ToLowerInvariant();
		
		if (tokenPrefix != TmaDefaults.TokenPrefix.ToLowerInvariant()) return Task.FromResult(AuthenticateResult.NoResult());

		token = token.Remove(0, TmaDefaults.TokenPrefix.Length);
		
		var data = HttpUtility.ParseQueryString(token);
		var initData = ParseToken(data);

		//validate initData
		if (string.IsNullOrWhiteSpace(initData.Hash)
            || (initData.AuthDate.HasValue is false)
		    || HashHelper.CalculateTmaHash(BuildDataCheckString(data), Options.BotToken) != initData.Hash)
		{
			return Task.FromResult(AuthenticateResult.Fail($"Invalid token"));
		}

		// Set feature & claims
		Request.HttpContext.Features.Set(initData);

		var claims = new List<Claim>()
		{
			new(TmaClaimTypes.UserId, initData.TmaInitDataUser.Id.ToString()),
			new(TmaClaimTypes.UserName, initData.TmaInitDataUser.UserName),
			new(TmaClaimTypes.FirstName, initData.TmaInitDataUser.FirstName),
			new(TmaClaimTypes.LastName, initData.TmaInitDataUser.LastName),
			new(TmaClaimTypes.ChatInstance, initData.ChatInstance),
			new(TmaClaimTypes.IsPremium, initData.TmaInitDataUser.IsPremium.ToString()),
		};

		var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
		var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

		return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
	}

	private TmaInitData ParseToken(NameValueCollection input)
	{
		return new TmaInitData()
		{
			TmaInitDataUser = string.IsNullOrWhiteSpace(input["user"]) ? null : JsonSerializer.Deserialize<TmaInitDataUser>(input["user"]),
			ChatInstance = input["chat_instance"] ?? string.Empty,
			ChatType = input["chat_type"] ?? string.Empty,
			AuthDate = long.TryParse(input["auth_date"], out var parseResult) ? DateTimeOffset.FromUnixTimeSeconds(parseResult) : null,
			Hash = input["hash"] ?? string.Empty,
		};
	}
	
	private string BuildDataCheckString(NameValueCollection input)
	{
		return string.Join("\n",
			input.AllKeys
				.Where(d => d != "hash")
				.OrderBy(x => x)
				.Select(x => $"{x}={input[x]}"));
	}
}