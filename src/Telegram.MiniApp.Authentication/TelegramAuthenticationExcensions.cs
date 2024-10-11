using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Neftm.TelegramMiniApp.Authorization.Models;

namespace Neftm.TelegramMiniApp.Authorization;

/// <summary>
/// Extension methods to configure Telegram Mini App authentication.
/// </summary>
public static class TelegramAuthenticationExcensions
{
	/// <summary>
	/// Adds Telegram Mini App authentication to the specified <see cref="IServiceCollection"/>.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/>.</param>
	/// <param name="options">A delegate to configure the telegram mini app authentication"/>.</param>
	/// <returns>Updated <see cref="IServiceCollection"/>.</returns>
	/// <remarks>
	/// This extension method sets up Telegram Mini App authentication with the specified options configuration.
	/// The method is used to integrate Telegram Mini App as an authentication provider in authentication system.
	/// </remarks>
	public static IServiceCollection AddTelegramMiniAppInHeader(this IServiceCollection services, Action<TmaAuthenticationOptions> options)
	{
		var optionsValue = new TmaAuthenticationOptions();
		options.Invoke(optionsValue);

		if (string.IsNullOrWhiteSpace(optionsValue.BotToken) ||
		    Regex.IsMatch(optionsValue.BotToken, @"^\d+:.+$") is false)
			throw new ArgumentException("Invalid Telegram bot token");
		
        services.AddAuthentication(TmaDefaults.AuthenticationScheme)
	        .AddScheme<TmaAuthenticationOptions, TmaAuthenticationHandler>(TmaDefaults.AuthenticationScheme, options);

        return services;
	}
}