using System.Collections.Generic;

namespace Cobilas.CLI.Manager.Interfaces;

public interface IOptionFunc : IAlias, ICLIAnalyzer {
	bool Mandatory { get; }
	void DefaultValue(CLIValueOrder valueOrder);
	void TreatedValue(CLIValueOrder valueOrder, TokenList list);
	void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage message);
}