using System.Collections.Generic;
using Cobilas.CLI.Manager.Collections;

namespace Cobilas.CLI.Manager;

public delegate KeyValuePair<string, long> TokenAnalysis(TokenListReadOnly read, string token);

public delegate bool ParserRule(CLITokenData data, KeyValuePair<string, long> token);

public delegate void TokenExecutorFunction(KeyValuePair<string, long> token, InfoTokenData data, InfoTokenData tokenListData);
