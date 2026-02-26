using System;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Represents an error message with a code and identifier, used for CLI error handling.
/// </summary>
public sealed class ErrorMessage {
	/// <summary>
	/// Gets or sets the error code associated with the message.
	/// </summary>
	/// <value>The numeric error code.</value>
	/// <returns>The current error code.</returns>
	public int ErroCode { get; set; }
	/// <summary>
	/// Gets or sets the textual error message.
	/// </summary>
	/// <value>The error message string.</value>
	/// <returns>The current message.</returns>
	public string Message { get; set; }
	/// <summary>
	/// Gets the unique identifier for this error message instance.
	/// </summary>
	/// <returns>A <see cref="Guid"/> uniquely identifying this instance.</returns>
	public Guid Id { get; private set; }
	/// <summary>
	/// Gets a default <see cref="ErrorMessage"/> instance with empty values.
	/// </summary>
	/// <returns>A new default error message.</returns>
	public static ErrorMessage Default => new();
	/// <summary>
	/// Initializes a new instance of the <see cref="ErrorMessage"/> class with default values.
	/// </summary>
	public ErrorMessage() {
		ErroCode = -1;
		Message = nameof(string.Empty);
		Id = Guid.NewGuid();
	}
	/// <inheritdoc/>
	public override int GetHashCode() => Id.GetHashCode() >> 1 ^ ErroCode;
	/// <summary>
	/// Returns a string representation of the error message.
	/// </summary>
	/// <returns>A formatted string containing the error code, ID, and message.</returns>
	public override string ToString()
		=> $"{nameof(ErroCode)}: {ErroCode}\r\n{nameof(Id)}: {Id}\r\n{nameof(Message)}:{{\r\n{Message}\r\n}}";
}