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
	static event Func<string, bool> confis;

	private static void Main(string[] args) {

		CLIParse.EndCode = (long)CLIDefaultToken.EndCode;
		CLIParse.ArgumentCode = (long)CLIDefaultToken.Argument;

		CLIParse.AddToken((long)CLIDefaultToken.Function, "tdsf-1", "tdsf-2");
		CLIParse.AddToken((long)CLIDefaultToken.Option, "tdsO-1", "tdsO-2", "tdsO-3");
		CLIParse.AddToken((long)CLIDefaultToken.Option | CLIParse.EndCode, "tdsO-1E");

		confis += (s) => s == "gagalvi";
		confis += (s) => s == "mastor";
		confis += (s) => s == "nina";
		confis += (s) => s == "gustavi";
		confis += (s) => s == "anastor";

		string res = Console.ReadLine();
		bool result = confis.Invoke(res);
			//.GetInvocationList()
			//.Cast<Func<string, bool>>()
			//.Any(f => f(res));

		if (result) {
			Console.WriteLine($"conf:{res}");
		} else {
			Console.WriteLine($"inconf:{res}");
		}

		//IFunction[] functions = {
		//	new LineFunction("tdsf-1",
		//		new LineEndOption("tdsO-1E", false),
		//		new LineOption("tdsO-1", false, 2),
		//		new LineOption("tdsO-2", true, 0),
		//		new LineArgument("arg{100}", true),

		//		new LineOption("tdsO-3", false, 2),
		//		new LineOption("tdsO-2", true, 0),
		//		new LineArgument("arg{100}", true)
		//	)
		//};

		//TokenList list = new(CLIParse.Parse(args));
		//ErrorMessage message = ErrorMessage.Default;
		//list.Move();

		//foreach (IFunction item in functions) {
		//	if (!item.IsAlias(list.CurrentKey)) continue;
		//	list.Move();
		//	if (item is ICLIAnalyzer alz) {
		//		if (alz.Analyzer(list, message)) {
		//			Console.WriteLine(message);
		//			return;
		//		}
		//	}
		//}
	}
}
