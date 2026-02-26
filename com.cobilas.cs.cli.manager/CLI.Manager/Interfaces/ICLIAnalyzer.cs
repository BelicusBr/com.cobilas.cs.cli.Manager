namespace Cobilas.CLI.Manager.Interfaces;
/// <summary>
/// Defines a contract for analyzing a token list and generating error messages during CLI parsing.
/// </summary>
public interface ICLIAnalyzer {
	/// <summary>
	/// Analyzes the provided token list and populates the error message if analysis fails.
	/// </summary>
	/// <param name="list">The token list to analyze. Can be null.</param>
	/// <param name="message">The error message container to populate in case of errors. Can be null.</param>
	/// <returns><see langword="true"/> if the analysis succeeds; otherwise, <see langword="false"/>.</returns>
	bool Analyzer(TokenList? list, ErrorMessage? message);
}