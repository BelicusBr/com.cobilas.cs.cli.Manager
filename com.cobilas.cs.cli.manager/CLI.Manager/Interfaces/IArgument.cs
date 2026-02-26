namespace Cobilas.CLI.Manager.Interfaces;
/// <summary>
/// Represents an argument in the command-line interface parsing system.
/// This interface serves as a marker for objects that can function as both an argument and an option function.
/// </summary>
/// <seealso cref="IOptionFunc"/>
public interface IArgument : IOptionFunc { }