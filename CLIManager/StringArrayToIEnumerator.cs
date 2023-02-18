using System.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager {
    public sealed class StringArrayToIEnumerator : IEnumerator<string> {
        private int index;
        private string[] list;
        private string current;

        public string Current => current;
        object IEnumerator.Current => current;
        public int Index { get => index; set => index = value; }

        public StringArrayToIEnumerator(string[] list) {
            if ((this.list = list) == null)
                this.list = System.Array.Empty<string>();
            Reset();
        }

        public void Dispose() {
            list = (string[])null;
        }

        public bool MoveNext() {
            if (++index < list.Length) {
                current = list[index];
                return true;
            }
            return false;
        }

        public void Reset() {
            index = -1;
        }
    }
}
