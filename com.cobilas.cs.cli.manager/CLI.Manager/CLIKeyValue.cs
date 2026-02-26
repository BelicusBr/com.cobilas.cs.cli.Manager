using System;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager;

public readonly struct CLIKeyValue<TKey, TValue>(TKey key, TValue value) :
	IEquatable<CLIKeyValue<TKey, TValue>> {
	private readonly TKey key = key;
	private readonly TValue value = value;

	public TKey Key => key;
	public TValue Value => value;

	public bool Equals(CLIKeyValue<TKey, TValue> other)
		=> EqualityComparer<TKey>.Default.Equals(key, other.key) &&
		EqualityComparer<TValue>.Default.Equals(value, other.value);

	public override bool Equals(object? obj)
		=> obj is CLIKeyValue<TKey, TValue> ckv && Equals(ckv);

	public override int GetHashCode() => base.GetHashCode();

	public override string ToString() => $"[{key}, {value}]";

	public static bool operator ==(CLIKeyValue<TKey, TValue> A, CLIKeyValue<TKey, TValue> B) => A.Equals(B);
	public static bool operator !=(CLIKeyValue<TKey, TValue> A, CLIKeyValue<TKey, TValue> B) => !A.Equals(B);
}