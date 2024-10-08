using System.Text;

namespace Neftm.TelegramMiniApp.Authorization.Constants;

/// <summary>
/// Provides constants for internal use in the library
/// </summary>
internal static class TmaConstants
{
	
	/// <summary>
	/// Key for calculating telegram hash 
	/// </summary>
	internal static byte[] HashKey = Encoding.UTF8.GetBytes("WebAppData");
}