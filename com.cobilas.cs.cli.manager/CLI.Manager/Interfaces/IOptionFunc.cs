using System.Collections.Generic;

namespace Cobilas.CLI.Manager.Interfaces;
/// <summary>
/// Defines a contract for an option function that combines alias behavior, CLI analysis, and value handling.
/// </summary>
/// <seealso cref="IAlias"/>
public interface IOptionFunc : IAlias {
	/// <summary>
	/// Gets a value indicating whether this option function is mandatory.
	/// </summary>
	/// <returns><see langword="true"/> if the option is mandatory; otherwise, <see langword="false"/>.</returns>
	bool Mandatory { get; }
	/// <summary>
	/// Sets the default value for this option function based on the provided value order.
	/// </summary>
	/// <param name="valueOrder">The value order containing default values. Can be null.</param>
	/// <param name="message">The error message container to populate in case of errors. Can be null.</param>
	void DefaultValue(CLIValueOrder? valueOrder, ErrorMessage? message);
	/// <summary>
	/// Processes and treats the value from the token list for this option function.
	/// </summary>
	/// <param name="valueOrder">The value order that defines how values should be treated. Can be null.</param>
	/// <param name="list">The token list containing the raw values. Can be null.</param>
	/// <param name="message">The error message container to populate in case of errors. Can be null.</param>
	void TreatedValue(CLIValueOrder? valueOrder, TokenList? list, ErrorMessage? message);
	/// <summary>
	/// Generates an exception message for a specific key-value pair when an error occurs.
	/// </summary>
	/// <param name="list">The token list at the point of error. Can be null.</param>
	/// <param name="value">The key-value pair that caused the exception.</param>
	/// <param name="message">The error message container to populate. Can be null.</param>
	void ExceptionMessage(TokenList? list, KeyValuePair<string, long> value, ErrorMessage? message);
}
