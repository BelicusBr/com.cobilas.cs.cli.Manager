using System;
using System.Runtime.Serialization;

namespace Cobilas.CLI.Manager.Exceptions;

[Serializable]
public class InvalidCLIArgumentException : Exception {
	public InvalidCLIArgumentException() { }
	public InvalidCLIArgumentException(string message) : base(message) { }
	public InvalidCLIArgumentException(string message, Exception inner) : base(message, inner) { }
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	protected InvalidCLIArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
}