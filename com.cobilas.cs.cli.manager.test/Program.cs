using System;
using System.IO;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

internal partial class Program {

	private static void Main(string[] args) {
		Console.WriteLine("Program.Main");
		Console.WriteLine("Inicializado!");

		//Add a function with an ID for the standard objects.
		CLIParse.AddFunction(1, remove_func);
		CLIParse.AddFunction(2, create_func);
		CLIParse.AddFunction(3, defValueEmpty);

		//Adds the functions recognized by the parser.
		CLIParse.AddToken((long)CLIDefaultToken.Function, "remove", "-r", "create", "-c");

		//Here begins a list of functions, which also serves as a map.
		IFunction[] functions = [
			new DefaultFunction(
				"remove/-r", 1,
				new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIDefaultToken.Argument)}", 3)
			),
			new DefaultFunction(
				"create/-c", 2,
				new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIDefaultToken.Argument)}", 3)
			)
		];

		//Here, the function returns a list of arguments with their respective tokens.
		List<KeyValuePair<string, long>> l = CLIParse.Parse(args);

		// Here, a list of tokens is instantiated based on the list returned by the parser method.
		TokenList list = new(l);
		ErrorMessage message = ErrorMessage.Default;

		// Move the index to the first element.
		list.Move();

		foreach (IFunction item in functions) {
			if (item.IsAlias(list.CurrentKey)) {
				// This is where the analysis is done to detect errors.
				if (item.Analyzer(list, message)) {
					Console.WriteLine($"alz-msm:\r\n{message}");
					break;
				}
				// After the analysis, the list index should be reset and then moved 2 indices forward.
				list.Reset();
				list.Move(2);
				// Here you will find the arguments already discussed.
				if (item.GetValues(list, message)) {
					Console.WriteLine($"msm:\r\n{message}");
					break;
				}
				// Here, the function defined in the constructor is executed.
				item.Run();
				break;
			}
		}
		Console.WriteLine("Finalizado!");
	}

	private static void remove_func(CLIKey key, CLIValueOrder valueOrder) {
		if (key == "remove" || key == "-r") {
			string path = Environment.CurrentDirectory;
			path = Path.Combine(path, valueOrder["arg1"]!);
			if (File.Exists(path)) {
				Console.WriteLine(path);
				File.Delete(path);
			} else Console.WriteLine($"No-exit:{path}");
		}
	}

	private static void create_func(CLIKey key, CLIValueOrder valueOrder) {
		if (key == "create" || key == "-c") {
			string path = Environment.CurrentDirectory;
			path = Path.Combine(path, valueOrder["arg1"]!);
			if (!File.Exists(path)) {
				Console.WriteLine(path);
				File.Create(path).Dispose();
			} else Console.WriteLine($"exit:{path}");
		}
	}

	private static void defValueEmpty(CLIValueOrder valueOrder) { }
}
