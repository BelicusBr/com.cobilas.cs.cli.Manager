using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;
/// <summary>
/// Represents an end option that marks the termination of command-line processing.
/// </summary>
/// <seealso cref="IOptionFunc"/>
/// <seealso cref="ILineJumpOption"/>
/// <remarks>
/// Initializes a new instance of the <see cref="LineEndOption"/> struct.
/// </remarks>
/// <param name="alias">The alias string for the end option. Cannot be null.</param>
/// <param name="mandatory">Indicates whether the end option is mandatory.</param>
public readonly struct LineEndOption(string? alias, bool mandatory) : IOptionFunc, ILineJumpOption {
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly bool mandatory = mandatory;
	/// <summary>
	/// Gets the alias of the end option.
	/// </summary>
	/// <returns>The option alias string.</returns>
	public string Alias => alias;
	/// <summary>
	/// Gets a value indicating whether this end option is mandatory.
	/// </summary>
	/// <returns><see langword="true"/> if the option is mandatory; otherwise, <see langword="false"/>.</returns>
	public bool Mandatory => mandatory;
	/// <summary>
	/// Gets the type code identifier for this end option.
	/// </summary>
	/// <returns>The type code as a long value.</returns>
	public long TypeCode => (long)CLIDefaultToken.Option | CLIParse.EndCode;
	/// <inheritdoc/>
	int ILineJumpOption.JumpUp => 0;
	/// <inheritdoc/>
	bool ILineJumpOption.JumpToEnd => true;
	/// <summary>
	/// Determines if the provided alias matches this option's alias.
	/// </summary>
	/// <param name="alias">The alias to compare.</param>
	/// <returns><see langword="true"/> if the aliases match; otherwise, <see langword="false"/>.</returns>
	public bool IsAlias(string? alias)
		=> LineFunction.IsAlias(this, alias);
	/// <summary>
	/// Determines whether this end option's type code matches the specified type code.
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
}