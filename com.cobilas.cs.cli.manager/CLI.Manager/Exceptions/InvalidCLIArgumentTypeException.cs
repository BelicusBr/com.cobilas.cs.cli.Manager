using System;
using System.Runtime.Serialization;

namespace Cobilas.CLI.Manager.Exceptions;

[Serializable]
public class InvalidCLIArgumentTypeException : Exception {
	public InvalidCLIArgumentTypeException() { }
	public InvalidCLIArgumentTypeException(string message) : base(message) { }
	public InvalidCLIArgumentTypeException(string message, Exception inner) : base(message, inner) { }
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	protected InvalidCLIArgumentTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
}