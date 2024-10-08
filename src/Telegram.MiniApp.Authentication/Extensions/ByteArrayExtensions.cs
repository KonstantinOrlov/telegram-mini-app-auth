using System.Text;

namespace Neftm.TelegramMiniApp.Authorization.Extensions;

/// <summary>
/// Provides extension methods for byte[]
/// For internal use in the library
/// </summary>
internal static class ByteArrayExtensions
{
	
	/// <summary>
	/// Convert byte array to Hex string
	/// </summary>
	/// <param name="input">Input byte array.</param>
	/// <param name="upperCase">Upper case switch flag.</param>
	/// <returns>/>Hex string</returns>

	internal static string ToHex(this byte[] input, bool upperCase = false)
	{
		var result = new StringBuilder(input.Length*2);
		
		foreach (var b in input)
			result.Append(b.ToString(upperCase ? "X2" : "x2"));
		
		return result.ToString();
	}
}