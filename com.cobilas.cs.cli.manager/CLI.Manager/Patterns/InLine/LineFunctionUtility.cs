using System;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.Manager.Patterns.InLine; 
internal static class LineFunctionUtility {
	internal readonly static AnalyzerFunc Analyzer =
		(alias, list, options, message) => {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			ExceptionMessages.ThrowIfNull(options, nameof(options));

			if (alias != (CLIKey)list.CurrentKey)
				return false;
			
			list.Move();
			for (int I = 0; I < options.Count; I++) {
				IOptionFunc item = options[I];
				if (LineFunction.HasElement(item, list.Current)) {
					if (item is ILineJumpOption ljo2)
						if (ljo2.JumpToEnd) {
							I = options.Count;
							continue;
						}
					if (item is ICLIAnalyzer alz) {
						if (alz.Analyzer(list, message))
							return true;
					} else list.Move();
				} else {
					if (item.Mandatory) {
						item.ExceptionMessage(list, list.Current, message);
						return true;
					} else if (item is ILineJumpOption ljo) {
						if (!ljo.JumpToEnd) {
							I += ljo.JumpUp;
							continue;
						}
					}
				} 
			}

			if (list.CurrentValue != CLIParse.EndCode) {
				CLIParse.GetFunction<Action<CLIKey, KeyValuePair<string, long>, ErrorMessage?>>(1)?
					.Invoke(alias, list.Current, message);
				return true;
			}

			return false;
		};

	internal readonly static GetValuesFunc GetValues =
		(alias, list, valueOrder, options, message) => {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			ExceptionMessages.ThrowIfNull(options, nameof(options));
			ExceptionMessages.ThrowIfNull(valueOrder, nameof(valueOrder));

			if (alias != (CLIKey)list.CurrentKey)
				return false;

			list.Move();
			for (int I = 0; I < options.Count; I++) {
				IOptionFunc item = options[I];
				if (LineFunction.HasElement(item, list.Current)) {
					if (item is ILineJumpOption ljo2)
						if (ljo2.JumpToEnd) {
							I = options.Count;
							item.TreatedValue(valueOrder, list, message);
							continue;
						}
					if (item is IFunction ifc) {
						if (ifc.GetValues(list, message))
							return true;
						valueOrder.AddRange(ifc.ValueOrder);
					} else {
						item.TreatedValue(valueOrder, list, message);
						list.Move();
					}
				} else {
					if (item is ILineJumpOption ljo) {
						item.DefaultValue(valueOrder, message);
						if (!ljo.JumpToEnd) {
							I += ljo.JumpUp;
							continue;
						}
					} else if (!item.Mandatory) {
						item.DefaultValue(valueOrder, message);
						return false;
					} else {
						item.ExceptionMessage(list, list.Current, message);
						return true;
					}
				} 
			}
			return false;
		};
}
