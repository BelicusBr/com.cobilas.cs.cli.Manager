using System;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Represents a key-value pair where the key is of type <typeparamref name="TKey"/> and the value is of type <typeparamref name="TValue"/>.
/// This structure is immutable and implements equality comparison.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <param name="key">The key value.</param>
/// <param name="value">The value associated with the key.</param>
public readonly struct CLIKeyValue<TKey, TValue>(TKey key, TValue value) : IEquatable<CLIKeyValue<TKey, TValue>> {
	private readonly TKey key = key;
	private readonly TValue value = value;

	/// <summary>
	/// Gets the key component of the key-value pair.
	/// </summary>
	/// <returns>The key.</returns>
	public TKey Key => key;
	/// <summary>
	/// Gets the value component of the key-value pair.
	/// </summary>
	/// <returns>The value.</returns>
	public TValue Value => value;
	/// <inheritdoc/>
	public bool Equals(CLIKeyValue<TKey, TValue> other)
		=> EqualityComparer<TKey>.Default.Equals(key, other.key) &&
		   EqualityComparer<TValue>.Default.Equals(value, other.value);
	/// <inheritdoc/>
	public override bool Equals(object? obj)
		=> obj is CLIKeyValue<TKey, TValue> ckv && Equals(ckv);
	/// <inheritdoc/>
	public override int GetHashCode() => base.GetHashCode();
	/// <summary>
	/// Returns a string representation of the key-value pair.
	/// </summary>
	/// <returns>A string in the format "[key, value]".</returns>
	public override string ToString() => $"[{key}, {value}]";
	/// <summary>
	/// Determines whether two <see cref="CLIKeyValue{TKey, TValue}"/> instances are equal.
	/// </summary>
	/// <param name="A">The first instance.</param>
	/// <param name="B">The second instance.</param>
	/// <returns><see langword="true"/> if equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator ==(CLIKeyValue<TKey, TValue> A, CLIKeyValue<TKey, TValue> B) => A.Equals(B);
	/// <summary>
	/// Determines whether two <see cref="CLIKeyValue{TKey, TValue}"/> instances are not equal.
	/// </summary>
	/// <param name="A">The first instance.</param>
	/// <param name="B">The second instance.</param>
	/// <returns><see langword="true"/> if not equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator !=(CLIKeyValue<TKey, TValue> A, CLIKeyValue<TKey, TValue> B) => !A.Equals(B);
}