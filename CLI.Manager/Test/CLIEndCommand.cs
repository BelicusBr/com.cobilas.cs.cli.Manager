using System;

namespace Cobilas.CLI.Manager.Test;

public sealed class CLIEndCommand(Func<object[], object> func) : CLIBase(["end-command"]) {

    public Func<object[], object> Func { get; private set; } = func;

    public override bool ContainsAlias(string alias) {
        if (Alias is null || Alias.Length == 0) return false;
        for (int I = 0; I < Alias.Length; I++)
            if (Alias[I] == alias)
                return true;
        return false;
    }

    protected override void Dispose(bool disposing) => base.Dispose(disposing);
}
