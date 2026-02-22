using Cobilas.CLI.Manager.Exceptions;

#pragma warning disable IDE0130 // O namespace não corresponde à estrutura da pasta
namespace System.Collections.Generic;
#pragma warning restore IDE0130 // O namespace não corresponde à estrutura da pasta

internal static class DictionaryExtension {

	public static void Add<key, value>(this Dictionary<key, value>? d, KeyValuePair<key, value> item) {
		ExceptionMessages.ThrowIfNull(d);
		d.Add(item.Key, item.Value);
	}

	public static KeyValuePair<key, value> Find<key, value>(this Dictionary<key, value>? d, Predicate<key>? predicate) {
		ExceptionMessages.ThrowIfNull(d);
		ExceptionMessages.ThrowIfNull(predicate);
		foreach (KeyValuePair<key, value> item in d)
			if (predicate(item.Key))
				return item;
		return default;
	}
}
