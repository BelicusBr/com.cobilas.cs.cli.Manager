using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager;

public readonly struct DefaultArgument(bool mandatory, string alias, Action<CLIValueOrder> defaultValue) : IArgument {
	private readonly CLIKey alias = alias;
	private readonly bool mandatory = mandatory;

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => CLIParse.ArgumentCode;

	public DefaultArgument(string alias, Action<CLIValueOrder> defaultValue) : this(false, alias, defaultValue) { }

	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage message) {
		if (value.Value != TypeCode) {
			message.ErroCode = 1;
			message.Message = $"A opção '{value.Key}' foi definida antes do argumento!!!";
		} else {
			message.ErroCode = 0;
			message.Message = $"({value.Key})Arg invalido!!!";
		}
	}

	public bool IsAlias(string alias) => alias == this.alias;

	public bool Analyzer(TokenList list, ErrorMessage message) {
		if (TypeCode != list.CurrentValue) {
			ExceptionMessage(list.Current, message);
			return true;
		}
		list.Move();
		return false;
	}

	public void TreatedValue(CLIValueOrder valueOrder, TokenList list)
		=> valueOrder.Add(Alias, list.CurrentKey);

	public void DefaultValue(CLIValueOrder valueOrder) => defaultValue(valueOrder);
}