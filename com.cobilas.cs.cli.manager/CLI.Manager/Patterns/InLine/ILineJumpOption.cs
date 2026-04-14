/// <summary>
/// Represents an interface for line jump options that define navigation behavior in command-line processing.
/// </summary>
public interface ILineJumpOption {
	/// <summary>
	/// Gets the number of positions to jump up in the processing sequence.
	/// </summary>
	/// <returns>The number of positions to move upwards.</returns>
	public int JumpUp { get; }
	/// <summary>
	/// Gets a value indicating whether to jump to the end of the processing sequence.
	/// </summary>
	/// <returns><see langword="true"/> if jumping to the end; otherwise, <see langword="false"/>.</returns>
	public bool JumpToEnd { get; }
}
