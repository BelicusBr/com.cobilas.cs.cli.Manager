using System.Collections.Generic;

interface IOptionFunc : IAlias, ICLIAnalyzer {
	bool Mandatory { get; }
	void DefaultValue(Dictionary<CLIKey, string> valueOrder);
	void TreatedValue(Dictionary<CLIKey, string> valueOrder, TokenList list);
	void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage message);
}
