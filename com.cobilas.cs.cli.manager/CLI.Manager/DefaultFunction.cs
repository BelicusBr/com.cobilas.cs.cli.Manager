using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Represents a default implementation of a CLI function.
/// This structure aggregates a collection of options, manages a value order,
/// and executes a registered delegate when invoked.
/// </summary>
/// <param name="alias">The primary alias for the function. Cannot be null or empty.</param>
/// <param name="idRunFunction">The identifier of a function registered in <see cref="CLIParse"/> that accepts a <see cref="CLIKey"/> and an optional <see cref="CLIValueOrder"/>.</param>
/// <param name="options">An array of option functions that belong to this function.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="alias"/> is null.</exception>
public readonly struct DefaultFunction(string alias, uint idRunFunction, params IOptionFunc[] options) : IFunction {
	private readonly CLIKey alias = alias;
	private readonly List<IOptionFunc> options = [.. options];
	private readonly CLIValueOrder valueOrder = [];
	private readonly Action<CLIKey, CLIValueOrder?> runFunction = CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?>>(idRunFunction);
	/// <summary>
	/// Gets the string representation of the alias.
	/// </summary>
	/// <returns>The alias string.</returns>
	public string Alias => alias;
	/// <summary>
	/// Gets the list of option functions associated with this function.
	/// </summary>
	/// <returns>A list of <see cref="IOptionFunc"/> objects.</returns>
	public List<IOptionFunc> Options => options;
	/// <summary>
	/// Gets the type code for this function, which is <see cref="CLIDefaultToken.Function"/>.
	/// </summary>
	/// <returns>The numeric type code.</returns>
	public long TypeCode => (long)CLIDefaultToken.Function;
	/// <summary>
	/// Gets the value order that collects the processed values from options and arguments.
	/// </summary>
	/// <returns>The <see cref="CLIValueOrder"/> instance.</returns>
	public CLIValueOrder ValueOrder => valueOrder;
	/// <inheritdoc/>
	public bool IsAlias(string? alias) {
		if (alias is not null)
			foreach (string item in alias.Split(CLIKey.separator, StringSplitOptions.RemoveEmptyEntries))
				if (this.alias == item)
					return true;
		return false;
	}
	/// <inheritdoc/>
	public bool GetValues(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));

		for (int I = 0; I < options.Count; I++) {
			IOptionFunc of = options[I];
			if (of.TypeCode == list.CurrentValue) {
				if (of.IsAlias(list.CurrentKey) || of.IsAlias("{ARG}")) {
					of.TreatedValue(valueOrder, list);
					list.Move();
				}
			} else {
				if (of.Mandatory) {
					of.ExceptionMessage(list.Current, message);
					return true;
				} else of.DefaultValue(valueOrder);
			}
		}
		return false;
	}
	/// <inheritdoc/>
	public void Run() => runFunction(alias, valueOrder);
	/// <inheritdoc/>
	public void Run(Action<CLIKey, CLIValueOrder?>? action) {
		ExceptionMessages.ThrowIfNull(action, nameof(action));
		action(alias, valueOrder);
	}
	/// <inheritdoc/>
	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));

		list.Move();
		foreach (IOptionFunc item in options)
			if (item.Analyzer(list, message))
				return true;
		if (list.CurrentValue != CLIParse.EndCode) {
			if (list.CurrentValue == CLIParse.ArgumentCode) {
				message.ErroCode = 74;
				message.Message = $"The argument is not defined for the function ({alias})!";
			} else if (list.CurrentValue == (long)CLIDefaultToken.Option) {
				message.ErroCode = 75;
				message.Message = $"The option ({list.CurrentKey}) is not defined for the function ({alias})!";
			}
			return true;
		}
		return false;
	}
}