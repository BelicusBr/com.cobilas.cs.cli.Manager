# Cobilas CLI Manager

A powerful and flexible command-line interface (CLI) parsing library for .NET applications, designed to handle complex command structures with ease.

## Installation

### Package Manager
```bash
Install-Package Cobilas.CLI.Manager
```

### .NET CLI
```bash
dotnet add package Cobilas.CLI.Manager
```

### Package Reference
```xml
<PackageReference Include="Cobilas.CLI.Manager" Version="1.0.0" />
```

## Description

Cobilas CLI Manager provides a structured approach to parsing command-line arguments in .NET applications. It offers:

- **Type-safe command definitions** using interfaces and structs
- **Flexible token system** with support for functions, options, and arguments
- **Customizable error handling** with detailed error messages
- **Pattern-based parsing** with inline and block patterns
- **Extensible architecture** allowing custom option and function implementations

## Technical Characteristics

- **Target Frameworks**: .NET Standard 2.0+, .NET 6.0+, .NET 7.0+, .NET 8.0+
- **Dependencies**: None (self-contained)
- **License**: MIT

Function IDs and Events
The library uses predefined function IDs to handle different aspects of CLI processing:

Function IDs
/* Functions ID
 * 0) IOptionFunc.DefaultValue
 * 1) IOptionFunc.ExceptionMessage
 * 2) IOptionFunc.TreatedValue
 * 3) IFunction.GetValues
 * 4) IFunction.Run
 * 5) ICLIAnalyzer.Analyzer
 */
Function Internal Events
/* Function internal events
 * 0) IOptionFunc.DefaultValue => Action<CLIKey, CLIValueOrder?, ErrorMessage?>
 * 1) IOptionFunc.ExceptionMessage => Action<CLIKey, TokenList?, KeyValuePair<string, long>, ErrorMessage?>
 * 2) IOptionFunc.TreatedValue => Action<CLIKey, CLIValueOrder?, TokenList?, ErrorMessage?>
 * 3) IFunction.GetValues => Func<CLIKey, TokenList?, CLIValueOrder?, List<IOptionFunc>?, ErrorMessage?, bool>
 * 4) IFunction.Run => Action<CLIKey, CLIValueOrder?, ErrorMessage?>
 * 5) ICLIAnalyzer.Analyzer => Func<CLIKey, TokenList?, List<IOptionFunc>?, ErrorMessage?, bool>
 */
Key Components

### Core Types
- `CLIParse`: Main parsing class with static methods for token registration and parsing
- `CLIKey`: Represents compound keys that can match multiple alias strings
- `CLIValueOrder`: Ordered collection of key-value pairs for storing parsed values
- `TokenList`: List of token key-value pairs with cursor navigation
- `ErrorMessage`: Container for error information during parsing

### Interfaces
- `IAlias`: Base interface for all alias entities
- `IArgument`: Represents command-line arguments
- `ICLIAnalyzer`: Defines contract for analyzing token lists
- `IFunction`: Represents CLI functions with options and execution logic
- `IOption`: Represents CLI options that can contain arguments
- `IOptionFunc`: Combines alias behavior with value handling capabilities

### Patterns (Inline)
- `LineArgument`: Represents a command-line argument
- `LineBlock`: Combines multiple options into a processing unit
- `LineEndOption`: Marks termination of command-line processing
- `LineFunction`: Main function with alias and option processing
- `LineOption`: Command-line option with jump behavior

### Exceptions
- `InvalidCLIArgumentException`: Invalid CLI argument error
- `InvalidCLIArgumentTypeException`: Invalid CLI argument type error
- `InvalidCLIFunctionException`: Invalid CLI function error
- `InvalidCLIOptionException`: Invalid CLI option error
- `NotDescribedException`: Parser rule violation without description

## Implementation Details

The library uses a token-based parsing system where:
1. Tokens are registered with type codes (Function, Option, Argument, EndCode)
2. Command-line arguments are converted to token sequences
3. Functions define expected patterns using option elements
4. The parser validates input against defined patterns
5. Values are extracted and stored in CLIValueOrder collections

## Usage

### Standard Usage

```csharp
using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.Manager.Patterns.InLine;

internal partial class Program {
    private static void Main(string[] args) {
        // Configure parsing settings
        CLIParse.EndCode = (long)CLIDefaultToken.EndCode;
        CLIParse.ArgumentCode = (long)CLIDefaultToken.Argument;

        // Register tokens
        CLIParse.AddToken((long)CLIDefaultToken.Function, "tdsf-1", "tdsf-2");
        CLIParse.AddToken((long)CLIDefaultToken.Option, "tdsO-1", "tdsO-2", "tdsO-3");
        CLIParse.AddToken((long)CLIDefaultToken.Option | CLIParse.EndCode, "tdsO-1E");

        // Register function handlers
        CLIParse.AddFunction(0u, def_value);
        CLIParse.AddFunction(1u, error_value);
        CLIParse.AddFunction(2u, get_value);
        CLIParse.AddFunction(3u, LineFunction.FunctionGetValues);
        CLIParse.AddFunction(5u, LineFunction.FunctionAnalyzer);

        // Define functions with their patterns
        IFunction[] functions = {
            new LineFunction("tdsf-1",
                new LineEndOption("tdsO-1E", false),
                new LineOption("tdsO-1", false, 2),
                new LineOption("tdsO-2", true, 0),
                new LineArgument("arg{100}", true),
                new LineOption("tdsO-3", false, 2),
                new LineOption("tdsO-2", true, 0),
                new LineArgument("arg{100}", true)
            ),
            new LineFunction("tdsf-2",
                new LineEndOption("tdsO-1E", false),
                new LineBlock("tdsO-1", false,
                    new LineOption("tdsO-2", true, 0),
                    new LineArgument("arg{100}", true)
                ),
                new LineBlock("tdsO-3", false,
                    new LineOption("tdsO-2", true, 0),
                    new LineArgument("arg{100}", true)
                )
            )
        };

        // Parse and process arguments
        TokenList list = new(CLIParse.Parse(args));
        ErrorMessage message = ErrorMessage.Default;
        list.Move();

        foreach (IFunction item in functions) {
            if (!item.IsAlias(list.CurrentKey)) continue;
            
            if (item is ICLIAnalyzer alz && alz.Analyzer(list, message)) {
                Console.WriteLine(message);
                return;
            }
            
            list.Reset();
            list.Move();
            
            if (item.GetValues(list, message)) {
                Console.WriteLine(message);
                return;
            }

            if (item.Run(message)) {
                Console.WriteLine(message);
                return;
            }
        }
    }

    // Function implementations
    private static void def_value(CLIKey alias, CLIValueOrder? valueOrder, ErrorMessage? message) {
        if (alias == (CLIKey)"arg{100}")
            valueOrder.Add((CLIKey)"arg{100}", Environment.OSVersion.ToString());
        else valueOrder.Add(alias, $"def-arg-{alias}");
    }
    
    private static void error_value(CLIKey alias, TokenList? list, KeyValuePair<string, long> value, ErrorMessage? message) {
        message.Message = $"({alias})|{value}";
    }
    
    private static void get_value(CLIKey alias, CLIValueOrder? valueOrder, TokenList? list, ErrorMessage? message) {
        if (alias == (CLIKey)"arg{100}")
            valueOrder.Add((CLIKey)"arg{100}", list.CurrentKey);
        else valueOrder.Add(list.CurrentKey, $"arg-{list.CurrentKey}");
    }
}
```

### Custom Usage

```csharp
public class NumericRangeOption : IOption, ICLIAnalyzer
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
        _arguments = [
            new DefaultArgument(true, "min/{ARG}", 0),
            new DefaultArgument(true, "max/{ARG}", 0)
        ];
    }

    public bool Analyzer(TokenList? list, ErrorMessage? message)
    {
        // Custom analysis logic
        if (list is null || message is null) return true;
        // Implementation details...
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
        // Extract numeric arguments
        string min = list!.GetValueAndMove.Key;
        string max = list!.GetValueAndMove.Key;
        valueOrder!.Add("min", min);
        valueOrder.Add("max", max);
    }

    public void DefaultValue(CLIValueOrder? valueOrder)
    {
        // Provide defaults
        valueOrder!.Add("min", "0");
        valueOrder.Add("max", "100");
    }
}
```

## Explanation

The Cobilas CLI Manager works through several key concepts:

1. **Tokenization**: Command-line arguments are converted into tokens with type codes
2. **Pattern Matching**: Functions define expected patterns of tokens (options, arguments)
3. **Validation**: The input is validated against the defined patterns
4. **Value Extraction**: Valid values are extracted and stored in order
5. **Execution**: Functions can execute logic using the extracted values

The library supports both simple linear patterns and complex nested patterns through blocks, making it suitable for everything from simple utilities to complex command-line applications.

## License

This project is licensed under the MIT License - see the LICENSE file for details.