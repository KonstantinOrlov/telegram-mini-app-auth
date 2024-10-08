using Microsoft.AspNetCore.Authentication;

namespace Neftm.TelegramMiniApp.Authorization.Models;

/// <summary>
/// Options for setting up Telegram authentication
/// </summary>
public class TmaAuthenticationOptions : AuthenticationSchemeOptions
{
	/// <summary>
	/// Bot token. Required for authenticating.
	/// </summary>
	public string BotToken { get; set; }
	/// <summary>
	/// The name of the HTTP Header where the token will come from
	/// </summary>
	public string TokenHeaderName { get; set; } = "Authorization";
}