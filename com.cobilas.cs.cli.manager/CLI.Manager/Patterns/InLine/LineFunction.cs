using System;
using System.Collections.Generic;
using System.Linq;
using Cobilas.CLI.Manager.Interfaces;

using AnalyzerFunc = System.Func<
	Cobilas.CLI.Manager.CLIKey, 
	Cobilas.CLI.Manager.TokenList?, 
	System.Collections.Generic.List<Cobilas.CLI.Manager.Interfaces.IOptionFunc>?, 
	Cobilas.CLI.Manager.ErrorMessage?, bool>;
using GetValuesFunc = System.Func<
	Cobilas.CLI.Manager.CLIKey, 
	Cobilas.CLI.Manager.TokenList?, 
	Cobilas.CLI.Manager.CLIValueOrder, 
	System.Collections.Generic.List<Cobilas.CLI.Manager.Interfaces.IOptionFunc>?, 
	Cobilas.CLI.Manager.ErrorMessage?, bool>;

namespace Cobilas.CLI.Manager.Patterns.InLine;
/// <summary>
/// Represents a command-line function with alias, value ordering, and option processing capabilities.
/// </summary>
/// <seealso cref="IFunction"/>
/// <seealso cref="ICLIAnalyzer"/>
public readonly struct LineFunction : IFunction , ICLIAnalyzer {
	private readonly CLIKey alias;
	private readonly CLIValueOrder valueOrder;
	private readonly List<IOptionFunc> options;
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

	public static Func<CLIKey, TokenList?, List<IOptionFunc>?, ErrorMessage?, bool> FunctionAnalyzer => LineFunctionUtility.Analyzer;
	public static Func<CLIKey, TokenList?, CLIValueOrder, List<IOptionFunc>?, ErrorMessage?, bool> FunctionGetValues => LineFunctionUtility.GetValues;

	/// <summary>
	/// Initializes a new instance of the <see cref="LineFunction"/> struct.
	/// </summary>
	/// <param name="alias">The alias name for the function.</param>
	/// <param name="options">The array of option functions associated with this function.</param>
	public LineFunction(string alias, params IOptionFunc[] options) {
		valueOrder = [];
		this.alias = alias;
		this.options = [.. options];
	}
	/// <summary>
	/// Determines if the provided alias matches this function's alias.
	/// </summary>
	/// <param name="alias">The alias to compare.</param>
	/// <returns><see langword="true"/> if the aliases match; otherwise, <see langword="false"/>.</returns>
	public bool IsAlias(string? alias) => IsAlias(this, alias);
	/// <inheritdoc/>
	public void Run(ErrorMessage? message)
		=> Run(CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?, ErrorMessage?>>(4), message);
	/// <inheritdoc/>
	public void Run(Action<CLIKey, CLIValueOrder?, ErrorMessage?>? action, ErrorMessage? message)
		=> action?.Invoke(alias, valueOrder, message);
	/// <inheritdoc/>
	bool ICLIAnalyzer.Analyzer(TokenList? list, ErrorMessage? message) {
		Delegate? func = CLIParse.GetFunction(5);
		if (func is null) return false;
		foreach (AnalyzerFunc? item in func.GetInvocationList().Cast<AnalyzerFunc?>())
			if (item is not null)
			{
				bool? numB = (bool?)item?.Invoke(alias, list, options, message);
				if (numB.HasValue)
					return numB.Value;
			}
		return false;
	}
	/// <inheritdoc/>
	bool IFunction.GetValues(TokenList? list, ErrorMessage? message) {
		Delegate? func = CLIParse.GetFunction(3);
		if (func is null) return false;
		foreach (GetValuesFunc? item in func.GetInvocationList().Cast<GetValuesFunc?>())
			if (item is not null) {
				bool? numB = (bool?)item?.Invoke(alias, list, valueOrder, options, message);
				if (numB.HasValue)
					return numB.Value;
			}
		return false;
	}
	/// <summary>
	/// Determines if a type code matches the comparison code using flag checking.
	/// </summary>
	/// <param name="typeCode">The type code to check.</param>
	/// <param name="compare">The comparison code.</param>
	/// <returns><see langword="true"/> if the type code matches; otherwise, <see langword="false"/>.</returns>
	public static bool IsTypeCode(long typeCode, long compare)
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
}
