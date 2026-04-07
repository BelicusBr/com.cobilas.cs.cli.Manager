using System;
using System.IO;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.Manager.Patterns.InLine;

internal partial class Program {

	private static void Main(string[] args) {
		CLIParse.EndCode = (long)CLIDefaultToken.EndCode;
		CLIParse.ArgumentCode = (long)CLIDefaultToken.Argument;

		CLIParse.AddToken((long)CLIDefaultToken.Function, "tdsf-1", "tdsf-2");
		CLIParse.AddToken((long)CLIDefaultToken.Option, "tdsO-1", "tdsO-2", "tdsO-3");
		CLIParse.AddToken((long)CLIDefaultToken.Option | CLIParse.EndCode, "tdsO-1E");

		IFunction[] functions = {
			new LineFunction("tdsf-1",
				new LineEndOption("tdsO-1E", false),
				new LineOption("tdsO-1", false, 2),
				new LineOption("tdsO-2", true, 0),
				new LineArgument("arg{100}", true),

				new LineOption("tdsO-3", false, 2),
				new LineOption("tdsO-2", true, 0),
				new LineArgument("arg{100}", true)
			)
		};

		TokenList list = new(CLIParse.Parse(args));
		ErrorMessage message = ErrorMessage.Default;
		list.Move();

		foreach (IFunction item in functions) {
			if (!item.IsAlias(list.CurrentKey)) continue;
			list.Move();
			if (item is ICLIAnalyzer alz) {
				if (alz.Analyzer(list, message)) {
					Console.WriteLine(message);
					return;
				}
			}
		}
	}
}
