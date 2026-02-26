using System;
using System.Runtime.Serialization;

namespace Cobilas.CLI.Manager.Exceptions;
/// <summary>
/// Represents an error that occurs when an invalid CLI function is encountered.
/// </summary>
[Serializable]
public class InvalidCLIFunctionException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidCLIFunctionException"/> class.
	/// </summary>
	public InvalidCLIFunctionException() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidCLIFunctionException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public InvalidCLIFunctionException(string message) : base(message) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidCLIFunctionException"/> class with a specified error message
	/// and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
	public InvalidCLIFunctionException(string message, Exception inner) : base(message, inner) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidCLIFunctionException"/> class with serialized data.
	/// </summary>
	/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
	/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
	/// <remarks>
	/// This constructor is obsolete for .NET 8.0 and later due to <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/serialization/8.0/deserializing-records#reason-for-change">SYSLIB0051</see>.
	/// </remarks>
#if NET8_0_OR_GREATER
	[Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	protected InvalidCLIFunctionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	/// <summary>
	/// Sets the <see cref="SerializationInfo"/> with information about the exception.
	/// </summary>
	/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
	/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
	/// <remarks>
	/// This method is obsolete for .NET 8.0 and later due to <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/serialization/8.0/deserializing-records#reason-for-change">SYSLIB0051</see>.
	/// </remarks>
#if NET8_0_OR_GREATER
	[Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
}