using System;

namespace Cobilas.CLI.Manager.Test;

public sealed class CLICommand(string alias, CLIOption[] options, params CLIBase[] commands) : CLIBase(AliasToArray(alias)) {

    public CLIOption[] Options { get; private set; } = options;
    public CLIBase[] Commands { get; private set; } = commands;

    public CLICommand(string alias, params CLIBase[] commands) : this(alias, [], commands) { }

    public override bool ContainsAlias(string alias)
    {
        if (Alias is null || Alias.Length == 0) return false;
        for (int I = 0; I < Alias.Length; I++)
            if (Alias[I] == alias)
                return true;
        return false;
    }

    protected override void Dispose(bool disposing) => base.Dispose(disposing);
}
