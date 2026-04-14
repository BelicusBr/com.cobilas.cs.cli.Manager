using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine; 
internal static class LineFunctionUtility {
	internal readonly static Func<CLIKey, TokenList?, List<IOptionFunc>?, ErrorMessage?, bool> Analyzer =
		(alias, list, options, message) => {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			ExceptionMessages.ThrowIfNull(options, nameof(options));

			for (int I = 0; I < options.Count; I++) {
				IOptionFunc item = options[I];
				if (item.TypeCode != list.CurrentValue) {
					if (item.Mandatory) {
						item.ExceptionMessage(list.Current, message);
						return true;
					} else if (item is ILineJumpOption ljo) {
						if (!ljo.JumpToEnd) {
							I += ljo.JumpUp;
							continue;
						}
					}
				} else {
					if (item is ILineJumpOption ljo2)
						if (ljo2.JumpToEnd) {
							I = options.Count;
							continue;
						}
					if (item is ICLIAnalyzer alz) {
						list.Move();
						if (alz.Analyzer(list, message))
							return true;
					}
					list.Move();
				}
			}
			return false;
		};

	internal readonly static Func<CLIKey, TokenList?, CLIValueOrder, List<IOptionFunc>?, ErrorMessage?, bool> GetValues =
		(alias, list, valueOrder, options, message) => {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			ExceptionMessages.ThrowIfNull(options, nameof(options));

			for (int I = 0; I < options.Count; I++) {
				IOptionFunc item = options[I];
				if (item.TypeCode != list.CurrentValue) {
					if (!item.Mandatory) {
						item.DefaultValue(valueOrder, message);
						return true;
					} else if (item is ILineJumpOption ljo) {
						if (!ljo.JumpToEnd) {
							I += ljo.JumpUp;
							continue;
						}
					}
				} else {
					if (item is ILineJumpOption ljo2)
						if (ljo2.JumpToEnd) {
							I = options.Count;
							continue;
						}
					if (item is IFunction ifc) {
						list.Move();
						if (ifc.GetValues(list, message))
							return true;
					}
					list.Move();
				}
			}
			return false;
		};
}
