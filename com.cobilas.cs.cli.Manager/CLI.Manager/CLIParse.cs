using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Collections;

namespace Cobilas.CLI.Manager;

public static class CLIParse {
	private static readonly Dictionary<string, long> _tokens = [];

	public static bool IsAutoException { get; set; }
	public static ParserRule? Rule { get; set; } = null;
	public static TokenAnalysis? Analysis { get; set; } = IAnalysis;
	public static long EndCode { get; set; } = (long)CLIToken.EndCode;
	public static long ArgumentCode { get; set; } = (long)CLIToken.Argument;
	public static CLITokenData TokenData { get; set; } = CLITokenData.Default;

	public static void AddToken(long tokenID, string? ntoken) {
		ExceptionMessages.ThrowIfNullOrEmpty(ntoken, nameof(ntoken));

		_tokens.Add(ntoken, tokenID);
	}

	public static void AddToken(long tokenID, params string?[]? ntoken) {
		ExceptionMessages.ThrowIfNull(ntoken, nameof(ntoken));

		foreach (string? item in ntoken)
			AddToken(tokenID, item);
	}

	public static TokenList Parse(string[]? args) {
		ExceptionMessages.ThrowIfNull(args, nameof(args));

		Analysis ??= IAnalysis;
		TokenList tokens = [];
		TokenListReadOnly read = new(_tokens);
		foreach (string item in args) {
			KeyValuePair<string, long> temp = Analysis(read, item);

			if (Rule is not null)
				if (Rule(TokenData, temp)) {
					if (IsAutoException)
						throw TokenData.Exception ?? NotDescribedException.RuleViolated;
					break;
				}

			tokens.Add(temp);
		}
		tokens.Add("(end-f)", EndCode);
		return tokens;
	}

	private static KeyValuePair<string, long> IAnalysis(TokenListReadOnly read, string token) {
		if (read.TryGetValue(token, out long id))
			return new KeyValuePair<string, long>(token, id);
		return new KeyValuePair<string, long>(token, ArgumentCode);
	}
}
