using System;
using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Collections;
using Cobilas.CLI.Manager.Exceptions;

internal class Program {
	/*
	remove/-r arg
	add/-a arg 
	add/-a arg --index/--i arg
	 */
	private static void Main(string[] args) {

		TokenExecutor removeF = new();
		TokenExecutor addF = new();

		removeF.AddTokenInfo("remove/-r", 1, (long)CLIToken.Function);
		addF.AddTokenInfo("add/-a", 1, (long)CLIToken.Function);
		addF.AddTokenInfo("--index/--i", 1, (long)CLIToken.Option);

		Console.WriteLine("Program.Main");
		CLIParse.IsAutoException = true;
		CLIParse.Rule = (c, t) => {
			InfoTokenData data = (InfoTokenData)c;
			if (t.Value == (long)CLIToken.Function) {
				object? funcC = data["funcC"];
				if (funcC == null) data["funcC"] = 1L;
				else if ((long)funcC == 1) {
					data.Exception = new InvalidCLIFunctionException();
					return true; 
				}
			} else if (t.Value == (long)CLIToken.Option) {
				object? funcC = data["funcC"];
				if (funcC is null || (long)funcC == 0) {
					data.Exception = new InvalidCLIOptionException();
					return true;
				}
			}
			return false;
		};

		AddToken();

		TokenList l = CLIParse.Parse(args);

		foreach (var item in l)
		{
			Console.WriteLine($"[{item.Key}, {(CLIToken)item.Value}]");
		}

		addF.Invok(l, (t, d, i) => {
			
		});
	}

	private static void AddToken() {

		CLIParse.AddToken((long)CLIToken.Function, "remove", "-r", "add", "-a");
		CLIParse.AddToken((long)CLIToken.Option, "--index", "--i");

	}
}