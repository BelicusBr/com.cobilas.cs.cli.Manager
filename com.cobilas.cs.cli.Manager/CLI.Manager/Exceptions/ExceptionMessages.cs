using System;
using System.Diagnostics.CodeAnalysis;

namespace Cobilas.CLI.Manager.Exceptions; 

public static class ExceptionMessages {
	public static void ThrowIfNull([NotNull]object? argument, string? paramName = null) {
#if NET7_0_OR_GREATER
		ArgumentNullException.ThrowIfNull(argument, paramName);
#else
		if (argument is null)
			throw new ArgumentNullException(paramName);
#endif
	}

	public static void ThrowIfNullOrEmpty([NotNull]string? argument, string? paramName = null) {
#if NET7_0_OR_GREATER
		ArgumentNullException.ThrowIfNullOrEmpty(argument, paramName);
#else
		if (argument is null)
			throw new ArgumentNullException(paramName);
		else if (string.IsNullOrEmpty(argument))
			throw new ArgumentException($"The parameter '{paramName}' is empty.", paramName);
#endif
	}
}
