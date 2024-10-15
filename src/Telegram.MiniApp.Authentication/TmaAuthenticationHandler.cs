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

		//validate initData
		if (string.IsNullOrWhiteSpace(data[TmaInitDataKeys.Hash])
		    || (string.IsNullOrWhiteSpace(data[TmaInitDataKeys.AuthDate]))
		    || HashHelper.CalculateTmaHash(BuildDataCheckString(data), Options.BotToken) != data[TmaInitDataKeys.Hash])
		{
			return Task.FromResult(AuthenticateResult.Fail($"Invalid token"));
		}

		// Set feature & claims
		Request.HttpContext.Features.Set(data);
		var claims = BuildClaims(data);

		var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
		var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

		return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
	}

	private string BuildDataCheckString(NameValueCollection input)
	{
		return string.Join("\n",
			input.AllKeys
				.Where(d => d != TmaInitDataKeys.Hash)
				.OrderBy(x => x)
				.Select(x => $"{x}={input[x]}"));
	}

	private List<Claim> BuildClaims(NameValueCollection data)
	{
		var claims = new List<Claim>();

		if (string.IsNullOrWhiteSpace(data[TmaInitDataKeys.User]) is false)
		{
			var user = JsonSerializer.Deserialize<TmaInitDataUser>(data[TmaInitDataKeys.User]);

			claims.AddRange(new List<Claim>()
			{
				new(TmaClaimTypes.UserId, user.Id.ToString()),
				new(TmaClaimTypes.FirstName, user.FirstName),
				new(TmaClaimTypes.IsPremium, user.IsPremium.ToString()),
			});

			//Possible empty
			if (string.IsNullOrWhiteSpace(user.UserName) is false)
				claims.Add(new Claim(TmaClaimTypes.UserName, user.UserName));
			
			if (string.IsNullOrWhiteSpace(user.LastName) is false)
				claims.Add(new Claim(TmaClaimTypes.LastName, user.LastName));
		}

		if (string.IsNullOrWhiteSpace(data[TmaInitDataKeys.ChatInstance]) is false)
		{
			claims.Add(new Claim(TmaClaimTypes.ChatInstance, data[TmaInitDataKeys.ChatInstance]));
		}

		return claims;
	}
}