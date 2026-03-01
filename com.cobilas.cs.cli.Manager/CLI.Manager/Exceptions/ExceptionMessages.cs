using System;
using System.Diagnostics.CodeAnalysis;

namespace Cobilas.CLI.Manager.Exceptions;
/// <summary>
/// Provides centralized exception throwing helper methods with conditional compilation
/// for different .NET versions.
/// </summary>
public static class ExceptionMessages {
	/// <summary>
	/// Throws an <see cref="ObjectDisposedException"/> if the specified condition is <see langword="true"/>.
	/// </summary>
	/// <param name="condition">The condition to evaluate.</param>
	/// <param name="instance">The object instance that may be disposed. Used to infer the type name.</param>
	/// <exception cref="ObjectDisposedException">Thrown when <paramref name="condition"/> is <see langword="true"/>.</exception>
	public static void ThrowIfDisposable(bool condition, object? instance)
		=> ThrowIfDisposable(condition, instance?.GetType());
	/// <summary>
	/// Throws an <see cref="ObjectDisposedException"/> if the specified condition is <see langword="true"/>.
	/// </summary>
	/// <param name="condition">The condition to evaluate.</param>
	/// <param name="type">The type of the object that may be disposed. Its full name is used in the exception message.</param>
	/// <exception cref="ObjectDisposedException">Thrown when <paramref name="condition"/> is <see langword="true"/>.</exception>
	public static void ThrowIfDisposable(bool condition, Type? type)
	{
#if NET7_0_OR_GREATER
		ObjectDisposedException.ThrowIf(condition, type!);
#else
        if (condition)
            throw new ObjectDisposedException(type?.FullName);
#endif
	}
	/// <summary>
	/// Throws an <see cref="ArgumentNullException"/> if the specified argument is <see langword="null"/>.
	/// </summary>
	/// <param name="argument">The argument to check for null.</param>
	/// <param name="paramName">The name of the parameter that caused the exception.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="argument"/> is <see langword="null"/>.</exception>
	public static void ThrowIfNull([NotNull] object? argument, string? paramName = null)
	{
#if NET6_0_OR_GREATER
		ArgumentNullException.ThrowIfNull(argument, paramName);
#else
        if (argument is null)
            throw new ArgumentNullException(paramName);
#endif
	}
	/// <summary>
	/// Throws an exception if the specified string argument is <see langword="null"/>,
	/// empty, or consists only of white-space characters.
	/// </summary>
	/// <param name="argument">The string argument to validate.</param>
	/// <param name="paramName">The name of the parameter that caused the exception.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="argument"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="argument"/> is empty or white-space.</exception>
	public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, string? paramName = null)
	{
#if NET7_0_OR_GREATER
		ArgumentNullException.ThrowIfNullOrWhiteSpace(argument, paramName);
#else
        if (argument is null)
            throw new ArgumentNullException(paramName);
        else if (string.IsNullOrEmpty(argument))
            throw new ArgumentException($"The parameter '{paramName}' is empty.", paramName);
        else if (string.IsNullOrWhiteSpace(argument))
            throw new ArgumentException($"The parameter {paramName} consists only of whitespace characters!", paramName);
#endif
	}
	/// <summary>
	/// Throws an exception if the specified string argument is <see langword="null"/> or empty.
	/// </summary>
	/// <param name="argument">The string argument to validate.</param>
	/// <param name="paramName">The name of the parameter that caused the exception.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="argument"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="argument"/> is empty.</exception>
	public static void ThrowIfNullOrEmpty([NotNull] string? argument, string? paramName = null)
	{
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