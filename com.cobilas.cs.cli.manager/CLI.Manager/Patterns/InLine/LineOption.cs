using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;

public readonly struct LineOption(string? alias, bool mandatory, int jumpUp) : IOptionFunc, ILineJumpOption {
	private readonly int jumpUp = jumpUp;
	private readonly CLIKey alias = alias;
	private readonly bool mandatory = mandatory;

	public int JumpUp => jumpUp;
	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => (long)CLIDefaultToken.Option;

	bool ILineJumpOption.JumpToEnd => false;

	public bool IsAlias(string? alias)
		=> LineFunction.IsAlias(this, alias);

	void IOptionFunc.DefaultValue(CLIValueOrder? valueOrder)
	{
		
	}

	void IOptionFunc.ExceptionMessage(object? onj, KeyValuePair<string, long> value, ErrorMessage? message)
	{
		message.Message = value.Key;
	}

	void IOptionFunc.TreatedValue(CLIValueOrder? valueOrder, TokenList? list)
	{
		
	}
}
