using Cobilas.CLI.Manager.Exceptions;
using System;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager;

public class InfoTokenData : CLITokenData {
	private readonly Dictionary<string, object?> values;

	public object? this[string key] {
		get {
			if (values.TryGetValue(key, out object? value)) return value;
			return null;
		}
		set {
			if (!values.ContainsKey(key))
				values.Add(key, value);
			else values[key] = value;
		}
	}

	public InfoTokenData()
	{
		values = [];
	}

	public T? GetValue<T>(string? key)
		=> GetValue<T>(key, (k) => k == key);


	public T? GetValue<T>(string? key, Predicate<string>? predicate) {
		ExceptionMessages.ThrowIfNullOrEmpty(key);
		ExceptionMessages.ThrowIfNull(predicate);
		object? result = values.Find(predicate).Value;
		return Convert.GetTypeCode(result) switch {
			TypeCode.Empty => default,
			_ => (T?)result
		};
	}

	public void Clear() => values.Clear();
}
