using System.Collections.Generic;

namespace Cobilas.CLI.Manager;

public static class CLIParse {
	private static readonly Dictionary<string, long> _tokens = [];

	public static void AddToken(string ntoken, long tokenID) => _tokens.Add(ntoken, tokenID);

	public static void AddToken(string ntoken, CLIToken token) => AddToken(ntoken, (long)token);

	public static List<(string, long)> Parse(string[] tokens) {
		List<(string, long)> result = [];
		foreach (string item in tokens) {
			if (_tokens.TryGetValue(item, out long tokenID)) 
				result.Add((item, tokenID));
			else result.Add((item, (long)CLIToken.Text));
		}
		return result;
	}
}
