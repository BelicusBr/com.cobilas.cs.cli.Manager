using System;
using System.IO;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.Manager.Patterns.InLine;
using System.Linq;

internal partial class Program {
	/* 0) IOptionFunc.DefaultValue
	 * 1) IOptionFunc.ExceptionMessage
	 * 2) IOptionFunc.TreatedValue
	 * 3) IFunction.GetValues
	 * 4) IFunction.Run
	 * 5) ICLIAnalyzer.Analyzer
	 */

	private static void Main(string[] args) {

		CLIParse.EndCode = (long)CLIDefaultToken.EndCode;
		CLIParse.ArgumentCode = (long)CLIDefaultToken.Argument;

		CLIParse.AddToken((long)CLIDefaultToken.Function, "tdsf-1", "tdsf-2");
		CLIParse.AddToken((long)CLIDefaultToken.Option, "tdsO-1", "tdsO-2", "tdsO-3");
		CLIParse.AddToken((long)CLIDefaultToken.Option | CLIParse.EndCode, "tdsO-1E");

		CLIParse.AddFunction(0u, def_value);
		CLIParse.AddFunction(1u, error_value);
		CLIParse.AddFunction(2u, get_value);
		CLIParse.AddFunction(3u, LineFunction.FunctionGetValues);
		CLIParse.AddFunction(5u, LineFunction.FunctionAnalyzer);

		IFunction[] functions = {
			new LineFunction("tdsf-1",
				new LineEndOption("tdsO-1E", false),
				new LineOption("tdsO-1", false, 2),
				new LineOption("tdsO-2", true, 0),
				new LineArgument("arg{100}", true),

				new LineOption("tdsO-3", false, 2),
				new LineOption("tdsO-2", true, 0),
				new LineArgument("arg{100}", true)
			),
			new LineFunction("tdsf-2",
				new LineEndOption("tdsO-1E", false),
				new LineOption("tdsO-1", false, 2),
				new LineOption("tdsO-2", true, 0),
				new LineArgument("arg{100}", true),

				new LineOption("tdsO-3", false, 2),
				new LineOption("tdsO-2", true, 0),
				new LineArgument("arg{100}", true)
			)
		};

		TokenList list = new(CLIParse.Parse(Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries)));
		ErrorMessage message = ErrorMessage.Default;
		list.Move();

		foreach (IFunction item in functions) {
			if (!item.IsAlias(list.CurrentKey)) continue;
			if (item is ICLIAnalyzer alz) {
				if (alz.Analyzer(list, message)) {
					Console.WriteLine(message);
					return;
				}
			}
			list.Reset();
			list.Move();
			if (item.GetValues(list, message)) {
				Console.WriteLine(message);
				return;
			}

			foreach (var item2 in item.ValueOrder)
				Console.WriteLine(item2);
		}
	}

	private static void def_value(CLIKey alias, CLIValueOrder? valueOrder, ErrorMessage? message) {
		if (alias == (CLIKey)"arg{100}")
			valueOrder.Add((CLIKey)"arg{100}", Environment.OSVersion.ToString());
		else valueOrder.Add(alias, $"def-arg-{alias}");
	}
	private static void error_value(CLIKey alias, KeyValuePair<string, long> value, ErrorMessage? message) { }
	private static void get_value(CLIKey alias, CLIValueOrder? valueOrder, TokenList? list, ErrorMessage? message) {
		if (alias == (CLIKey)"arg{100}")
			valueOrder.Add((CLIKey)"arg{100}", list.CurrentKey);
		else valueOrder.Add(list.CurrentKey, $"arg-{list.CurrentKey}");
	}
}
