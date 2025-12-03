using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobilas.CLI.Manager;

public sealed class CommandTree(CLICommand[,]? commands) {
	private CLICommand[,]? commands = commands;
	private List<(string, long)> parse;

	public void AddParse(string[] args) => parse = CLIParse.Parse(args);

	public void ForEach(Action<(string, long), CLICommand> action) {

	}
}
