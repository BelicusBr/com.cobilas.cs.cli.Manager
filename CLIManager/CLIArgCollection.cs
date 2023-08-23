using System;
using System.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager {
    /// <summary>Represents a collection of arguments.</summary>
    public sealed class CLIArgCollection : IEnumerable<CLIArg>, ICollection<CLIArg>, IReadOnlyList<CLIArg> {
        private CLIArg[] args;

        public int Count => args == null ? 0 : args.Length;
        public bool IsReadOnly => args != null && args.IsReadOnly;

        public CLIArg this[int index] => ((IReadOnlyList<CLIArg>)args)[index];

        public CLIArgCollection() {
            args = Array.Empty<CLIArg>();
        }

        public void Add(CLIArg item) {
            Array.Resize(ref args, Count + 1);
            args[Count-1] = item;
        }

        public bool Contains(string arg)
            => IndexOf(arg) >= 0;

        public int IndexOf(string arg) {
            for (int I = 0; I < Count; I++)
                if (this[I].Arg == arg)
                    return I;
            return -1;
        }

        public void Add(string arg, string value)
            => Add(new CLIArg(arg, value));

        public void Clear() {
            Array.Clear(args, 0, Count);
            args = Array.Empty<CLIArg>();
        }

        public void CopyTo(CLIArg[] array, int arrayIndex) {
            array.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CLIArg> GetEnumerator() {
            for (int I = 0; I < Count; I++)
                yield return args[I];
        }

        bool ICollection<CLIArg>.Contains(CLIArg item) {
            return false;
        }

        bool ICollection<CLIArg>.Remove(CLIArg item) {
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            for (int I = 0; I < Count; I++)
                yield return args[I];
        }
    }
}
