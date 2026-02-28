using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Represents a default implementation of an option in the CLI system.
/// This structure holds an alias, mandatory flag, a collection of arguments,
/// and registered functions for argument naming and default value handling.
/// </summary>
/// <param name="alias">The alias string for the option. Cannot be null or empty.</param>
/// <param name="mandatory">Indicates whether the option is mandatory.</param>
/// <param name="idArgName">The identifier of a function registered in <see cref="CLIParse"/> that accepts an integer index and returns a string argument name.</param>
/// <param name="idDefaultValue">The identifier of a function registered in <see cref="CLIParse"/> that accepts a <see cref="CLIValueOrder"/> parameter.</param>
/// <param name="arguments">An array of arguments associated with this option. May be null.</param>
public readonly struct DefaultOption(
	string alias,
	bool mandatory,
	uint idArgName,
	uint idDefaultValue,
	params IArgument[]? arguments) : IOption {
	private readonly CLIKey alias = alias;
	private readonly bool mandatory = mandatory;
	private readonly List<IArgument>? options = GetArguments(arguments);
	private readonly Func<int, string> argName = CLIParse.GetFunction<Func<int, string>>(idArgName);
	private readonly Action<CLIValueOrder?> defaultValue = CLIParse.GetFunction<Action<CLIValueOrder?>>(idDefaultValue);
	/// <summary>
	/// Initializes a new instance of the <see cref="DefaultOption"/> structure with a non‑mandatory option
	/// and a collection of arguments.
	/// </summary>
	/// <param name="alias">The alias string for the option.</param>
	/// <param name="idArgName">The identifier of the argument‑naming function.</param>
	/// <param name="idDefaultValue">The identifier of the default‑value function.</param>
	/// <param name="arguments">An array of arguments associated with this option.</param>
	public DefaultOption(
		string alias,
		uint idArgName,
		uint idDefaultValue,
		params IArgument[] arguments)
		: this(alias, false, idArgName, idDefaultValue, arguments)
	{
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="DefaultOption"/> structure with a mandatory flag
	/// but no arguments.
	/// </summary>
	/// <param name="alias">The alias string for the option.</param>
	/// <param name="mandatory">Indicates whether the option is mandatory.</param>
	/// <param name="idArgName">The identifier of the argument‑naming function.</param>
	/// <param name="idDefaultValue">The identifier of the default‑value function.</param>
	public DefaultOption(
		string alias,
		bool mandatory,
		uint idArgName,
		uint idDefaultValue)
		: this(alias, mandatory, idArgName, idDefaultValue, null)
	{
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="DefaultOption"/> structure with a non‑mandatory option
	/// and no arguments.
	/// </summary>
	/// <param name="alias">The alias string for the option.</param>
	/// <param name="idArgName">The identifier of the argument‑naming function.</param>
	/// <param name="idDefaultValue">The identifier of the default‑value function.</param>
	public DefaultOption(
		string alias,
		uint idArgName,
		uint idDefaultValue)
		: this(alias, false, idArgName, idDefaultValue, null)
	{
	}
	/// <summary>
	/// Gets the string representation of the alias.
	/// </summary>
	/// <returns>The alias string.</returns>
	public string Alias => alias;
	/// <summary>
	/// Gets a value indicating whether this option is mandatory.
	/// </summary>
	/// <returns><see langword="true"/> if mandatory; otherwise, <see langword="false"/>.</returns>
	public bool Mandatory => mandatory;
	/// <summary>
	/// Gets the list of arguments associated with this option.
	/// </summary>
	/// <returns>A list of <see cref="IArgument"/> objects, or <see langword="null"/> if no arguments are defined.</returns>
	public List<IArgument>? Options => options;
	/// <summary>
	/// Gets the type code for this option, which is <see cref="CLIDefaultToken.Option"/>.
	/// </summary>
	/// <returns>The numeric type code.</returns>
	public long TypeCode => (long)CLIDefaultToken.Option;
	/// <inheritdoc/>
	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		if (options is not null) {
			if (list.CurrentValue != TypeCode) {
				if (Mandatory) {
					ExceptionMessage(list.Current, message);
					return true;
				}
			} else if (!IsAlias(list.CurrentKey))
				if (Mandatory) {
					ExceptionMessage(list.Current, message);
					return true;
				}
			list.Move();
			foreach (IArgument item in options)
				if (item.Analyzer(list, message))
					return true;
		}
		return false;
	}
	/// <inheritdoc/>
	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		if (value.Value == (long)CLIDefaultToken.Function) {
			message.ErroCode = 27;
			message.Message = $"The element '({(CLIDefaultToken)value.Value})[{value.Key}]' is not an option!!!";
		} else {
			message.ErroCode = 22;
			message.Message = $"The element '({(CLIDefaultToken)value.Value}){value.Key}' is called before '({(CLIDefaultToken)TypeCode}){alias}'!!!";
		}
	}
	/// <inheritdoc/>
	public bool IsAlias(string? alias) {
		ExceptionMessages.ThrowIfNull(alias, nameof(alias));
		foreach (string item in alias.Split(CLIKey.separator, StringSplitOptions.RemoveEmptyEntries))
			if (this.alias == item)
				return true;
		return false;
	}
	/// <inheritdoc/>
	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(valueOrder, nameof(valueOrder));
		if (options is not null)
			for (int I = 0; I < options.Count; I++) {
				KeyValuePair<string, long> temp = list.GetValueAndMove;
				string name = argName(I);
				valueOrder.Add(name, temp.Key);
			}
	}
	/// <inheritdoc/>
	public void DefaultValue(CLIValueOrder? valueOrder) => defaultValue(valueOrder);

	private static List<IArgument>? GetArguments(IArgument[]? args)
		=> args is null ? (List<IArgument>?)null : [.. args];
}