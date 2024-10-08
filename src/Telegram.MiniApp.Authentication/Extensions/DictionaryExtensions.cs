namespace Neftm.TelegramMiniApp.Authorization.Extensions;

/// <summary>
/// Provides extension methods for IDictionary
/// For internal use in the library
/// </summary>
internal static class DictionaryExtensions
{
	/// <summary>
	/// Gets the value associated with the specified key
	/// </summary>
	/// <param name="dictionary">Input dictionary.</param>
	/// <param name="key">The key whose value to get.</param>
	/// <param name="defaultValue">Return this value, if key is not found. Optional parameter.</param>
	/// <returns>/>The value associated with the specified key, if the key is found;
	/// otherwise, the default value for the type of the value parameter, or the value specified in the defaultValue parameter.</returns>
	public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default!)
	{
		return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
	}
}