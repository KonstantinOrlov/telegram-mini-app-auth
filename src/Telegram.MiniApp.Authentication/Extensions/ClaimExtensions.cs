using System.Collections.Specialized;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Neftm.TelegramMiniApp.Authorization.Constants;
using Neftm.TelegramMiniApp.Authorization.Models;

namespace Neftm.TelegramMiniApp.Authorization.Extensions;

/// <summary>
/// Extension methods for easy access to Telegram Mini App authentication claims
/// </summary>
public static class ClaimExtensions
{
	/// <summary>
	/// Easy access to TMA authentiocation claims.
	/// </summary>
	/// <param name="user">The <see cref="ClaimsPrincipal"/>.</param>
	/// <param name="result">The <see cref="TmaUserPrincipals"/>.</param>
	/// <returns>/>true if the <see cref="ClaimsPrincipal"/> contains claims of Telegram Mini App authentication; otherwise, false.</returns>
	/// <remarks>
	/// When this method returns, the Telegram mini app authentication claims mapping to result parameter, if they are found;
	/// otherwise, the result parameter is assigned empty TmaUser object. 
	/// </remarks>
	public static bool TryGetTmaUser(this ClaimsPrincipal user, out TmaUserPrincipals? result)
	{
		result = null;
		var claims = user.Identities
			.FirstOrDefault(x => x.AuthenticationType == TmaDefaults.AuthenticationScheme)?
			.Claims
			.ToDictionary(k => k.Type, v => v.Value);

		if (claims is null || claims.Count == 0) return false;

		result = new TmaUserPrincipals()
		{
			UserId = long.TryParse(claims.GetValueOrDefault(TmaClaimTypes.UserId), out var userId) ? userId : default,
			UserName = claims.GetValueOrDefault(TmaClaimTypes.UserName, string.Empty),
			FirstName = claims.GetValueOrDefault(TmaClaimTypes.FirstName, string.Empty),
			LastName = claims.GetValueOrDefault(TmaClaimTypes.LastName, string.Empty),
			ChatInstance = claims.GetValueOrDefault(TmaClaimTypes.ChatInstance, string.Empty),
			IsPremium = bool.TryParse(claims.GetValueOrDefault(TmaClaimTypes.IsPremium), out var isPremium) ? isPremium : default,
		};

		return true;
	}

	/// <summary>
	/// Provides full initData
	/// </summary>
	/// <param name="context">The <see cref="HttpContext"/>.</param>
	/// <param name="result">The <see cref="TmaInitData"/>.</param>
	/// <returns>/>true if the <see cref="FeatureCollection"/> contains TmaInitData object; otherwise, false.</returns>
	/// <remarks>
	/// When this method returns, the result parameter is assigned a TmaInitData object from the FeatureCollection, if they are found;
	/// otherwise, the result parameter is assigned null. 
	/// </remarks>
	public static bool TryGetTmaInitData(this HttpContext context, out TmaInitData? result)
	{
		result = null;
		var feature = context.Features.Get<NameValueCollection>();

		if (feature is null || feature.AllKeys.Length == 0) return false;

		try
		{
			result = ParseInitData(feature);
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static TmaInitData ParseInitData(NameValueCollection input)
	{
		return new TmaInitData()
		{
			QueryId = input[TmaInitDataKeys.QueryId] ?? string.Empty,
			TmaInitDataUser = string.IsNullOrWhiteSpace(input[TmaInitDataKeys.User])
				? null
				: JsonSerializer.Deserialize<TmaInitDataUser>(input[TmaInitDataKeys.User] ?? string.Empty),
			TmaInitDataReceiver = string.IsNullOrWhiteSpace(input[TmaInitDataKeys.Receiver])
				? null
				: JsonSerializer.Deserialize<TmaInitDataUser>(input[TmaInitDataKeys.Receiver] ?? string.Empty),
			Chat = string.IsNullOrWhiteSpace(input[TmaInitDataKeys.Chat])
				? null
				: JsonSerializer.Deserialize<TmaInitDataChat>(input[TmaInitDataKeys.Chat] ?? string.Empty),
			ChatType = input[TmaInitDataKeys.ChatType] ?? string.Empty,
			ChatInstance = input[TmaInitDataKeys.ChatInstance] ?? string.Empty,
			StartParam = input[TmaInitDataKeys.StartParam] ?? string.Empty,
			CanSendAfter = string.IsNullOrWhiteSpace(input[TmaInitDataKeys.CanSendAfter])
				? null
				: DateTimeOffset.FromUnixTimeSeconds(long.Parse(input[TmaInitDataKeys.CanSendAfter] ?? string.Empty)),
			AuthDate = string.IsNullOrWhiteSpace(input[TmaInitDataKeys.AuthDate])
				? null
				: DateTimeOffset.FromUnixTimeSeconds(long.Parse(input[TmaInitDataKeys.AuthDate] ?? string.Empty)),
			Hash = input["hash"] ?? string.Empty,
			Signature = input["signature"] ?? string.Empty,
		};
	}
}