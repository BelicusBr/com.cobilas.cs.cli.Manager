using System;

namespace Cobilas.CLI.Manager.Test;

public abstract class CLIBase : IDisposable
{
    private bool disposedValue;

    public string[] Alias { get; protected set; }

    protected CLIBase(string[] alias)
    {
        Alias = alias;
    }

    ~CLIBase()
        => IDispose(disposing: false);

    public abstract bool ContainsAlias(string alias);

    /// <inheritdoc/>
    public void Dispose()
    {
        IDispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void IDispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Alias = [];
            }
            Dispose(disposing);
            disposedValue = true;
        }
    }

    protected virtual void Dispose(bool disposing) { }
    
    public static string[] AliasToArray(string alias) => alias.Split(['/', '\\'], StringSplitOptions.RemoveEmptyEntries);
}
