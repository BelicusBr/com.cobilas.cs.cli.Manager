using Cobilas.CLI.Manager.Exceptions;

#pragma warning disable IDE0130
namespace System.Collections.Generic;
#pragma warning restore IDE0130

internal static class DictionaryExtension {

	// Mantido apenas pelo null-check — comportamento idêntico ao nativo caso d não seja null
	internal static void Add<TKey, TValue>(
		this Dictionary<TKey, TValue>? d,
		KeyValuePair<TKey, TValue> item) where TKey : notnull {
		ExceptionMessages.ThrowIfNull(d);
		d.Add(item.Key, item.Value);
	}

	// Retorna bool + out: padrão Try* do .NET, sem retorno silencioso de default
	internal static bool TryFind<TKey, TValue>(
		this Dictionary<TKey, TValue>? d,
		Predicate<TKey>? predicate,
		out KeyValuePair<TKey, TValue> result) where TKey : notnull {
		ExceptionMessages.ThrowIfNull(d);
		ExceptionMessages.ThrowIfNull(predicate);

		foreach (KeyValuePair<TKey, TValue> item in d)
			if (predicate(item.Key)) {
				result = item;
				return true;
			}

		result = default;
		return false;
	}

	// Versão que lança exceção se não encontrar — para casos onde ausência é erro
	internal static KeyValuePair<TKey, TValue> Find<TKey, TValue>(
		this Dictionary<TKey, TValue>? d,
		Predicate<TKey>? predicate) where TKey : notnull {
		if (!d.TryFind(predicate, out var result))
			throw new KeyNotFoundException("No key matched the given predicate.");
		return result;
	}
}