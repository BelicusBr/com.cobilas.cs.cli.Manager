using System;
using System.IO;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

internal partial class Program {

	private static readonly IFunction[] functions = [
		new DefaultFunction(
			"remove/-r", 1,
			new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIDefaultToken.Argument)}", 2)
		),
		new DefaultFunction(
			"create/-c", 2,
			new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIDefaultToken.Argument)}", 2)
		)
	];

	private static void remove_func(CLIKey key, CLIValueOrder valueOrder) {
		Console.WriteLine($"Run({key})");
		if (key == "remove" || key == "-r") {
			string path = Environment.CurrentDirectory;
			path = Path.Combine(path, valueOrder["arg1"]);
			if (File.Exists(path)) { 
				Console.WriteLine(path);
				File.Delete(path);
			} else Console.WriteLine($"No-exit:{path}");
		}
	}

	private static void create_func(CLIKey key, CLIValueOrder valueOrder) {
		Console.WriteLine($"Run({key})");
		if (key == "create" || key == "-c") {
			string path = Environment.CurrentDirectory;
			path = Path.Combine(path, valueOrder["arg1"]);
			if (!File.Exists(path)) {
				Console.WriteLine(path);
				File.Create(path).Dispose();
			}
			else Console.WriteLine($"No-exit:{path}");
		}
	}

	private static void defValueEmpty(CLIValueOrder valueOrder) { }

	private static void Main(string[] args) {
		Console.WriteLine("Program.Main");
		Console.WriteLine("Inicializado!");

		CLIParse.AddFunction(1, remove_func);
		CLIParse.AddFunction(2, create_func);
		CLIParse.AddFunction(3, defValueEmpty);

		CLIParse.AddToken((long)CLIDefaultToken.Function, "remove", "-r", "create", "-c");

		List<KeyValuePair<string, long>> l = CLIParse.Parse(args);

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
				item.Run();
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
