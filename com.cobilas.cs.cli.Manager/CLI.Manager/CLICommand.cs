using System;

namespace Cobilas.CLI.Manager;

public sealed class CLICommand(CLIFunction? function, params string[]? alias) : IDisposable {
	private string[]? _alias = alias;
	private CLIFunction? function = function;

	public bool IsAlias(string? alias) {
#if NET7_0_OR_GREATER
		ArgumentException.ThrowIfNullOrEmpty(alias, nameof(alias));
		if (_alias is null)
#else
		if (string.IsNullOrEmpty(alias)) 
			throw new ArgumentNullException(nameof(alias));
		else if (_alias is null)
#endif
			throw new NullReferenceException("The alias list is null.");

		foreach (string item in _alias)
			if (item == alias) 
				return true;
		return false;
	}

	public object?[]? Invok(params object?[]? args) => function?.Invoke(args);

	public void Dispose() {
		if (_alias is not null)
#if NET6_0_OR_GREATER
			Array.Clear(_alias);
#else
			Array.Clear(_alias, 0, _alias.Length);
#endif
		function = null;
	}
}
