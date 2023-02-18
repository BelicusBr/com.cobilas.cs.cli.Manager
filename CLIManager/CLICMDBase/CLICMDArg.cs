namespace Cobilas.CLI.Manager {
    public sealed class CLICMDArg : CLICMDBase {
        public const string alias = "{arg}";
        public override string Alias => alias;

        public static CLICMDArg Default => new CLICMDArg();

        public override void Dispose() { }

        public override bool Equals(object obj)
            => obj is string stg && Equals(stg);

        public override bool Equals(string other)
            => alias == other;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => alias;
    }
}
