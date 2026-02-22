using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

internal class Program {

	private static void Main(string[] args) {
		Console.WriteLine("Program.Main");

		CLIParse.ArgumentCode = (long)CLIToken.Argument;
		CLIParse.AddToken((long)CLIToken.Option, "--index", "--i");
		CLIParse.AddToken((long)CLIToken.Function, "remove", "-r", "add", "-a");

		List<KeyValuePair<string, long>> l = CLIParse.Parse(args);

		foreach (var item in l)
			Console.WriteLine($"[{item.Key}, {(CLIToken)item.Value}]");

		RemoveFunc remove = new("remove/-r",
				new Arg(true, "arg1/{ARG}")
			);

		remove.Funct(l, out string msm);
		Console.WriteLine($"msm:{msm}");
		foreach (KeyValuePair<CLIKey, string> item in remove.ValueOrder)
			Console.WriteLine(item);
	}
}

readonly struct CLIKeyValue<TKey, TValue>(TKey key, TValue value) : 
	IEquatable<CLIKeyValue<TKey, TValue>> {
	private readonly TKey key = key;
	private readonly TValue value = value;

	public TKey Key => key;
	public TValue Value => value;

	public bool Equals(CLIKeyValue<TKey, TValue> other)
		=> EqualityComparer<TKey>.Default.Equals(key, other.key) &&
		EqualityComparer<TValue>.Default.Equals(value, other.value);

	public override bool Equals([NotNullWhen(true)] object? obj)
		=> obj is CLIKeyValue<TKey, TValue> ckv && Equals(ckv);

	public override int GetHashCode() => base.GetHashCode();

	public override string ToString() => $"[{key}, {value}]";

	public static bool operator ==(CLIKeyValue<TKey, TValue> A, CLIKeyValue<TKey, TValue> B) => A.Equals(B);
	public static bool operator !=(CLIKeyValue<TKey, TValue> A, CLIKeyValue<TKey, TValue> B) => !A.Equals(B);
}

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

readonly struct RemoveFunc(string alias, params IOptionFunc[] options) : IFunction {
	private readonly string alias = alias;
	private readonly List<IOptionFunc> options = [.. options];
	private readonly Dictionary<CLIKey, string> valueOrder = [];

	public string Alias => alias;
	public List<IOptionFunc> Options => options;
	public Dictionary<CLIKey, string> ValueOrder => valueOrder;

	public long TypeCode => throw new NotImplementedException();

	public bool IsAlias(string alias) {
		foreach (var item in alias.Split('/', StringSplitOptions.RemoveEmptyEntries))
			if (alias == item)
				return true;
		return false;
	}

	public void Funct(List<KeyValuePair<string, long>> l, out string msm) {
		msm = string.Empty;
		for (int I = 0, C = 1; I < options.Count; I++) {
			IOptionFunc of = options[I];
			KeyValuePair<string, long> t = l[C];

			CLIKeyValue<CLIKey, string> temp = CLIKey.Empty;
			if (of.TypeCode == t.Value) {
				if (of.IsAlias(t.Key) || of.IsAlias("{ARG}")) temp = of.TreatedValue(t);
				if (temp != CLIKey.Empty) {
					valueOrder.Add(temp.Key, temp.Value);
					C++;
				}
			} else {
				if (of.Mandatory) {
					of.ExceptionMessage(out msm);
					break;
				} else {
					temp = of.DefaultValue;
					valueOrder.Add(temp.Key, temp.Value);
				}
			}
		}
	}
}

readonly struct Arg(bool mandatory, string alias) : IArgument {
	private readonly CLIKey alias = alias;
	private readonly bool mandatory = mandatory;

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => CLIParse.ArgumentCode;
	public CLIKeyValue<CLIKey, string> DefaultValue => throw new NotImplementedException();

	public Arg(string alias) : this(false, alias) { }

	public void ExceptionMessage(out string msm) {
		msm = "Arg invalido!!!";
	}

	public bool IsAlias(string alias) => alias == this.alias;

	public CLIKeyValue<CLIKey, string> TreatedValue(KeyValuePair<string, long> value)
		=> new(alias, value.Key);
}

interface IAlias {
	string Alias { get; }
	long TypeCode { get; }
	bool IsAlias(string alias);
}

interface IFunction : IAlias {
	List<IOptionFunc> Options { get; }
}

interface IOptionFunc : IAlias {
	bool Mandatory { get; }
	CLIKeyValue<CLIKey, string> DefaultValue { get; }
	void ExceptionMessage(out string msm);
	CLIKeyValue<CLIKey, string> TreatedValue(KeyValuePair<string, long> value);
}

interface IOption : IOptionFunc {
	List<IArgument> Options { get; }
}

interface IArgument : IOptionFunc { }