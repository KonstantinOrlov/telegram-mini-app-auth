using System.Security.Claims;
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

		if (claims is null || claims.Any() is false) return false;

		result = new TmaUserPrincipals()
		{
			UserId = int.TryParse(claims.GetValueOrDefault(TmaClaimTypes.UserId), out var userId) ? userId : default,
			UserName = claims.GetValueOrDefault(TmaClaimTypes.UserName),
			FirstName = claims.GetValueOrDefault(TmaClaimTypes.FirstName),
			LastName = claims.GetValueOrDefault(TmaClaimTypes.LastName),
			ChatInstance = claims.GetValueOrDefault(TmaClaimTypes.ChatInstance),
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
		try
		{
			result = context.Features.Get<TmaInitData>();
			return true;
		}
		catch (InvalidOperationException e)
		{
			result = null;
			return false;
		}
	}
}