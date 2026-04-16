using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;
/// <summary>
/// Represents a command-line argument with an alias and mandatory status.
/// </summary>
/// <seealso cref="IArgument"/>
/// <remarks>
/// Initializes a new instance of the <see cref="LineArgument"/> struct.
/// </remarks>
/// <param name="alias">The alias string for the argument. Cannot be null or empty.</param>
/// <param name="mandatory">Indicates whether the argument is mandatory.</param>
public readonly struct LineArgument(string? alias, bool mandatory) : IArgument {
	private readonly CLIKey alias = GetAlias(alias);
	private readonly bool mandatory = mandatory;
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
	/// <summary>
	/// Determines if the provided alias matches this argument's alias.
	/// </summary>
	/// <param name="alias">The alias to compare.</param>
	/// <returns><see langword="true"/> if the aliases match; otherwise, <see langword="false"/>.</returns>
	public bool IsAlias(string? alias)
		=> LineFunction.IsAlias(this, alias);
	/// <summary>
	/// Determines whether this argument's type code matches the specified type code.
	/// </summary>
	/// <param name="typeCode">The type code to compare.</param>
	/// <returns><see langword="true"/> if the type codes match; otherwise, <see langword="false"/>.</returns>
	public bool HasTypeCode(long typeCode)
		=> LineFunction.HasTypeCode(TypeCode, typeCode);
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

	private static string GetAlias(string? alias) {
		ExceptionMessages.ThrowIfNullOrEmpty(alias, nameof(alias));
		return $"{alias}/{{ARG}}";
	}
}