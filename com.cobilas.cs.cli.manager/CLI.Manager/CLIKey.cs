using System;
using Cobilas.CLI.Manager.Exceptions;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Represents a compound key that can match multiple alias strings, typically used for CLI tokens.
/// </summary>
public readonly struct CLIKey : IEquatable<string>, IEquatable<CLIKey> {
	private readonly string[] alias;
	/// <summary>
	/// The characters used to split a combined alias string into individual aliases.
	/// </summary>
	public static readonly char[] separator = ['/'];
	/// <summary>
	/// Gets an empty <see cref="CLIKeyValue{CLIKey, String}"/> with empty key and value.
	/// </summary>
	/// <returns>An empty key-value pair where the key is an empty string and the value is an empty string.</returns>
	public static CLIKeyValue<CLIKey, string> Empty => new(string.Empty, string.Empty);
	/// <summary>
	/// Initializes a new <see cref="CLIKey"/> by splitting the provided alias string using <see cref="separator"/>.
	/// </summary>
	/// <param name="alias">A combined alias string containing multiple aliases separated by '/'. Cannot be null or empty.</param>
	/// <exception cref="ArgumentException">Thrown when <paramref name="alias"/> is null or empty.</exception>
	public CLIKey(string? alias) {
		ExceptionMessages.ThrowIfNullOrEmpty(alias, nameof(alias));
		this.alias = alias.Split(separator, StringSplitOptions.RemoveEmptyEntries);
	}
	/// <summary>
	/// Determines whether this key matches the specified string.
	/// </summary>
	/// <param name="other">The string to compare.</param>
	/// <returns><see langword="true"/> if any of the internal aliases equal <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
	public bool Equals(string? other) {
		if (other is null) return false;
		foreach (string item in alias)
			if (item == other)
				return true;
		return false;
	}
	/// <inheritdoc/>
	public override bool Equals(object? obj)
		=> obj is string stg && Equals(stg) || obj is CLIKey k && Equals(k);
	/// <inheritdoc/>
	public override int GetHashCode() => base.GetHashCode();
	/// <summary>
	/// Determines whether this key equals another <see cref="CLIKey"/>.
	/// </summary>
	/// <param name="other">The other key to compare.</param>
	/// <returns><see langword="true"/> if any alias in this key matches any alias in <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
	public bool Equals(CLIKey other) {
		foreach (string item in other.alias)
			if (Equals(item))
				return true;
		return false;
	}
	/// <summary>
	/// Returns a string representation of the key by joining all aliases with the separator.
	/// </summary>
	/// <returns>A string containing all aliases joined by '/'.</returns>
	public override string ToString() => (string)this;
	/// <summary>
	/// Compares two <see cref="CLIKey"/> instances for equality.
	/// </summary>
	/// <param name="k1">First key.</param>
	/// <param name="k2">Second key.</param>
	/// <returns><see langword="true"/> if they are equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator ==(CLIKey k1, CLIKey k2) => k1.Equals(k2);
	/// <summary>
	/// Compares two <see cref="CLIKey"/> instances for inequality.
	/// </summary>
	/// <param name="k1">First key.</param>
	/// <param name="k2">Second key.</param>
	/// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator !=(CLIKey k1, CLIKey k2) => !k1.Equals(k2);
	/// <summary>
	/// Compares a <see cref="CLIKey"/> with a string for equality.
	/// </summary>
	/// <param name="k">The key.</param>
	/// <param name="s">The string.</param>
	/// <returns><see langword="true"/> if the key matches the string; otherwise, <see langword="false"/>.</returns>
	public static bool operator ==(CLIKey k, string s) => k.Equals(s);
	/// <summary>
	/// Compares a <see cref="CLIKey"/> with a string for inequality.
	/// </summary>
	/// <param name="k">The key.</param>
	/// <param name="s">The string.</param>
	/// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator !=(CLIKey k, string s) => !k.Equals(s);
	/// <summary>
	/// Compares a string with a <see cref="CLIKey"/> for equality.
	/// </summary>
	/// <param name="s">The string.</param>
	/// <param name="k">The key.</param>
	/// <returns><see langword="true"/> if they are equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator ==(string s, CLIKey k) => k.Equals(s);
	/// <summary>
	/// Compares a string with a <see cref="CLIKey"/> for inequality.
	/// </summary>
	/// <param name="s">The string.</param>
	/// <param name="k">The key.</param>
	/// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator !=(string s, CLIKey k) => !k.Equals(s);
	/// <summary>
	/// Implicitly converts a string to a <see cref="CLIKey"/>.
	/// </summary>
	/// <param name="stg">The string to convert.</param>
	/// <returns>A new <see cref="CLIKey"/> instance.</returns>
	public static implicit operator CLIKey(string stg) => new(stg);
	/// <summary>
	/// Implicitly converts a <see cref="CLIKey"/> to a string by joining its aliases with '/'.
	/// </summary>
	/// <param name="key">The key to convert.</param>
	/// <returns>A string representation of the key.</returns>
	public static implicit operator string(CLIKey key) => string.Join("/", key.alias ?? []);
}