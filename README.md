# Cobilas.CLI.Manager
A powerful and flexible command-line interface (CLI) parser library for .NET applications.  
It provides both simple, ready‑to‑use components for common CLI tasks and abstract interfaces for building highly customised command parsers.

---

## 📦 Installation

Install the [NuGet package](https://www.nuget.org/packages/Cobilas.CLI.Manager):

```bash
dotnet add package Cobilas.CLI.Manager
```

Or via the Package Manager Console:

```powershell
Install-Package Cobilas.CLI.Manager
```

---

## ✨ Features

- **Simple, declarative CLI definition**  
  Use `DefaultFunction`, `DefaultOption`, and `DefaultArgument` to quickly build commands with minimal code.

- **Extensible architecture**  
  Implement interfaces like `IFunction`, `IOption`, `IArgument`, and `IOptionFunc` to create fully customised parsing logic.

- **Token‑based parsing**  
  The `CLIParse` class converts raw command‑line arguments into a list of tokens (key‑value pairs with type codes).  
  Built‑in token types are defined in `CLIDefaultToken` (Function, Option, Argument, EndCode).

- **Alias support**  
  Every CLI element can have multiple aliases (e.g., `"create/-c"`). The `CLIKey` structure handles compound keys.

- **Value order management**  
  `CLIValueOrder` collects processed values (arguments, option parameters) and makes them available to your functions.

- **Error handling**  
  The `ErrorMessage` class and the `ICLIAnalyzer` interface allow you to detect and report parsing errors gracefully.

- **Integrated function registry**  
  Register delegates with `CLIParse.AddFunction()` and reference them by ID in your default components – ideal for separating logic from structure.

---

## 🚀 Usage

### Standard Implementation

The following example demonstrates a simple file management tool with two commands: `remove` (or `-r`) and `create` (or `-c`).  
Each command expects one argument (a file name).

```csharp
using System;
using System.IO;
using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Interfaces;
using System.Collections.Generic;

internal partial class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Program.Main started.");

        // Register function delegates with unique IDs
        CLIParse.AddFunction(1, remove_func);
        CLIParse.AddFunction(2, create_func);
        CLIParse.AddFunction(3, defValueEmpty);

        // Define tokens (the strings that will be recognised as commands)
        CLIParse.AddToken((long)CLIDefaultToken.Function, "remove", "-r", "create", "-c");

        // Describe the available functions and their expected arguments
        IFunction[] functions =
        [
            new DefaultFunction(
                "remove/-r", 1,
                new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIDefaultToken.Argument)}", 2)
            ),
            new DefaultFunction(
                "create/-c", 2,
                new DefaultArgument(true, $"arg1/{{ARG}}/{nameof(CLIDefaultToken.Argument)}", 2)
            )
        ];

        // Parse the command line into a token list
        List<KeyValuePair<string, long>> tokenPairs = CLIParse.Parse(args);
        using TokenList list = new(tokenPairs);
        ErrorMessage message = ErrorMessage.Default;

        // Move to the first token (the command)
        list.Move();

        foreach (IFunction function in functions)
        {
            if (function.IsAlias(list.CurrentKey))
            {
                // Analyse the token stream for this function
                if (function.Analyzer(list, message))
                {
                    Console.WriteLine($"Analysis error:\n{message}");
                    break;
                }

                // Reset the list and skip the command token to read arguments
                list.Reset();
                list.Move(2);

                // Retrieve values into the function's ValueOrder
                if (function.GetValues(list, message))
                {
                    Console.WriteLine($"Value error:\n{message}");
                    break;
                }

                // Execute the function
                function.Run();
                break;
            }
        }

        Console.WriteLine("Program.Main finished.");
    }

    private static void remove_func(CLIKey key, CLIValueOrder valueOrder)
    {
        if (key == "remove" || key == "-r")
        {
            string path = Path.Combine(Environment.CurrentDirectory, valueOrder["arg1"]!);
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine($"Removed: {path}");
            }
            else
            {
                Console.WriteLine($"File not found: {path}");
            }
        }
    }

    private static void create_func(CLIKey key, CLIValueOrder valueOrder)
    {
        if (key == "create" || key == "-c")
        {
            string path = Path.Combine(Environment.CurrentDirectory, valueOrder["arg1"]!);
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                Console.WriteLine($"Created: {path}");
            }
            else
            {
                Console.WriteLine($"File already exists: {path}");
            }
        }
    }

    private static void defValueEmpty(CLIValueOrder? valueOrder) { }
}
```

**Explanation**

1. **Delegate registration** – Functions are stored in `CLIParse` with an ID. Later, `DefaultFunction` uses that ID to invoke the right delegate.
2. **Token definition** – `CLIParse.AddToken` tells the parser which strings correspond to function tokens.
3. **Function description** – Each `DefaultFunction` defines its alias, its registered delegate ID, and its arguments.
4. **Parsing** – `CLIParse.Parse` returns a list of `KeyValuePair<string, long>` where the key is the original argument and the value is its token type.
5. **Analysis** – `Analyzer` checks if the token stream matches the expected structure.
6. **Value extraction** – `GetValues` populates the `ValueOrder` dictionary with the actual argument strings.
7. **Execution** – `Run` calls the registered delegate, passing the alias and the collected values.

### Custom Implementation

For complex CLI requirements, you can create your own implementations of the core interfaces.

#### Key Interfaces

| Interface      | Purpose                                                                                  |
|----------------|------------------------------------------------------------------------------------------|
| `IAlias`       | Provides an alias string and a type code.                                                |
| `ICLIAnalyzer` | Defines an `Analyzer` method to validate a token list against the element’s structure.   |
| `IOptionFunc`  | Base for options and arguments; adds mandatory flag, default value, and value treatment. |
| `IArgument`    | Marker for arguments (inherits `IOptionFunc`).                                           |
| `IOption`      | Represents an option that can contain a list of arguments.                               |
| `IFunction`    | Represents a command with a collection of options and a `ValueOrder`.                    |

#### Example: A custom option that expects two numeric arguments

```csharp
public class NumericRangeOption : IOption
{
    private readonly CLIKey _alias;
    private readonly List<IArgument> _arguments;

    public string Alias => _alias;
    public long TypeCode => (long)CLIDefaultToken.Option;
    public bool Mandatory { get; }
    public List<IArgument>? Options => _arguments;

    public NumericRangeOption(string alias, bool mandatory)
    {
        _alias = new CLIKey(alias);
        Mandatory = mandatory;
        _arguments =
        [
            new DefaultArgument(true, "min/{ARG}", 0), // ID 0 = default value function
            new DefaultArgument(true, "max/{ARG}", 0)
        ];
    }

    public bool Analyzer(TokenList? list, ErrorMessage? message)
    {
        // Custom analysis logic
        if (list is null || message is null) return true;
        // ... implementation
        return false;
    }

    public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message)
    {
        message!.ErroCode = 42;
        message.Message = $"Unexpected token: {value.Key}";
    }

    public bool IsAlias(string? alias) => _alias.Equals(alias);

    public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list)
    {
        // Extract the two numeric arguments and store them under meaningful names
        string min = list!.GetValueAndMove.Key;
        string max = list!.GetValueAndMove.Key;
        valueOrder!.Add("min", min);
        valueOrder.Add("max", max);
    }

    public void DefaultValue(CLIValueOrder? valueOrder)
    {
        // Provide defaults if the option is omitted
        valueOrder!.Add("min", "0");
        valueOrder.Add("max", "100");
    }
}
```

Then use this custom option inside a `DefaultFunction` or a custom `IFunction`.

---

## 📚 API Overview

### Core Classes

- **`CLIParse`** – Static entry point for token registration, function registration, and parsing arguments.
- **`CLIKey`** – Represents a compound key that can match multiple alias strings (e.g., `"create/-c"`). Supports equality comparisons and implicit conversions.
- **`CLIValueOrder`** – A dictionary‑like collection that stores key‑value pairs after parsing. Keys are `CLIKey` objects; values are strings.
- **`TokenList`** – Wraps a list of `KeyValuePair<string, long>` with a movable cursor. Used by analyzers and value extractors.
- **`ErrorMessage`** – Container for error information (code, message, unique ID).

### Default Implementations

- **`DefaultFunction`** – Implements `IFunction`. Accepts an alias, a registered delegate ID, and an array of `IOptionFunc` (options/arguments).
- **`DefaultOption`** – Implements `IOption`. Accepts an alias, mandatory flag, registered delegate IDs for argument naming and default value, and an array of `IArgument`.
- **`DefaultArgument`** – Implements `IArgument`. Accepts a mandatory flag, an alias, and a registered delegate ID for default value injection.

### Enums

- **`CLIDefaultToken`** – Predefined token types: `Function = 1`, `Option = 2`, `Argument = 3`, `EndCode = 4`.

---

## 🧪 Advanced Topics

### Token Customisation

You are not limited to the default token types. When adding tokens with `CLIParse.AddToken`, you can assign any `long` value as the token ID. This allows you to create your own type system.

### Function Registry

`CLIParse` maintains a dictionary of delegates keyed by `uint`. This lets you separate the definition of a command’s structure from its implementation – useful for plugin architectures or dynamic CLI generation.

### Error Handling Flow

1. **`Analyzer`** – Checks if the token stream matches the expected pattern. Returns `true` on error and populates `ErrorMessage`.
2. **`GetValues`** – Extracts values into `CLIValueOrder`. Returns `true` on error.
3. **`Run`** – Executes the command logic, using the values stored in `ValueOrder`.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE.md).  
Feel free to use it in personal or commercial projects.

---

*Happy coding with Cobilas.CLI.Manager!*