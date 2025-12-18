using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;

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

#pragma warning disable CS8604 // Possível argumento de referência nula.
		_tokens.Add(ntoken, tokenID);
#pragma warning restore CS8604 // Possível argumento de referência nula.
	}

	public static void AddToken(long tokenID, params string?[]? ntoken) {
		ExceptionMessages.ThrowIfNull(ntoken, nameof(ntoken));

#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
		foreach (string? item in ntoken)
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
			AddToken(tokenID, item);
	}

	public static TokenList Parse(string[]? args) {
		ExceptionMessages.ThrowIfNull(args, nameof(args));

		Analysis ??= IAnalysis;
		TokenList tokens = [];
		TokenListReadOnly read = new(_tokens);
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
		foreach (string item in args) {
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
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
