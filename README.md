# Cobilas.CLI.Manager
A powerful and flexible command-line interface (CLI) parsing library for .NET applications. \
It provides a structured way to define commands, options, arguments, and aliases, \
with built-in support for mandatory/optional elements, default values, and error handling.

## Implementção padrão

O pacote possui objetos padrão para parses mais simples.
```c#
// Exemplo simples
using System;
using System.IO;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

internal partial class Program {

	private static readonly IFunction[] functions = [
		new DefaultFunction(
			"remove/-r", 1,
			new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIDefaultToken.Argument)}", 2)
		),
		new DefaultFunction(
			"create/-c", 2,
			new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIDefaultToken.Argument)}", 2)
		)
	];

	private static void remove_func(CLIKey key, CLIValueOrder valueOrder) {
		Console.WriteLine($"Run({key})");
		if (key == "remove" || key == "-r") {
			string path = Environment.CurrentDirectory;
			path = Path.Combine(path, valueOrder["arg1"]);
			if (File.Exists(path)) { 
				Console.WriteLine(path);
				File.Delete(path);
			} else Console.WriteLine($"No-exit:{path}");
		}
	}

	private static void create_func(CLIKey key, CLIValueOrder valueOrder) {
		Console.WriteLine($"Run({key})");
		if (key == "create" || key == "-c") {
			string path = Environment.CurrentDirectory;
			path = Path.Combine(path, valueOrder["arg1"]);
			if (!File.Exists(path)) {
				Console.WriteLine(path);
				File.Create(path).Dispose();
			}
			else Console.WriteLine($"No-exit:{path}");
		}
	}

	private static void defValueEmpty(CLIValueOrder valueOrder) { }

	private static void Main(string[] args) {
		Console.WriteLine("Program.Main");
		Console.WriteLine("Inicializado!");

		CLIParse.AddFunction(1, remove_func);
		CLIParse.AddFunction(2, create_func);
		CLIParse.AddFunction(3, defValueEmpty);

		CLIParse.AddToken((long)CLIDefaultToken.Function, "remove", "-r", "create", "-c");

		List<KeyValuePair<string, long>> l = CLIParse.Parse(args);

		TokenList list = new(l);
		ErrorMessage message = ErrorMessage.Default;
		list.Move();
		if (Analyzer(functions, list, message)) {
			Console.WriteLine($"alz-msm:\r\n{message}");
			return;
		}
		list.Reset();
		list.Move();

		foreach (IFunction item in functions) {
			if (item.IsAlias(list.CurrentKey)) {
				list.Move();
				if(item.GetValues(list, message)) {
					Console.WriteLine($"msm:\r\n{message}");
					return;
				}
				item.Run();
				break;
			}
		}
		Console.WriteLine("Finalizado!");
	}

	private static bool Analyzer(IFunction[] functions, TokenList list, ErrorMessage message) {
		foreach (IFunction item1 in functions) {
			if (item1.IsAlias(list.CurrentKey))
				if (item1.Analyzer(list, message))
					return true;
		}
		return false;
	}
}
```

## Implementção customizado

Para criar uma implementção customizada para situações não simples deve-se usar as
interfaces `IFunction`, `IOption` e `IArgument`.
`IFunction` e a interface para criar uma função.
`IOption` e a interface para criar uma opção para função.
`IArgument` e a interface para criar uma argumento para opção e função.

```
//Exemplo argumento
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

		if (value.Value != TypeCode) {
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
```
```
//Exemplo de função
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
		return false;
	}
}
```

```
//Exemplo de opção
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
			}
			else if (!IsAlias(list.CurrentKey))
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
		message.ErroCode = 22;
		message.Message = $"The element '({(CLIDefaultToken)value.Value}){value.Key}' is called before '({(CLIDefaultToken)TypeCode}){alias}'!!!";
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
```

alem disso o enumerador `CLIDefaultToken` pode ser substituido pro um enumerador customizado pelo fato do parser
usar um numero envez de um enumerador.
Só o codigo de `Argument` e `EndCode` deven ser deifidos nas propriedades `CLIParse.ArgumentCode {get;set;}` e `CLIParse.EndCode {get;set;}`.
