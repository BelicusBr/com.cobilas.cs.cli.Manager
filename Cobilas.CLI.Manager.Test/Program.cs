using System;
using System.Linq;
using Cobilas.CLI.Manager;

internal class Program {
	private static void Main(string[] args) {
		Console.WriteLine("Program.Main");
		
		CLIParse.AddToken("remove", CLIToken.Function);
		CLIParse.AddToken("-r", CLIToken.Function);
		CLIParse.AddToken("version", CLIToken.Function);
		CLIParse.AddToken("-v", CLIToken.Function);
		CLIParse.AddToken("add", CLIToken.Function);
		CLIParse.AddToken("-a", CLIToken.Function);
		CLIParse.AddToken("-folder", CLIToken.Option);
		CLIParse.AddToken("-file", CLIToken.Option);
		CLIParse.AddToken("--fd", CLIToken.Option);
		CLIParse.AddToken("--fl", CLIToken.Option);


		foreach ((string, CLIToken) item in CLIParse.Parse(args).Select(v => ((string, CLIToken))v))
			Console.WriteLine(item);
	}
}