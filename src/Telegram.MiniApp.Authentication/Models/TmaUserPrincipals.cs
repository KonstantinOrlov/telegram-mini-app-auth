namespace Neftm.TelegramMiniApp.Authorization.Models;

/// <summary>
/// Provides Telegram Mini App user data
/// </summary>
public class TmaUserPrincipals
{
	/// <summary>
	/// Unique identifier for the user or bot
	/// </summary>
	public long UserId { get; init; }
	
	/// <summary>
	/// Username of the user or bot.
	/// </summary>
	public string UserName { get; init; } = string.Empty;
	
	/// <summary>
	/// First name of the user or bot.
	/// </summary>
	public string FirstName { get; init; } = string.Empty;
	
	/// <summary>
	/// Last name of the user or bot.
	/// </summary>
	public string LastName { get; init; } = string.Empty;
	
	/// <summary>
	/// Global identifier, uniquely corresponding to the chat from which the Mini App was opened.
	/// </summary>
	public string ChatInstance { get; init; } = string.Empty;
	
	/// <summary>
	/// True, if this user is a Telegram Premium user.
	/// </summary>
	public bool IsPremium { get; init; }
}