using System;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager.Interfaces;

public interface IFunction : IAlias, ICLIAnalyzer {
	List<IOptionFunc> Options { get; }
	CLIValueOrder ValueOrder { get; }

	bool GetValues(TokenList list, ErrorMessage message);
	void Run();
	void Run(Action<CLIKey, CLIValueOrder> action);
}