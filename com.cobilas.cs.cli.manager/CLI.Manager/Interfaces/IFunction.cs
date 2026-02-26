using System;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager.Interfaces;
/// <summary>
/// Represents a function in the command-line interface, combining alias behavior, CLI analysis, and execution logic.
/// </summary>
/// <seealso cref="IAlias"/>
/// <seealso cref="ICLIAnalyzer"/>
public interface IFunction : IAlias, ICLIAnalyzer {
	/// <summary>
	/// Gets the value order associated with this function.
	/// </summary>
	/// <returns>The <see cref="CLIValueOrder"/> that defines how values are ordered.</returns>
	CLIValueOrder ValueOrder { get; }
	/// <summary>
	/// Gets the list of option functions associated with this function.
	/// </summary>
	/// <returns>A list of <see cref="IOptionFunc"/> objects representing the options.</returns>
	List<IOptionFunc> Options { get; }
	/// <summary>
	/// Retrieves and processes values from the token list, populating error messages if necessary.
	/// </summary>
	/// <param name="list">The token list to extract values from. Can be null.</param>
	/// <param name="message">The error message container to populate in case of errors. Can be null.</param>
	/// <returns><see langword="true"/> if values are successfully retrieved; otherwise, <see langword="false"/>.</returns>
	bool GetValues(TokenList? list, ErrorMessage? message);
	/// <summary>
	/// Executes the function with its default behavior.
	/// </summary>
	void Run();
	/// <summary>
	/// Executes the function with a custom action that can process key-value pairs and the value order.
	/// </summary>
	/// <param name="action">An action that receives a <see cref="CLIKey"/> and an optional <see cref="CLIValueOrder"/>. Can be null.</param>
	void Run(Action<CLIKey, CLIValueOrder?>? action);
}