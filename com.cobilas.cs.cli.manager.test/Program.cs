using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

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

readonly struct RemoveFunc(string alias, params IOption[] options) : IFunction
{
	private readonly string alias = alias;
	private readonly List<IOption> options = [.. options];

	public string Alias => alias;
	public List<IOption> Options => options;

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

readonly struct Arg : IOption {
	public string Alias => "{ARG}";

	public bool IsAlias(string alias) => alias == Alias;
}

interface IAlias {
	string Alias { get; }
	bool IsAlias(string alias);
}

interface IFunction : IAlias {
	List<IOption> Options { get; }
}

interface IOption : IAlias {
}