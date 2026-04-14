using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine;
/// <summary>
/// Represents a command-line option with jump behavior in processing sequences.
/// </summary>
/// <seealso cref="IOptionFunc"/>
/// <seealso cref="ILineJumpOption"/>
public readonly struct LineOption : IOptionFunc, ILineJumpOption {
	private readonly int jumpUp;
	private readonly CLIKey alias;
	private readonly bool mandatory;
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

	public LineOption(string? alias, bool mandatory, int jumpUp) {
		this.jumpUp = jumpUp;
		this.mandatory = mandatory;
		this.alias = alias ?? throw new ArgumentNullException(nameof(alias));
	}
	/// <summary>
	/// Determines if the provided alias matches this option's alias.
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
}
