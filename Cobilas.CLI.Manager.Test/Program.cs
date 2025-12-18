using System;
using System.Linq;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

internal class Program {

	private static void Main(string[] args) {
		Console.WriteLine("Program.Main");
		CLIParse.IsAutoException = false;
		CLIParse.Rule = (c, t) => {
			if (t.Value == (long)CLIToken.Function) {
				InfoTokenData data = (InfoTokenData)c;
				object? funcC = data["funcC"];
				if (funcC == null) data["funcC"] = 1L;
				else if ((long)funcC == 1) return true;
			}
			return false;
		};

		AddToken();

		foreach (var item in CLIParse.Parse(args))
		{
			Console.WriteLine(item);
		}
	}

	private static void AddToken() {

		CLIParse.AddToken((long)CLIToken.Function, "remove", "-r", "add", "-a");
		CLIParse.AddToken((long)CLIToken.Option, "-file", "--fl", "-folder", "--fd");

	}
}