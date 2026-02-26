using System;
using System.Linq;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Represents a list of token key-value pairs with a current position cursor.
/// Implements <see cref="IDisposable"/> to clear the internal array.
/// </summary>
public class TokenList : IDisposable {
	private int index;
	private bool disposedValue;
	private KeyValuePair<string, long>[]? list;
	/// <summary>
	/// Gets the current index position within the token list.
	/// </summary>
	/// <returns>The zero‑based current index.</returns>
	public int CurrentIndex => index;
	/// <summary>
	/// Gets the total number of tokens in the list.
	/// </summary>
	/// <returns>The count of tokens.</returns>
	/// <exception cref="NullReferenceException">Thrown if the internal list is null.</exception>
	public int Count {
		get {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			return list.Length;
		}
	}
	/// <summary>
	/// Gets the key (token string) at the current position.
	/// </summary>
	/// <returns>The key at <see cref="CurrentIndex"/>.</returns>
	/// <exception cref="NullReferenceException">Thrown if the internal list is null.</exception>
	public string CurrentKey {
		get {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			return list[index].Key;
		}
	}
	/// <summary>
	/// Gets the value (token ID) at the current position.
	/// </summary>
	/// <returns>The value at <see cref="CurrentIndex"/>.</returns>
	/// <exception cref="NullReferenceException">Thrown if the internal list is null.</exception>
	public long CurrentValue {
		get {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			return list[index].Value;
		}
	}
	/// <summary>
	/// Gets the entire key-value pair at the current position.
	/// </summary>
	/// <returns>The pair at <see cref="CurrentIndex"/>.</returns>
	/// <exception cref="NullReferenceException">Thrown if the internal list is null.</exception>
	public KeyValuePair<string, long> Current {
		get {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			return list[index];
		}
	}
	/// <summary>
	/// Gets the key-value pair at the current position and then advances the cursor by one.
	/// </summary>
	/// <returns>The pair at the current position before moving.</returns>
	/// <exception cref="NullReferenceException">Thrown if the internal list is null.</exception>
	public KeyValuePair<string, long> GetValueAndMove {
		get {
			ExceptionMessages.ThrowIfNull(list, nameof(list));
			KeyValuePair<string, long> value = list[index];
			Move();
			return value;
		}
	}
	/// <summary>
	/// Finalizes an instance of the <see cref="TokenList"/> class.
	/// </summary>
	~TokenList() => Dispose(disposing: false);
	/// <summary>
	/// Initializes a new instance of the <see cref="TokenList"/> class from an enumerable collection of token pairs.
	/// </summary>
	/// <param name="list">The collection of token key-value pairs. Cannot be null.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is null.</exception>
	public TokenList(IEnumerable<KeyValuePair<string, long>>? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		int l_index = 0;
		this.list = new KeyValuePair<string, long>[list.Count()];
		foreach (KeyValuePair<string, long> item in list)
			this.list[l_index++] = item;
		index = -1;
	}
	/// <summary>
	/// Moves the current cursor forward by the specified number of positions.
	/// </summary>
	/// <param name="count">The number of positions to move.</param>
	public void Move(int count) => index += count;
	/// <summary>
	/// Moves the current cursor forward by one position.
	/// </summary>
	public void Move() => Move(1);
	/// <summary>
	/// Resets the cursor to the beginning (position -1).
	/// </summary>
	public void Reset() => index = -1;
	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
	/// </summary>
	public void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	/// <summary>
	/// Releases the unmanaged resources used by the <see cref="TokenList"/> and optionally releases the managed resources.
	/// </summary>
	/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool disposing) {
		if (!disposedValue) {
			if (disposing) {
				if (list is not null)
					Array.Clear(list, 0, Count);
				list = null;
			}
			disposedValue = true;
		}
	}
}