using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;
/// <summary>
/// Represents a command-line argument with an alias and mandatory status.
/// </summary>
/// <seealso cref="IArgument"/>
public readonly struct LineArgument : IArgument {
	private readonly CLIKey alias;
	private readonly bool mandatory;
	/// <summary>
	/// Gets the alias of the argument.
	/// </summary>
	/// <returns>The argument alias string.</returns>
	public string Alias => alias;
	/// <summary>
	/// Gets a value indicating whether this argument is mandatory.
	/// </summary>
	/// <returns><see langword="true"/> if the argument is mandatory; otherwise, <see langword="false"/>.</returns>
	public bool Mandatory => mandatory;
	/// <summary>
	/// Gets the type code identifier for this argument.
	/// </summary>
	/// <returns>The type code as a long value.</returns>
	public long TypeCode => CLIParse.ArgumentCode;

	public LineArgument(string? alias, bool mandatory) {
		this.alias = GetAlias(alias);
		this.mandatory = mandatory;
	}
	/// <summary>
	/// Determines if the provided alias matches this argument's alias.
	/// </summary>
	/// <param name="alias">The alias to compare.</param>
	/// <returns><see langword="true"/> if the aliases match; otherwise, <see langword="false"/>.</returns>
	public bool IsAlias(string? alias)
		=> LineFunction.IsAlias(this, alias);
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
		=> CLIParse.GetFunction<Action<CLIKey, TokenList?, ErrorMessage?>>(2)?
			.Invoke(alias, list, message);

	private static string GetAlias(string? alias) {
		ExceptionMessages.ThrowIfNullOrEmpty(alias, nameof(alias));
		return $"{alias}/{{ARG}}";
	}
}
