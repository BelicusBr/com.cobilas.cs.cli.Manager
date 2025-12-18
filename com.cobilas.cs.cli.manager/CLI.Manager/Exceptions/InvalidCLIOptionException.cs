using System;
using System.Runtime.Serialization;

namespace Cobilas.CLI.Manager.Exceptions;

[Serializable]
public class InvalidCLIOptionException : Exception {
	public InvalidCLIOptionException() { }
	public InvalidCLIOptionException(string message) : base(message) { }
	public InvalidCLIOptionException(string message, Exception inner) : base(message, inner) { }
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	protected InvalidCLIOptionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
}