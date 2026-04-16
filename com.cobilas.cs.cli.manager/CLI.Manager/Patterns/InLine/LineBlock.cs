using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;

public readonly struct LineBlock : IFunction, IOptionFunc, ICLIAnalyzer {
	private readonly CLIKey alias;
	private readonly bool mandatory;
	private readonly CLIValueOrder valueOrder;
	private readonly List<IOptionFunc> options;
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

	public LineBlock(string? alias, bool mandatory, params IOptionFunc[] options) {
		valueOrder = [];
		this.mandatory = mandatory;
		this.options = [.. options];
		this.alias = alias ?? throw new ArgumentNullException(nameof(alias));
	}
	/// <inheritdoc/>
	public bool IsAlias(string? alias)
		=> LineFunction.IsAlias(this, alias);
	/// <inheritdoc/>
	bool IFunction.GetValues(TokenList? list, ErrorMessage? message) {
		Func<CLIKey, TokenList?, CLIValueOrder, List<IOptionFunc>?, ErrorMessage?, bool>? func =
			CLIParse.GetFunction<Func<CLIKey, TokenList?, CLIValueOrder, List<IOptionFunc>?, ErrorMessage?, bool>>(3);
		if (func is null) return false;
		foreach (Delegate? item in func.GetInvocationList())
			if (item is not null) {
				bool? numB = (bool?)item?.DynamicInvoke(alias, list, valueOrder, options, message);
				if (numB.HasValue)
					return numB.Value;
			}
		return false;
	}
	/// <inheritdoc/>
	bool ICLIAnalyzer.Analyzer(TokenList? list, ErrorMessage? message) {
		Func<CLIKey, TokenList?, List<IOptionFunc>?, ErrorMessage?, bool>? func =
			CLIParse.GetFunction<Func<CLIKey, TokenList?, List<IOptionFunc>?, ErrorMessage?, bool>>(5);
		if (func is null) return false;
		foreach (Delegate? item in func.GetInvocationList())
			if (item is not null) {
				bool? numB = (bool?)item?.DynamicInvoke(alias, list, options, message);
				if (numB.HasValue)
					return numB.Value;
			}
		return false;
	}
	/// <inheritdoc/>
	void IOptionFunc.DefaultValue(CLIValueOrder? valueOrder, ErrorMessage? message)
		=> CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?, ErrorMessage?>>(0)?
			.Invoke(alias, valueOrder, message);
	/// <inheritdoc/>
	void IOptionFunc.ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message)
		=> CLIParse.GetFunction<Action<CLIKey, KeyValuePair<string, long>, ErrorMessage?>>(1)?
			.Invoke(alias, value, message);
	/// <inheritdoc/>
	void IOptionFunc.TreatedValue(CLIValueOrder? valueOrder, TokenList? list, ErrorMessage? message)
		=> CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?, TokenList?, ErrorMessage?>>(2)?
			.Invoke(alias, valueOrder, list, message);

	void IFunction.Run(ErrorMessage? message)
		=> ((IFunction)this).Run(null, message);

	void IFunction.Run(Action<CLIKey, CLIValueOrder?, ErrorMessage?>? action, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));


	}
}
