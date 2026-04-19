global using AnalyzerFunc = System.Func<
	Cobilas.CLI.Manager.CLIKey,
	Cobilas.CLI.Manager.TokenList?,
	System.Collections.Generic.List<Cobilas.CLI.Manager.Interfaces.IOptionFunc>?,
	Cobilas.CLI.Manager.ErrorMessage?, bool>;
global using DefaultValueFunc = System.Action<
	Cobilas.CLI.Manager.CLIKey,
	Cobilas.CLI.Manager.CLIValueOrder?,
	Cobilas.CLI.Manager.ErrorMessage?
	>;
global using RunFunc = System.Func<
	Cobilas.CLI.Manager.CLIKey,
	Cobilas.CLI.Manager.CLIValueOrder?,
	Cobilas.CLI.Manager.ErrorMessage?,
	bool>;
global using GetValuesFunc = System.Func<
	Cobilas.CLI.Manager.CLIKey,
	Cobilas.CLI.Manager.TokenList?,
	Cobilas.CLI.Manager.CLIValueOrder?,
	System.Collections.Generic.List<Cobilas.CLI.Manager.Interfaces.IOptionFunc>?,
	Cobilas.CLI.Manager.ErrorMessage?, bool>;
global using ExceptionMessageFunc = System.Action<
	Cobilas.CLI.Manager.CLIKey,
	Cobilas.CLI.Manager.TokenList?,
	System.Collections.Generic.KeyValuePair<string, long>, 
	Cobilas.CLI.Manager.ErrorMessage?>;
global using TreatedValueFunc = System.Action<
	Cobilas.CLI.Manager.CLIKey, 
	Cobilas.CLI.Manager.CLIValueOrder?, 
	Cobilas.CLI.Manager.TokenList?, 
	Cobilas.CLI.Manager.ErrorMessage?>;