using Microsoft.AspNetCore.Authentication;
using Neftm.TelegramMiniApp.Authorization.Models;

namespace Neftm.TelegramMiniApp.Authorization;

/// <summary>
/// Extension methods to configure Telegram Mini App authentication.
/// </summary>
public static class AuthenticationBuilderExtensions
{
	/// <summary>
	/// Adds Telegram Mini App authentication to the specified <see cref="AuthenticationBuilder"/>.
	/// </summary>
	/// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
	/// <param name="options">A delegate to configure the telegram mini app authentication"/>.</param>
	/// <returns>Updated <see cref="AuthenticationBuilder"/>.</returns>
	/// <remarks>
	/// This extension method sets up Telegram Mini App authentication with the specified options configuration.
	/// The method is used to integrate Telegram Mini App as an authentication provider in authentication system.
	/// </remarks>
	public static AuthenticationBuilder AddTelegramMiniAppInHeader(this AuthenticationBuilder builder, string authenticationScheme, Action<TmaAuthenticationOptions> options)
	{
		return builder.AddScheme<TmaAuthenticationOptions, TmaAuthenticationHandler>(authenticationScheme, options);
	}
}