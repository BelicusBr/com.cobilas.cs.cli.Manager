namespace Cobilas.CLI.Manager.Interfaces;

public interface IAlias {
	string Alias { get; }
	long TypeCode { get; }
	bool IsAlias(string alias);
}