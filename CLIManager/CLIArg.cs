namespace Cobilas.CLI.Manager {
    public struct CLIArg {
        private readonly string arg;
        private readonly string value;

        public string Arg => arg;
        public string Value => value;

        public CLIArg(string arg, string value) {
            this.arg = arg;
            this.value = value;
        }

        public override string ToString()
            => $"Arg{{{arg}}} Value{{{value}}}";
    }
}
