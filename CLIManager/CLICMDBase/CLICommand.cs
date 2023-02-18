using System;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager {
    public sealed class CLICommand : CLICMDBase {
        private readonly int id;
        private readonly int argCount;
        private readonly int opcCount;
        private readonly string alias2;
        private readonly string[] alias;
        private readonly CLICMDBase[] bases;

        public int ID => id;
        public int ArgCount => argCount;
        public int OpitionCount => opcCount;
        public override string Alias => alias2;
        public int Count => bases == null ? 0 : bases.Length;

        public CLICMDBase this[int index] => bases[index];
        public CLICMDBase this[string alias] => bases[IndexOf(alias)];

        public CLICommand(int id, string alias, params CLICMDBase[] bases) {
            this.id = id;
            this.alias = (this.alias2 = alias).Split('/');
            foreach (var item in this.bases = bases) {
                if (item is CLICMDArg) ++argCount;
                else if (item is CLIOption) ++opcCount;
            }
        }

        public CLICommand(int id, string alias) : this(id, alias, Array.Empty<CLICMDBase>()) { }

        public CLICommand(string alias, params CLICMDBase[] bases) : this(0, alias, bases) { }

        public CLICommand(string alias) : this(0, alias, Array.Empty<CLICMDBase>()) { }

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

        public bool Contains(string alias)
            => IndexOf(alias) >= 0;

        public int IndexOf(string alias) {
            for (int I = 0; I < alias.Length; I++)
                if (bases[I].Equals(alias))
                    return I;
            return -1;
        }

        public static bool Cateter(StringArrayToIEnumerator args, CLICommand root, CLIArgCollection collection, ErrorMensager error, out int funcID) {
            int opcCount = 0;
            int opcCursor = 0;
            int opcNoOptCount = 0;
            int opcNoOptCursor = 0;
            while (args.MoveNext()) {
                for (int I = 0; I < root.Count; I++) {
                    if (root[I].Equals(args.Current)) {
                        if (root[I].GetType() == typeof(CLICommand)) {
                            if (opcCount != opcCursor) {
                                error.Add("cmd fwd opc");
                                funcID = 0;
                                return false;
                            }
                            root = root[I] as CLICommand;
                            opcCount = 0;
                            opcCursor = 0;
                            opcNoOptCount = 0;
                            opcNoOptCursor = 0;
                            if (root.OpitionCount != 0) {
                                int ind = args.Index;
                                for (int J = 0; J < root.Count; J++)
                                    if (root[J] is CLIOption cliopc)
                                        if (!cliopc.IsOptional)
                                            ++opcNoOptCount;
                                while (args.MoveNext()) {
                                    bool @break = false;
                                    for (int J = 0; J < root.Count; J++)
                                        if (root[J].Equals(args.Current))
                                            if (root[J].GetType() == typeof(CLICommand)) {
                                                @break = true;
                                                break;
                                            } else if (root[J] is CLIOption cliopc) {
                                                ++opcCount;
                                                if (!cliopc.IsOptional)
                                                    ++opcNoOptCursor;
                                            }
                                    if (@break)
                                        break;
                                }
                                args.Index = ind;
                                if (opcNoOptCount != opcNoOptCursor) {
                                    error.Add("no opc compusi");
                                    funcID = 0;
                                    return false;
                                }
                            }
                            break;
                        } else if (root[I].GetType() == typeof(CLIOption)) {
                            ++opcCursor;
                            for (int J = 0; J < (root[I] as CLIOption).ArgCount; J++) {
                                _ = args.MoveNext();
                                collection.Add(root[I].Alias, args.Current);
                            }
                            break;
                        }
                    } else if (root[I].Equals(CLICMDArg.alias)) {
                        if (opcCount != opcCursor) {
                            error.Add("arg fwd opc");
                            funcID = 0;
                            return false;
                        }
                        for (int J = 0; J < root.ArgCount; J++) {
                            _ = args.MoveNext();
                            collection.Add(root.Alias, args.Current);
                        }
                        break;
                    }
                }
            }
            funcID = root.id;
            return true;
        }

        public static bool operator ==(CLICommand A, string B) => (object)A != (object)null && A.Equals(B);
        public static bool operator !=(CLICommand A, string B) => !(A == B);

        public static bool operator ==(string A, CLICommand B) => (object)B != (object)null && B.Equals(A);
        public static bool operator !=(string A, CLICommand B) => !(A == B);
    }
}
