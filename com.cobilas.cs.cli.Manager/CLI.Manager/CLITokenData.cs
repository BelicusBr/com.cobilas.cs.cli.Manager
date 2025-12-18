using System;

namespace Cobilas.CLI.Manager;

public abstract class CLITokenData {

	public static CLITokenData Default => new InfoTokenData();

	public Exception? Exception { get; set; }
}
