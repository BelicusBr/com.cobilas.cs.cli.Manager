using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

internal class Program {

	private static void Main(string[] args) {
		Console.WriteLine("Program.Main");

		CLIParse.AddToken((long)CLIToken.Option, "--index", "--i");
		CLIParse.AddToken((long)CLIToken.Function, "remove", "-r", "add", "-a");

		List<KeyValuePair<string, long>> l = CLIParse.Parse(args);

		foreach (var item in l)
			Console.WriteLine($"[{item.Key}, {(CLIToken)item.Value}]");

		RemoveFunc remove = new("remove/-r",
				new Arg()
			);
	}
}



readonly struct CLIKey(string alias) : IEquatable<string> {
	private readonly string[] alias = alias.Split('/', StringSplitOptions.RemoveEmptyEntries);

	public bool Equals(string? other) {
		if (other is null) return false;
		foreach (string item in alias)
			if (item == other)
				return true;
		return false;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
		=> obj is string stg && Equals(stg);

	public override int GetHashCode() => base.GetHashCode();

	public static bool operator ==(CLIKey k, string s) => k.Equals(s);
	public static bool operator !=(CLIKey k, string s) => !k.Equals(s);
	public static bool operator ==(string s, CLIKey k) => k.Equals(s);
	public static bool operator !=(string s, CLIKey k) => !k.Equals(s);

	public static implicit operator CLIKey(string stg) => new(stg);
	public static implicit operator string(CLIKey key) => string.Join('/', key.alias);
}

readonly struct RemoveFunc(string alias, params IOptionFunc[] options) : IFunction
{
	private readonly string alias = alias;
	private readonly List<IOptionFunc> options = [.. options];

	public string Alias => alias;
	public List<IOptionFunc> Options => options;

	public long TypeCode => throw new NotImplementedException();

	public bool IsAlias(string alias) {
		foreach (var item in alias.Split('/', StringSplitOptions.RemoveEmptyEntries))
			if (alias == item)
				return true;
		return false;
	}

	public void Funct(List<KeyValuePair<string, long>> l) {
		for (int I = 0; I < l.Count; I++) {
			
		}
	}
}

readonly struct Arg(bool mandatory, string alias) : IArgument {
	private readonly CLIKey alias = alias;
	private readonly bool mandatory = mandatory;

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => CLIParse.ArgumentCode;
	public KeyValuePair<CLIKey, string> DefaultValue => throw new NotImplementedException();

	public Arg(string alias) : this(false, alias) { }

	public void ExceptionMessage(out string msm)
	{
		throw new NotImplementedException();
	}

	public bool IsAlias(string alias) => alias == this.alias;

	public KeyValuePair<CLIKey, string> TreatedValue(KeyValuePair<string, long> value)
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
	KeyValuePair<CLIKey, string> DefaultValue { get; }
	void ExceptionMessage(out string msm);
	KeyValuePair<CLIKey, string> TreatedValue(KeyValuePair<string, long> value);
}

interface IOption : IOptionFunc {
	List<IArgument> Options { get; }
}

interface IArgument : IOptionFunc { }