﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager {
    /// <summary>Represents the errors to be shown in the CLI.</summary>
    public sealed class ErrorMensager: IReadOnlyList<string> {
        private bool mod;
        private string[] msm;

        public bool Modifir => mod;
        public int Count => ((IReadOnlyCollection<string>)msm).Count;

        public string this[int index] => ((IReadOnlyList<string>)msm)[index];

        public ErrorMensager() => msm = Array.Empty<string>();

        /// <summary>Add error messages.</summary>
        public void Add(string msm) {
            mod = true;
            Array.Resize(ref this.msm, Count + 1);
            this.msm[Count - 1] = msm;
        }

        public IEnumerator<string> GetEnumerator()
            => ((IEnumerable<string>)msm).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => msm.GetEnumerator();
    }
}
