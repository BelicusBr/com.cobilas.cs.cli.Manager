using System;

namespace Cobilas.CLI.Manager {
    public abstract class CLICMDBase : IDisposable, IEquatable<string> {
        public abstract string Alias { get; }

        public abstract void Dispose();
        public abstract bool Equals(string other);
    }
}
