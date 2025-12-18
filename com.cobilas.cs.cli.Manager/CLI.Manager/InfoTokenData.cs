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

	public void Clear()
	{
		values.Clear();
	}
}
