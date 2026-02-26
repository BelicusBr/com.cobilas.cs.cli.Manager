using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Provides static methods for parsing command-line arguments into tokens and managing functions.
/// </summary>
public static class CLIParse {
	private static readonly Dictionary<string, long> _tokens = [];
	private static readonly Dictionary<uint, Delegate> _functions = [];
	/// <summary>
	/// Gets or sets the numeric code used to mark the end of input.
	/// </summary>
	/// <value>The end code value. Defaults to <see cref="CLIDefaultToken.EndCode"/>.</value>
	/// <returns>The current end code.</returns>
	public static long EndCode { get; set; } = (long)CLIDefaultToken.EndCode;
	/// <summary>
	/// Gets or sets the numeric code used for unrecognized arguments.
	/// </summary>
	/// <value>The argument code value. Defaults to <see cref="CLIDefaultToken.Argument"/>.</value>
	/// <returns>The current argument code.</returns>
	public static long ArgumentCode { get; set; } = (long)CLIDefaultToken.Argument;
	/// <summary>
	/// Adds a single token string associated with a token ID.
	/// </summary>
	/// <param name="tokenID">The numeric identifier for the token.</param>
	/// <param name="ntoken">The token string. Cannot be null or empty.</param>
	/// <exception cref="ArgumentException">Thrown when <paramref name="ntoken"/> is null or empty.</exception>
	public static void AddToken(long tokenID, string? ntoken) {
		ExceptionMessages.ThrowIfNullOrEmpty(ntoken, nameof(ntoken));
		_tokens.Add(ntoken, tokenID);
	}
	/// <summary>
	/// Adds multiple token strings associated with a single token ID.
	/// </summary>
	/// <param name="tokenID">The numeric identifier for the token.</param>
	/// <param name="ntoken">An array of token strings. Cannot be null.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="ntoken"/> is null.</exception>
	public static void AddToken(long tokenID, params string?[]? ntoken) {
		ExceptionMessages.ThrowIfNull(ntoken, nameof(ntoken));
		foreach (string? item in ntoken)
			AddToken(tokenID, item);
	}
	/// <summary>
	/// Registers a function delegate with a unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier for the function.</param>
	/// <param name="function">The delegate representing the function. Cannot be null.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="function"/> is null.</exception>
	public static void AddFunction(uint id, Delegate? function) {
		ExceptionMessages.ThrowIfNull(function, nameof(function));
		_functions.Add(id, function);
	}
	/// <summary>
	/// Retrieves a function delegate by its identifier.
	/// </summary>
	/// <param name="id">The identifier of the function.</param>
	/// <returns>The registered delegate.</returns>
	public static Delegate GetFunction(uint id) => _functions[id];
	/// <summary>
	/// Retrieves a function delegate of a specific type by its identifier.
	/// </summary>
	/// <typeparam name="TFunc">The delegate type to cast to.</typeparam>
	/// <param name="id">The identifier of the function.</param>
	/// <returns>The registered delegate cast to <typeparamref name="TFunc"/>.</returns>
	public static TFunc GetFunction<TFunc>(uint id) where TFunc : Delegate
		=> (TFunc)_functions[id];
	/// <summary>
	/// Parses an array of command-line arguments into a list of token key-value pairs.
	/// </summary>
	/// <param name="args">The array of argument strings. Cannot be null.</param>
	/// <returns>A list of <see cref="KeyValuePair{String, Long}"/> where the key is the argument and the value is its token ID.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="args"/> is null.</exception>
	public static List<KeyValuePair<string, long>> Parse(string[]? args) {
		ExceptionMessages.ThrowIfNull(args, nameof(args));

		List<KeyValuePair<string, long>> result = [];

		foreach (string item in args) {
			if (_tokens.TryGetValue(item, out long value))
				result.Add(new(item, value));
			else
				result.Add(new(item, (long)CLIDefaultToken.Argument));
		}
		result.Add(new("(end-f)", (long)CLIDefaultToken.EndCode));
		return result;
	}
}