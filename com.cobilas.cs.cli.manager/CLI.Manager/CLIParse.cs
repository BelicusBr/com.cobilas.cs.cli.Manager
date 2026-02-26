using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;

namespace Cobilas.CLI.Manager;

public static class CLIParse {
	private static readonly Dictionary<string, long> _tokens = [];

	public static long EndCode { get; set; } = (long)CLIToken.EndCode;
	public static long ArgumentCode { get; set; } = (long)CLIToken.Argument;

	public static void AddToken(long tokenID, string? ntoken) {
		ExceptionMessages.ThrowIfNullOrEmpty(ntoken, nameof(ntoken));

		_tokens.Add(ntoken, tokenID);
	}

	public static void AddToken(long tokenID, params string?[]? ntoken) {
		ExceptionMessages.ThrowIfNull(ntoken, nameof(ntoken));

		foreach (string? item in ntoken)
			AddToken(tokenID, item);
	}

	public static List<KeyValuePair<string, long>> Parse(string[]? args) {
		ExceptionMessages.ThrowIfNull(args, nameof(args));

		List<KeyValuePair<string, long>> result = [];

		foreach (string item in args) {
			if (_tokens.TryGetValue(item, out long value))
				result.Add(new(item, value));
			else result.Add(new(item, (long)CLIToken.Argument));
		}
		result.Add(new("(end-f)", (long)CLIToken.EndCode));
		return result;
	}
}
