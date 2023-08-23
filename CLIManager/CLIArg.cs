namespace Cobilas.CLI.Manager {
    /// <summary>Represents an argument.</summary>
    public struct CLIArg {
        private readonly string arg;
        private readonly string value;

        /// <summary>Target argument.</summary>
        public string Arg => arg;
        /// <summary>Argument entered in the cli.</summary>
        public string Value => value;

        /// <param name="arg">Target argument.</param>
        /// <param name="value">Argument entered in the cli.</param>
        public CLIArg(string arg, string value) {
            this.arg = arg;
            this.value = value;
        }

        public override string ToString()
            => $"Arg{{{arg}}} Value{{{value}}}";
    }
}
