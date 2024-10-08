using Microsoft.AspNetCore.Authorization;
using Neftm.TelegramMiniApp.Authorization.Constants;

namespace Neftm.TelegramMiniApp.Authorization;

/// <summary>
/// Extension methods to add default telegram Mini app authorization policy
/// </summary>
public static class AuthorizationOptionsExtensions
{
	/// <summary>
	/// Add Telegram Mini App authorization policy to the specified <see cref="AuthorizationOptions"/>.
	/// </summary>
	/// <param name="options">The <see cref="AuthorizationOptions"/>.</param>
	/// <remarks>
	/// This extension method adds Telegram Mini App policy with default settings.
	/// </remarks>
	public static void AddDefaultTmaPolicy(this AuthorizationOptions options)
	{
		options.AddPolicy(TmaDefaults.DefaultPolicy, policy =>
		{
			policy.AddAuthenticationSchemes(TmaDefaults.AuthenticationScheme);
			policy.RequireClaim(TmaClaimTypes.UserId);
		});
	}
	
	/// <summary>
	/// Add Telegram Mini App authorization policy to the specified <see cref="AuthorizationOptions"/>.
	/// </summary>
	/// <param name="options">The <see cref="AuthorizationOptions"/>.</param>
	/// <remarks>
	/// This extension method adds Telegram Mini App authorization policy for users with telegram premium.
	/// This policy ensures that only premium users can access certain secured resources.
	/// </remarks>
	public static void AddTmaPremiumPolicy(this AuthorizationOptions options)
	{
		options.AddPolicy(TmaDefaults.PremiumPolicy, policy =>
		{
			policy.AddAuthenticationSchemes(TmaDefaults.AuthenticationScheme);
			policy.RequireClaim(TmaClaimTypes.UserId);
			policy.RequireClaim(TmaClaimTypes.IsPremium, "true");
		});
	}
}