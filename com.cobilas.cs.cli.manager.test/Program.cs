using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

internal class Program {

	private static readonly IFunction[] functions = [
		new DefaultFunction("remove/-r",
				new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIToken.Argument)}")
			),
		new DefaultFunction("add/-a"
				
			)
	];

	private static void Main(string[] args) {
		Console.WriteLine("Program.Main");
		Console.WriteLine("Inicializado!");

		CLIParse.AddToken((long)CLIToken.Option, "--index", "--i");
		CLIParse.AddToken((long)CLIToken.Function, "remove", "-r", "add", "-a");

		List<KeyValuePair<string, long>> l = CLIParse.Parse(args);

		Console.WriteLine("Token list:");
		foreach (var item in l)
			Console.WriteLine($"[{item.Key}, {(CLIToken)item.Value}]");
		Console.WriteLine("\r\n");

		TokenList list = new(l);
		ErrorMessage message = ErrorMessage.Default;
		list.Move();
		if (Analyzer(functions, list, message)) {
			Console.WriteLine($"alz-msm:\r\n{message}");
			return;
		}
		list.Reset();
		list.Move();

		foreach (IFunction item in functions) {
			if (item.IsAlias(list.CurrentKey)) {
				list.Move();
				if(item.GetValues(list, message)) {
					Console.WriteLine($"msm:\r\n{message}");
					return;
				}
				Console.WriteLine("Value Order:");
				foreach (KeyValuePair<CLIKey, string> item1 in ((DefaultFunction)item).ValueOrder)
					Console.WriteLine(item1);
				break;
			}
		}
		Console.WriteLine("Finalizado!");
	}

	private static bool Analyzer(IFunction[] functions, TokenList list, ErrorMessage message) {
		foreach (IFunction item1 in functions) {
			if (item1.IsAlias(list.CurrentKey))
				if (item1.Analyzer(list, message))
					return true;
		}
		return false;
	}
}
