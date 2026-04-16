using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;
/// <summary>
/// Represents a command-line option with jump behavior in processing sequences.
/// </summary>
/// <seealso cref="IOptionFunc"/>
/// <seealso cref="ILineJumpOption"/>
/// <remarks>
/// Initializes a new instance of the <see cref="LineOption"/> struct.
/// </remarks>
/// <param name="alias">The alias string for the option. Cannot be null.</param>
/// <param name="mandatory">Indicates whether the option is mandatory.</param>
/// <param name="jumpUp">The number of positions to jump up in the processing sequence.</param>
public readonly struct LineOption(string? alias, bool mandatory, int jumpUp) : IOptionFunc, ILineJumpOption {
	private readonly int jumpUp = jumpUp;
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly bool mandatory = mandatory;
	/// <summary>
	/// Gets the number of positions to jump up in the processing sequence.
	/// </summary>
	/// <returns>The number of positions to move upwards.</returns>
	public int JumpUp => jumpUp;
	/// <summary>
	/// Gets the alias of the option.
	/// </summary>
	/// <returns>The option alias string.</returns>
	public string Alias => alias;
	/// <summary>
	/// Gets a value indicating whether this option is mandatory.
	/// </summary>
	/// <returns><see langword="true"/> if the option is mandatory; otherwise, <see langword="false"/>.</returns>
	public bool Mandatory => mandatory;
	/// <summary>
	/// Gets the type code identifier for this option.
	/// </summary>
	/// <returns>The type code as a long value.</returns>
	public long TypeCode => (long)CLIDefaultToken.Option;
	/// <inheritdoc/>
	bool ILineJumpOption.JumpToEnd => false;
	/// <summary>
	/// Determines if the provided alias matches this option's alias.
	/// </summary>
	/// <param name="alias">The alias to compare.</param>
	/// <returns><see langword="true"/> if the aliases match; otherwise, <see langword="false"/>.</returns>
	public bool IsAlias(string? alias)
		=> LineFunction.IsAlias(this, alias);
	/// <summary>
	/// Determines whether this option's type code matches the specified type code.
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