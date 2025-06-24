namespace Cobilas.CLI.Manager.Test;

public sealed class CLICMDNull : CLIBase {

    private static readonly CLICMDNull @null = new();

    public static CLICMDNull Null => @null;

    public CLICMDNull() : base([], null, null) { }

    public override bool ContainsAlias(string alias) => false;

    /// <inheritdoc/>
    protected override void Dispose(bool disposing) => base.Dispose(disposing);
}
