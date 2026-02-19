using System;
using System.Diagnostics.CodeAnalysis;

namespace Cobilas.CLI.Manager.Exceptions; 

public static class ExceptionMessages {

	public static void ThrowIfDisposable(bool condition, object? instance)
		=> ThrowIfDisposable(condition, instance?.GetType());

	public static void ThrowIfDisposable(bool condition, Type? type) {
#if NET7_0_OR_GREATER
		ObjectDisposedException.ThrowIf(condition, type!);
#else
		if (condition)
			throw new ObjectDisposedException(type?.FullName);
#endif
	}

	public static void ThrowIfNull([NotNull]object? argument, string? paramName = null) {
#if NET6_0_OR_GREATER
		ArgumentNullException.ThrowIfNull(argument, paramName);
#else
		if (argument is null)
			throw new ArgumentNullException(paramName);
#endif
	}

	public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, string? paramName = null) {
#if NET7_0_OR_GREATER
		ArgumentNullException.ThrowIfNullOrWhiteSpace(argument, paramName);
#else
		if (argument is null)
			throw new ArgumentNullException(paramName);
		else if (string.IsNullOrEmpty(argument))
			throw new ArgumentException($"The parameter '{paramName}' is empty.", paramName);
		else if (string.IsNullOrEmpty(argument))
			throw new ArgumentException($"The parameter {paramName} consists only of whitespace characters!", paramName);
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
