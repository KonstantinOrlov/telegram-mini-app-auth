using System.Security.Cryptography;
using System.Text;
using Neftm.TelegramMiniApp.Authorization.Constants;
using Neftm.TelegramMiniApp.Authorization.Extensions;

namespace Neftm.TelegramMiniApp.Authorization.Helpers;

/// <summary>
/// Provides help methods for hash calculating.
/// </summary>
internal static class HashHelper
{
	/// <summary>
	/// Calculate Tma hash. See Telegram Mini App documentation <see cref="https://core.telegram.org/bots/webapps#validating-data-received-via-the-mini-app"/>
	/// </summary>
	/// <param name="dataCheckString">initData string without hash chain.</param>
	/// <param name="botToken">Telgram Mini App bot token.</param>
	/// <returns>Hash in string format.</returns>
	internal static string CalculateTmaHash(string dataCheckString, string botToken)
	{
		var secretKey = HMACSHA256.HashData(TmaConstants.HashKey, Encoding.UTF8.GetBytes(botToken));
		var hash = HMACSHA256.HashData(secretKey, Encoding.UTF8.GetBytes(dataCheckString));
		return hash.ToHex();
	}
}