using System;
using System.Runtime.Serialization;

namespace Cobilas.CLI.Manager.Exceptions;

[Serializable]
public class NotDescribedException : Exception {

	public static NotDescribedException RuleViolated => new("The parser rule was violated but not described!");

	public NotDescribedException() { }
	public NotDescribedException(string message) : base(message) { }
	public NotDescribedException(string message, Exception inner) : base(message, inner) { }
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	protected NotDescribedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
}