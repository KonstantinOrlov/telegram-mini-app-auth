using System.Text.Json.Serialization;

namespace Neftm.TelegramMiniApp.Authorization.Models;

/// <summary>
/// Provides initData
/// </summary>
public class TmaInitData
{
	/// <summary>
	/// Containing data about the current user.
	/// </summary>
	public TmaInitDataUser? TmaInitDataUser { get; init; }
	
	/// <summary>
	/// Global identifier, uniquely corresponding to the chat.
	/// </summary>
	public string ChatInstance { get; init; } = string.Empty;
	
	/// <summary>
	/// Type of the chat.
	/// </summary>
	public string ChatType { get; init; } = string.Empty;
	
	/// <summary>
	/// Time when the form was opened.
	/// </summary>
	public DateTimeOffset? AuthDate { get; init; }
	
	/// <summary>
	/// A hash of all passed parameters.
	/// </summary>
	public string Hash { get; init; } = string.Empty;
}

public class TmaInitDataUser
{
	/// <summary>
	/// A unique identifier for the user or bot.
	/// </summary>
	[JsonPropertyName("id")] public int Id { get; init; }
	
	/// <summary>
	/// First name of the user or bot.
	/// </summary>
	[JsonPropertyName("first_name")] public string FirstName { get; init; } = string.Empty;
	
	/// <summary>
	/// Last name of the user or bot.
	/// </summary>
	[JsonPropertyName("last_name")] public string LastName { get; init; } = string.Empty;
	
	/// <summary>
	/// Username of the user or bot.
	/// </summary>
	[JsonPropertyName("username")] public string UserName { get; init; } = string.Empty;
	
	/// <summary>
	/// IETF language tag. 
	/// </summary>
	[JsonPropertyName("language_code")] public string LanguageCode { get; init; } = string.Empty;
	
	/// <summary>
	/// True, if this user allowed the bot to message them.
	/// </summary>
	[JsonPropertyName("allows_write_to_pm")] public bool AllowsWriteToPm { get; init; }
	
	/// <summary>
	/// True, if this user is a Telegram Premium user.
	/// </summary>
	[JsonPropertyName("is_premium")] public bool IsPremium { get; init; }
}