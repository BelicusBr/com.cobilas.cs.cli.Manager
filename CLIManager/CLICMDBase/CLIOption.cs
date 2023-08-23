using System;

namespace Cobilas.CLI.Manager {
    /// <summary>Represents the options that will be inserted in the CLT.</summary>
    public sealed class CLIOption : CLICMDBase {
        private readonly int argCount;
        private readonly string alias2;
        private readonly string[] alias;
        private readonly bool isOptional;

        /// <summary>The number of arguments.</summary>
        public int ArgCount => argCount;
        /// <summary>Indicates whether the option is optional.</summary>
        public bool IsOptional => isOptional;
        public override string Alias => alias2;

        /// <param name="argCount">The number of arguments.</param>
        /// <param name="alias">The nickname of the option (exp1:init)(exp2:init/-i).</param>
        /// <param name="isOptional">Indicates whether the option is optional.</param>
        public CLIOption(int argCount, string alias, bool isOptional) {
            this.argCount = argCount;
            this.isOptional = isOptional;
            this.alias = (this.alias2 = alias).Split('/');
        }

        /// <param name="argCount">The number of arguments.</param>
        /// <param name="alias">The nickname of the option (exp1:init)(exp2:init/-i).</param>
        public CLIOption(int argCount, string alias) : this(argCount, alias, true) { }

        /// <param name="alias">The nickname of the option (exp1:init)(exp2:init/-i).</param>
        /// <param name="isOptional">Indicates whether the option is optional.</param>
        public CLIOption(string alias, bool isOptional) : this(0, alias, isOptional) { }

        /// <param name="alias">The nickname of the option (exp1:init)(exp2:init/-i).</param>
        public CLIOption(string alias) : this(0, alias) { }

        public override void Dispose() {
            if (alias != null && alias.Length != 0)
                Array.Clear(alias, 0, alias.Length);
        }

        public override bool Equals(object obj)
            => obj is string stg && Equals(stg);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(string other) {
            for (int I = 0; I < alias.Length; I++)
                if (alias[I] == other)
                    return true;
            return false;
        }

        public static bool operator ==(CLIOption A, string B) => (object)A != (object)null && A.Equals(B);
        public static bool operator !=(CLIOption A, string B) => !(A == B);

        public static bool operator ==(string A, CLIOption B) => (object)B != (object)null && B.Equals(A);
        public static bool operator !=(string A, CLIOption B) => !(A == B);
    }
}
