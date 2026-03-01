using System.Collections.Generic;

namespace Cobilas.CLI.Manager.Interfaces;
/// <summary>
/// Represents an option in the command-line interface, which can contain a collection of arguments.
/// </summary>
/// <seealso cref="IOptionFunc"/>
public interface IOption : IOptionFunc {
	/// <summary>
	/// Gets the list of arguments associated with this option.
	/// </summary>
	/// <returns>A list of <see cref="IArgument"/> objects, or null if no arguments are defined.</returns>
	List<IArgument>? Options { get; }
}