namespace Cobilas.CLI.Manager;
/// <summary>Defines the default token types used in the CLI parsing system.</summary>
[System.Flags]
public enum CLIDefaultToken : byte {
	/// <summary>Represents a function token.</summary>
	Function = 2,
	/// <summary>Represents an option token.</summary>
	Option = 4,
	/// <summary>Represents an argument token.</summary>
	Argument = 8,
	/// <summary>Represents the end-of-input marker.</summary>
	EndCode = 16
}