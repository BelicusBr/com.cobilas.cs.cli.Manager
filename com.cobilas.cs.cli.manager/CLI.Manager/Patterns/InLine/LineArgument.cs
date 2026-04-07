using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;

public readonly struct LineArgument(string? alias, bool mandatory) : IArgument {
	private readonly CLIKey alias = $"{alias}/{{ARG}}";
	private readonly bool mandatory = mandatory;
	
	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => CLIParse.ArgumentCode;

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
