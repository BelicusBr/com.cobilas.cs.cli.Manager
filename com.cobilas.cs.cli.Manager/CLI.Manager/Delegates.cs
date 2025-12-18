using System.Collections.Generic;

namespace Cobilas.CLI.Manager;

public delegate KeyValuePair<string, long> TokenAnalysis(TokenListReadOnly read, string token);

public delegate bool ParserRule(CLITokenData data, KeyValuePair<string, long> token);
