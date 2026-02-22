using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

readonly struct DefaultOption(
	string alias, 
	bool mandatory, 
	Func<int, string> argName,
	Action<Dictionary<CLIKey, string>> defaultValue,
	params IArgument[] arguments) : IOption {
	private readonly CLIKey alias = alias;
	private readonly bool mandatory = mandatory;
	private readonly Func<int, string> argName = argName;
	private readonly List<IArgument> options = [.. arguments];

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public List<IArgument> Options => options;
	public long TypeCode => (long)CLIToken.Option;

	public bool Analyzer(TokenList list, ErrorMessage message) {
		if (list.CurrentValue != TypeCode) {
			if (Mandatory) {
				ExceptionMessage(list.Current, message);
				return true;
			}
		} else if (!IsAlias(list.CurrentKey))
			if (Mandatory) {
				ExceptionMessage(list.Current, message);
				return true;
			}
		list.Move();
		foreach (IArgument item in options)
			if (item.Analyzer(list, message))
				return true;
		return false;
	}

	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage message) {
		message.ErroCode = 3;
		message.Message = $"O elemento '({(CLIToken)value.Value}){value.Key}' é chamado antes de '({(CLIToken)TypeCode}){alias}'!!!";
	}

	public bool IsAlias(string alias) {
		foreach (string item in alias.Split('/', StringSplitOptions.RemoveEmptyEntries))
			if (this.alias == item)
				return true;
		return false;
	}

	public void TreatedValue(Dictionary<CLIKey, string> valueOrder, TokenList list) {
		for (int I = 0; I < options.Count; I++) {
			KeyValuePair<string, long> temp = list.GetValueAndMove;
			string name = argName(I);
			valueOrder.Add(name, temp.Key);
		}
	}

	public void DefaultValue(Dictionary<CLIKey, string> valueOrder) => defaultValue(valueOrder);
}