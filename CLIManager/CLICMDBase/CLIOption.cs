using System;

namespace Cobilas.CLI.Manager {
    public sealed class CLIOption : CLICMDBase {
        private readonly int argCount;
        private readonly string alias2;
        private readonly string[] alias;
        private readonly bool isOptional;

        public int ArgCount => argCount;
        public bool IsOptional => isOptional;
        public override string Alias => alias2;

        public CLIOption(int argCount, string alias, bool isOptional) {
            this.argCount = argCount;
            this.isOptional = isOptional;
            this.alias = (this.alias2 = alias).Split('/');
        }

        public CLIOption(int argCount, string alias) : this(argCount, alias, true) { }

        public CLIOption(string alias, bool isOptional) : this(0, alias, isOptional) { }

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
