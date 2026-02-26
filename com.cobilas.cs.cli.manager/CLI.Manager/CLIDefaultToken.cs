namespace Cobilas.CLI.Manager;
/// <summary>Defines the default token types used in the CLI parsing system.</summary>
public enum CLIDefaultToken : byte {
	/// <summary>Represents a function token.</summary>
	Function = 1,
	/// <summary>Represents an option token.</summary>
	Option = 2,
	/// <summary>Represents an argument token.</summary>
	Argument = 3,
	/// <summary>Represents the end-of-input marker.</summary>
	EndCode = 4
}