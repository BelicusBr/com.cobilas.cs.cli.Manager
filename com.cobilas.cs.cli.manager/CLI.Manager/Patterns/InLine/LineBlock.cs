using System;
using System.Linq;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;
/// <summary>
/// Represents a block that can act as a function, an option function, and a CLI analyzer,
/// combining multiple options into a single processing unit.
/// </summary>
/// <seealso cref="IFunction"/>
/// <seealso cref="IOptionFunc"/>
/// <seealso cref="ICLIAnalyzer"/>
/// <remarks>
/// Initializes a new instance of the <see cref="LineBlock"/> struct.
/// </remarks>
/// <param name="alias">The alias string for the block. Cannot be null.</param>
/// <param name="mandatory">Indicates whether the block is mandatory.</param>
/// <param name="options">An array of option functions belonging to this block.</param>
public readonly struct LineBlock(string? alias, bool mandatory, params IOptionFunc[] options) : IFunction, IOptionFunc, ICLIAnalyzer {
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly bool mandatory = mandatory;
	private readonly CLIValueOrder valueOrder = [];
	private readonly List<IOptionFunc> options = [.. options];
	/// <inheritdoc/>
	public string Alias => alias;
	/// <inheritdoc/>
	public bool Mandatory => mandatory;
	/// <inheritdoc/>
	public List<IOptionFunc> Options => options;
	/// <inheritdoc/>
	public CLIValueOrder ValueOrder => valueOrder;
	/// <inheritdoc/>
	public long TypeCode => (long)(CLIDefaultToken.Function | CLIDefaultToken.Option);
	/// <inheritdoc/>
	public bool IsAlias(string? alias)
		=> LineFunction.IsAlias(this, alias);
	/// <summary>
	/// Determines whether this block's type code matches the specified type code.
	/// </summary>
	/// <param name="typeCode">The type code to compare.</param>
	/// <returns><see langword="true"/> if the type codes match; otherwise, <see langword="false"/>.</returns>
	public bool HasTypeCode(long typeCode)
		=> LineFunction.HasTypeCode(TypeCode, typeCode);
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
	/// <inheritdoc/>
	void IOptionFunc.DefaultValue(CLIValueOrder? valueOrder, ErrorMessage? message)
		=> CLIParse.GetFunction<DefaultValueFunc>(0)?
			.Invoke(alias, valueOrder, message);
	/// <inheritdoc/>
	void IOptionFunc.ExceptionMessage(TokenList? list, KeyValuePair<string, long> value, ErrorMessage? message)
		=> CLIParse.GetFunction<ExceptionMessageFunc>(1)?
			.Invoke(alias, list, value, message);
	/// <inheritdoc/>
	void IOptionFunc.TreatedValue(CLIValueOrder? valueOrder, TokenList? list, ErrorMessage? message)
		=> CLIParse.GetFunction<TreatedValueFunc>(2)?
			.Invoke(alias, valueOrder, list, message);
	/// <inheritdoc/>
	void IFunction.Run(ErrorMessage? message)
		=> ((IFunction)this).Run(null, message);
	/// <inheritdoc/>
	void IFunction.Run(Action<CLIKey, CLIValueOrder?, ErrorMessage?>? action, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		message.ErroCode = 1256;
		message.Message = $"The function '{alias}' does not have an implementation of '{nameof(IFunction)}.{nameof(IFunction.Run)}'!";
	}
}