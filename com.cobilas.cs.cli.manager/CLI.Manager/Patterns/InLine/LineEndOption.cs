using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;

public readonly struct LineEndOption(string? alias, bool mandatory) : IOptionFunc, ILineJumpOption {
	private readonly CLIKey alias = alias;
	private readonly bool mandatory = mandatory;

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => (long)CLIDefaultToken.Option | CLIParse.EndCode;

	int ILineJumpOption.JumpUp => 0;
	bool ILineJumpOption.JumpToEnd => true;

	public bool IsAlias(string? alias)
		=> LineFunction.IsAlias(this, alias);

	void IOptionFunc.DefaultValue(CLIValueOrder? valueOrder)
	{
		
	}

	void IOptionFunc.ExceptionMessage(object? onj, KeyValuePair<string, long> value, ErrorMessage? message)
	{
		
	}

	void IOptionFunc.TreatedValue(CLIValueOrder? valueOrder, TokenList? list)
	{
		
	}
}
