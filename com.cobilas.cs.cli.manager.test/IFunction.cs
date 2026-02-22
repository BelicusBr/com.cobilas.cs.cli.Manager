using System;
using System.Collections.Generic;

interface IFunction : IAlias, ICLIAnalyzer {
	List<IOptionFunc> Options { get; }
	Dictionary<CLIKey, string> ValueOrder { get; }

	bool GetValues(TokenList list, ErrorMessage message);
	void Run();
	void Run(Action<CLIKey, Dictionary<CLIKey, string>> action);
}
