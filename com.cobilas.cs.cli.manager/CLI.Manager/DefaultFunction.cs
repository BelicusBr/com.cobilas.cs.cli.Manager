using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager;

public readonly struct DefaultFunction(
	string alias,
	Action<CLIKey, CLIValueOrder> runFunction,
	params IOptionFunc[] options) : IFunction {
	private readonly CLIKey alias = alias;
	private readonly List<IOptionFunc> options = [.. options];
	private readonly CLIValueOrder valueOrder = [];
	private readonly Action<CLIKey, CLIValueOrder> runFunction = runFunction;

	public string Alias => alias;
	public List<IOptionFunc> Options => options;
	public long TypeCode => (long)CLIToken.Function;
	public CLIValueOrder ValueOrder => valueOrder;

	public DefaultFunction(string alias, params IOptionFunc[] options) :
		this(alias, null, options)
	{ }

	public bool IsAlias(string alias) {
		foreach (string item in alias.Split(CLIKey.separator, StringSplitOptions.RemoveEmptyEntries))
			if (this.alias == item)
				return true;
		return false;
	}

	public bool GetValues(TokenList list, ErrorMessage message) {
		for (int I = 0; I < options.Count; I++) {
			IOptionFunc of = options[I];
			if (of.TypeCode == list.CurrentValue) {
				if (of.IsAlias(list.CurrentKey) || of.IsAlias("{ARG}")) {
					of.TreatedValue(valueOrder, list);
					list.Move();
				}
			} else {
				if (of.Mandatory) {
					of.ExceptionMessage(list.Current, message);
					return true;
				} else {
					of.DefaultValue(valueOrder);
				}
			}
		}
		return false;
	}

	public void Run() => runFunction(alias, valueOrder);

	public void Run(Action<CLIKey, CLIValueOrder> action) => action(alias, valueOrder);

	public bool Analyzer(TokenList list, ErrorMessage message) {
		list.Move();
		foreach (IOptionFunc item in options)
			if (item.Analyzer(list, message))
				return true;
		return false;
	}
}