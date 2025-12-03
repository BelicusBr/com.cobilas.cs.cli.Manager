using System;

namespace Cobilas.CLI.Manager {
    /// <summary>Represents cli cmd base object.</summary>
    public abstract class CLICMDBase : IDisposable, IEquatable<string> {
        /// <summary>Returns the nickname of the command.</summary>
        public abstract string Alias { get; }

        public abstract void Dispose();
        public abstract bool Equals(string other);
    }
}
