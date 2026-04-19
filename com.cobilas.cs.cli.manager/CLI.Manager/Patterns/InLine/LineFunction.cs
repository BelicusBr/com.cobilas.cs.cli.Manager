using System;
using System.Linq;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;
/// <summary>
/// Represents a command-line function with alias, value ordering, and option processing capabilities.
/// </summary>
/// <seealso cref="IFunction"/>
/// <seealso cref="ICLIAnalyzer"/>
/// <remarks>
/// Initializes a new instance of the <see cref="LineFunction"/> struct.
/// </remarks>
/// <param name="alias">The alias name for the function.</param>
/// <param name="options">The array of option functions associated with this function.</param>
public readonly struct LineFunction(string alias, params IOptionFunc[] options) : IFunction, ICLIAnalyzer {
	private readonly CLIKey alias = alias;
	private readonly CLIValueOrder valueOrder = [];
	private readonly List<IOptionFunc> options = [.. options];
	/// <summary>
	/// Gets the alias of the function.
	/// </summary>
	/// <returns>The function alias string.</returns>
	public string Alias => alias;
	/// <summary>
	/// Gets the list of option functions associated with this function.
	/// </summary>
	/// <returns>A list of <see cref="IOptionFunc"/> objects.</returns>
	public List<IOptionFunc> Options => options;
	/// <summary>
	/// Gets the value order configuration for this function.
	/// </summary>
	/// <returns>The <see cref="CLIValueOrder"/> instance.</returns>
	public CLIValueOrder ValueOrder => valueOrder;
	/// <summary>
	/// Gets the type code identifier for this function.
	/// </summary>
	/// <returns>The type code as a long value.</returns>
	public long TypeCode => (long)CLIDefaultToken.Function;
	/// <summary>
	/// Gets a function delegate used to analyze the token list for this function.
	/// </summary>
	/// <returns>A delegate that performs analysis and returns a boolean indicating success or failure.</returns>
	public static AnalyzerFunc FunctionAnalyzer => LineFunctionUtility.Analyzer;
	/// <summary>
	/// Gets a function delegate used to retrieve values from the token list for this function.
	/// </summary>
	/// <returns>A delegate that retrieves values and returns a boolean indicating success or failure.</returns>
	public static GetValuesFunc FunctionGetValues => LineFunctionUtility.GetValues;
	/// <summary>
	/// Determines if the provided alias matches this function's alias.
	/// </summary>
	/// <param name="alias">The alias to compare.</param>
	/// <returns><see langword="true"/> if the aliases match; otherwise, <see langword="false"/>.</returns>
	public bool IsAlias(string? alias) => IsAlias(this, alias);
	/// <summary>
	/// Determines whether this function's type code matches the specified type code.
	/// </summary>
	/// <param name="typeCode">The type code to compare.</param>
	/// <returns><see langword="true"/> if the type codes match; otherwise, <see langword="false"/>.</returns>
	public bool HasTypeCode(long typeCode)
		=> HasTypeCode(TypeCode, typeCode);
	/// <inheritdoc/>
	public bool Run(ErrorMessage? message)
		=> Run(CLIParse.GetFunction<RunFunc>(4), message);
	/// <inheritdoc/>
	public bool Run(RunFunc? action, ErrorMessage? message) {
		if (action is null) return false;
		foreach (RunFunc? item in action.GetInvocationList().Cast<RunFunc?>())
			if (item is not null) {
				bool? numB = item?.Invoke(alias, valueOrder, message);
				if (numB.HasValue)
					if (numB.Value)
						return true;
			}
		return false;
	}
	/// <inheritdoc/>
	bool ICLIAnalyzer.Analyzer(TokenList? list, ErrorMessage? message) {
		Delegate? func = CLIParse.GetFunction(5);
		if (func is null) return false;
		foreach (AnalyzerFunc? item in func.GetInvocationList().Cast<AnalyzerFunc?>())
			if (item is not null) {
				bool? numB = item?.Invoke(alias, list, options, message);
				if (numB.HasValue)
					if (numB.Value)
						return true;
			}
		return false;
	}
	/// <inheritdoc/>
	bool IFunction.GetValues(TokenList? list, ErrorMessage? message) {
		Delegate? func = CLIParse.GetFunction(3);
		if (func is null) return false;
		foreach (GetValuesFunc? item in func.GetInvocationList().Cast<GetValuesFunc?>())
			if (item is not null) {
				bool? numB = item?.Invoke(alias, list, valueOrder, options, message);
				if (numB.HasValue)
					if (numB.Value)
						return true;
			}
		return false;
	}
	/// <summary>
	/// Determines if a type code matches the comparison code using flag checking.
	/// </summary>
	/// <param name="typeCode">The type code to check.</param>
	/// <param name="compare">The comparison code.</param>
	/// <returns><see langword="true"/> if the type code matches; otherwise, <see langword="false"/>.</returns>
	public static bool HasTypeCode(long typeCode, long compare)
		=> ((CLIDefaultToken)typeCode).HasFlag((CLIDefaultToken)compare);
	/// <summary>
	/// Determines if the provided alias matches the given alias object.
	/// </summary>
	/// <param name="alias">The alias object to compare.</param>
	/// <param name="aliasName">The alias name to compare against.</param>
	/// <returns><see langword="true"/> if the aliases match; otherwise, <see langword="false"/>.</returns>
	public static bool IsAlias(IAlias alias, string? aliasName) {
		if (aliasName is null)
			return false;
		else if (aliasName == string.Empty)
			return false;
		return (CLIKey)alias.Alias == (CLIKey)aliasName;
	}
	/// <summary>
	/// Determines whether the specified alias element matches the given key-value pair.
	/// </summary>
	/// <param name="alias">The alias object to check.</param>
	/// <param name="value">The key-value pair containing the token and its type code.</param>
	/// <returns><see langword="true"/> if the alias has the matching type code and the alias string equals either "{ARG}" or the key; otherwise, <see langword="false"/>.</returns>
	public static bool HasElement(IAlias alias, KeyValuePair<string, long> value)
		=> alias.HasTypeCode(value.Value) && ((CLIKey)alias.Alias == (CLIKey)"{ARG}" || (CLIKey)alias.Alias == (CLIKey)value.Key);
}