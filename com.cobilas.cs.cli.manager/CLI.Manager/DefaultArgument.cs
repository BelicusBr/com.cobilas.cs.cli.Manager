using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Represents a default implementation of an argument in the CLI system.
/// This structure is immutable and provides basic argument handling, including alias matching,
/// mandatory status, and default value injection via a registered function.
/// </summary>
/// <param name="mandatory">Indicates whether the argument is mandatory.</param>
/// <param name="alias">The alias string for the argument. Cannot be null.</param>
/// <param name="idDefaultValue">The identifier of a function registered in <see cref="CLIParse"/> that accepts a <see cref="CLIValueOrder"/> parameter.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="alias"/> is null.</exception>
public readonly struct DefaultArgument(bool mandatory, string? alias, uint idDefaultValue) : IArgument {
	private readonly bool mandatory = mandatory;
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly Action<CLIValueOrder?> defaultValue = CLIParse.GetFunction<Action<CLIValueOrder?>>(idDefaultValue);
	/// <summary>
	/// Initializes a new instance of the <see cref="DefaultArgument"/> structure with a non‑mandatory argument.
	/// </summary>
	/// <param name="alias">The alias string for the argument. Cannot be null.</param>
	/// <param name="idDefaultValue">The identifier of a function registered in <see cref="CLIParse"/> that accepts a <see cref="CLIValueOrder"/> parameter.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="alias"/> is null.</exception>
	public DefaultArgument(string? alias, uint idDefaultValue) : this(false, alias, idDefaultValue) { }
	/// <summary>
	/// Gets the string representation of the alias.
	/// </summary>
	/// <returns>The alias string.</returns>
	public string Alias => alias;
	/// <summary>
	/// Gets a value indicating whether this argument is mandatory.
	/// </summary>
	/// <returns><see langword="true"/> if mandatory; otherwise, <see langword="false"/>.</returns>
	public bool Mandatory => mandatory;
	/// <summary>
	/// Gets the type code for this argument, which is <see cref="CLIParse.ArgumentCode"/>.
	/// </summary>
	/// <returns>The numeric type code.</returns>
	public long TypeCode => CLIParse.ArgumentCode;
	/// <inheritdoc/>
	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));

		if (value.Value == (long)CLIDefaultToken.Function) {
			message.ErroCode = 29;
			message.Message = $"The element '({(CLIDefaultToken)value.Value})[{value.Key}]' is not an argument!!!";
		} else if (value.Value != TypeCode) {
			message.ErroCode = 35;
			message.Message = $"The option '{value.Key}' was defined before the argument!!!";
		} else {
			message.ErroCode = 17;
			message.Message = $"({value.Key})Invalid argument!!!";
		}
	}
	/// <inheritdoc/>
	public bool IsAlias(string? alias) {
		if (alias is null) return false;
		return alias == this.alias;
	}
	/// <inheritdoc/>
	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));

		if (TypeCode != list.CurrentValue) {
			ExceptionMessage(list.Current, message);
			return true;
		}
		list.Move();
		return false;
	}
	/// <inheritdoc/>
	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(valueOrder, nameof(valueOrder));

		valueOrder.Add(Alias, list.CurrentKey);
	}
	/// <inheritdoc/>
	public void DefaultValue(CLIValueOrder? valueOrder) => defaultValue(valueOrder);
}