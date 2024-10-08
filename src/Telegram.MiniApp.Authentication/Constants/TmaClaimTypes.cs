namespace Neftm.TelegramMiniApp.Authorization.Constants;

/// <summary>
/// Defines the claim types used for Telegram Mini App authentication.
/// </summary>
public static class TmaClaimTypes
{
	public const string UserId = "tma_user_id";
	public const string UserName = "tma_username";
	public const string FirstName = "tma_first_name";
	public const string LastName = "tma_last_name";
	public const string ChatInstance = "tma_chat_instance";
	public const string IsPremium = "tma_is_premium";
}