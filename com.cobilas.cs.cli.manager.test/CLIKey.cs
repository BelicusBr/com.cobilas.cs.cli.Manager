using System;
using System.Diagnostics.CodeAnalysis;

readonly struct CLIKey(string alias) : IEquatable<string>, IEquatable<CLIKey> {
	private readonly string[] alias = alias.Split('/', StringSplitOptions.RemoveEmptyEntries);

	public static CLIKeyValue<CLIKey, string> Empty => new(string.Empty, string.Empty);

	public bool Equals(string? other) {
		if (other is null) return false;
		foreach (string item in alias)
			if (item == other)
				return true;
		return false;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
		=> (obj is string stg && Equals(stg)) || (obj is CLIKey k && Equals(k));

	public override int GetHashCode() => base.GetHashCode();

	public bool Equals(CLIKey other) {
		foreach (string item in other.alias)
			if (Equals(item))
				return true;
		return false;
	}

	public override string ToString() => (string)this;

	public static bool operator ==(CLIKey k1, CLIKey k2) => k1.Equals(k2);
	public static bool operator !=(CLIKey k1, CLIKey k2) => !k1.Equals(k2);

	public static bool operator ==(CLIKey k, string s) => k.Equals(s);
	public static bool operator !=(CLIKey k, string s) => !k.Equals(s);
	public static bool operator ==(string s, CLIKey k) => k.Equals(s);
	public static bool operator !=(string s, CLIKey k) => !k.Equals(s);

	public static implicit operator CLIKey(string stg) => new(stg);
	public static implicit operator string(CLIKey key) => string.Join('/', key.alias);
}
