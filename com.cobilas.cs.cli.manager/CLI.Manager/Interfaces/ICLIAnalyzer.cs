namespace Cobilas.CLI.Manager.Interfaces;

public interface ICLIAnalyzer
{
	bool Analyzer(TokenList list, ErrorMessage message);
}