using System.Collections.Generic;

namespace Cobilas.CLI.Manager.Interfaces;

public interface IOption : IOptionFunc {
	List<IArgument> Options { get; }
}