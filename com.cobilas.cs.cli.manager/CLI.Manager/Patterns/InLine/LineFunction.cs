using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;

public readonly struct LineFunction : IFunction , ICLIAnalyzer {
	private readonly CLIKey alias;
	private readonly CLIValueOrder valueOrder;
	private readonly List<IOptionFunc> options;

	public string Alias => alias;
	public List<IOptionFunc> Options => options;
	public CLIValueOrder ValueOrder => valueOrder;
	public long TypeCode => (long)CLIDefaultToken.Function;

	public LineFunction(string alias, params IOptionFunc[] options) {
		valueOrder = [];
		this.alias = alias;
		this.options = [.. options];
	}

	public bool IsAlias(string? alias) => IsAlias(this, alias);

	public void Run(ErrorMessage error)
	{
		throw new NotImplementedException();
	}

	public void Run(Action<CLIKey, CLIValueOrder?>? action, ErrorMessage error)
	{
		throw new NotImplementedException();
	}

	bool ICLIAnalyzer.Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));

		for (int I = 0; I < options.Count; I++) {
			IOptionFunc item = options[I];
			if (item.TypeCode != list.CurrentValue) {
				if (item.Mandatory) {
					item.ExceptionMessage(null, list.Current, message);
					return true;
				} else if (item is ILineJumpOption ljo) {
					if (!ljo.JumpToEnd) {
						I += ljo.JumpUp;
						continue;
					}
				}
			} else {
				if (item is ILineJumpOption ljo2)
					if (ljo2.JumpToEnd) {
						I = options.Count;
						continue;
					}
				list.Move();
			}
		}
		return false;
	}

	bool IFunction.GetValues(TokenList? list, ErrorMessage? message) {
		return false;
	}

	public static bool IsTypeCode(long typeCode, long compare)
		=> ((CLIDefaultToken)typeCode).HasFlag((CLIDefaultToken)compare);

	public static bool IsAlias(IAlias alias, string? aliasName) {
		if (aliasName is null)
			return false;
		else if (aliasName == string.Empty)
			return false;
		return (CLIKey)alias.Alias == (CLIKey)aliasName;
	}
}
