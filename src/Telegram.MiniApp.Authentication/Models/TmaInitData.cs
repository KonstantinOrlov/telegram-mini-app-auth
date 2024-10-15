using System.Text.Json.Serialization;

namespace Neftm.TelegramMiniApp.Authorization.Models;

/// <summary>
/// Provides initData
/// </summary>
public class TmaInitData
{
	/// <summary>
	/// A unique identifier for the Mini App session.
	/// </summary>
	public string QueryId { get; init; } = string.Empty;
	
	/// <summary>
	/// Containing data about the current user.
	/// </summary>
	public TmaInitDataUser? TmaInitDataUser { get; init; }
	
	/// <summary>
	/// An object containing data about the chat partner of the current user in the chat where the bot was launched via the attachment menu. 
	/// </summary>
	public TmaInitDataUser? TmaInitDataReceiver { get; init; }

	/// <summary>
	/// Containing data about the chat where the bot was launched via the attachment menu. 
	/// </summary>
	public TmaInitDataChat? Chat { get; init; }
	
	/// <summary>
	/// Type of the chat.
	/// </summary>
	public string ChatType { get; init; } = string.Empty;
	
	/// <summary>
	/// Global identifier, uniquely corresponding to the chat.
	/// </summary>
	public string ChatInstance { get; init; } = string.Empty;

	/// <summary>
	/// The value of the startattach parameter, passed via link.
	/// </summary>
	public string StartParam { get; init; } = string.Empty;
	
	/// <summary>
	/// Time in seconds, after which a message can be sent via the answerWebAppQuery method.
	/// </summary>
	public DateTimeOffset? CanSendAfter { get; init; }
	
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
	/// True, if this user is a bot.
	/// </summary>
	[JsonPropertyName("is_bot")] public bool IsBot { get; init; }
	
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
	/// True, if this user is a Telegram Premium user.
	/// </summary>
	[JsonPropertyName("is_premium")] public bool IsPremium { get; init; }
	
	/// <summary>
	/// True, if this user added the bot to the attachment menu.
	/// </summary>
	[JsonPropertyName("added_to_attachment_menu")] public bool AddedToAttachmentMenu { get; init; }
	
	/// <summary>
	/// True, if this user allowed the bot to message them.
	/// </summary>
	[JsonPropertyName("allows_write_to_pm")] public bool AllowsWriteToPm { get; init; }
	
	/// <summary>
	/// URL of the user’s profile photo. The photo can be in .jpeg or .svg formats.
	/// </summary>
	[JsonPropertyName("photo_url")] public string PhotoUrl { get; init; } = string.Empty;
}

public class TmaInitDataChat
{
	/// <summary>
	/// Unique identifier for this chat.
	/// </summary>
	[JsonPropertyName("id")] public int Id { get; init; }

	/// <summary>
	/// Type of chat.
	/// </summary>
	[JsonPropertyName("type")] public string ChatType { get; init; } = string.Empty;

	/// <summary>
	/// Title of the chat.
	/// </summary>
	[JsonPropertyName("title")] public string Title { get; init; } = string.Empty;

	/// <summary>
	/// Username of the chat.
	/// </summary>
	[JsonPropertyName("username")] public string Username { get; init; } = string.Empty;

	/// <summary>
	/// URL of the chat’s photo.
	/// </summary>
	[JsonPropertyName("photo_url")] public string PhotoUrl { get; init; } = string.Empty;
}