using System.Collections.Generic;

interface IOption : IOptionFunc {
	List<IArgument> Options { get; }
}
