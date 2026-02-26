namespace Cobilas.CLI.Manager.Interfaces;
/// <summary>
/// Represents an alias entity used within the command-line interface parsing system.
/// This interface serves as a base for defining aliases for commands, options, or other CLI elements.
/// </summary>
public interface IAlias {
	/// <summary>
	/// Gets the string representation of the alias.
	/// </summary>
	/// <returns>The alias string.</returns>
	string Alias { get; }
	/// <summary>
	/// Gets a numeric code that identifies the type or category of the alias.
	/// This can be used to distinguish between different kinds of aliases (e.g., command aliases, option aliases).
	/// </summary>
	/// <returns>A <see cref="long"/> value representing the type code.</returns>
	long TypeCode { get; }
	/// <summary>
	/// Determines whether the specified string matches this alias.
	/// </summary>
	/// <param name="alias">The string to compare against the alias. Can be null.</param>
	/// <returns><see langword="true"/> if the specified string matches the alias; otherwise, <see langword="false"/>.</returns>
	bool IsAlias(string? alias);
}