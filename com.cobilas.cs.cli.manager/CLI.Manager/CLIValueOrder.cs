using System;
using System.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager;
/// <summary>
/// Represents an ordered collection of key-value pairs where the key is a <see cref="CLIKey"/> and the value is a nullable string.
/// Implements <see cref="IDictionary{CLIKey, String}"/> with array-based storage.
/// </summary>
/// <param name="capacity">The initial capacity of the internal array.</param>
public class CLIValueOrder(int capacity) : IDictionary<CLIKey, string?> {
	private int _size = 0;
	private KeyValuePair<CLIKey, string?>[] valueOrder = new KeyValuePair<CLIKey, string?>[capacity];
	/// <summary>
	/// Gets a value indicating whether the dictionary is read-only.
	/// </summary>
	/// <returns>Always <see langword="false"/> because this implementation is modifiable.</returns>
	public bool IsReadOnly => false;
	/// <summary>
	/// Gets the number of elements contained in the <see cref="CLIValueOrder"/>.
	/// </summary>
	/// <returns>The current count of key-value pairs.</returns>
	public int Count => _size;
	/// <summary>
	/// Gets or sets the total number of elements the internal data structure can hold without resizing.
	/// </summary>
	/// <value>The new capacity. Must be non‑negative.</value>
	/// <returns>The current capacity.</returns>
	/// <exception cref="IndexOutOfRangeException">Thrown when setting a negative value.</exception>
	public int Capacity {
		get => valueOrder.Length;
		set {
			if (value < 0) throw new IndexOutOfRangeException();
			Array.Resize(ref valueOrder, value);
		}
	}
	/// <inheritdoc/>
	ICollection<CLIKey> IDictionary<CLIKey, string?>.Keys => Array.ConvertAll(valueOrder, (l) => l.Key);
	/// <inheritdoc/>
	ICollection<string?> IDictionary<CLIKey, string?>.Values => Array.ConvertAll(valueOrder, (l) => l.Value);
	/// <summary>
	/// Gets or sets the value associated with the specified key.
	/// </summary>
	/// <param name="key">The key of the value to get or set.</param>
	/// <returns>The value associated with the key.</returns>
	/// <exception cref="InvalidOperationException">Thrown when getting a value for a key that does not exist.</exception>
	public string? this[CLIKey key] {
		get => valueOrder[KeyIndex(key)].Value;
		set {
			int index = KeyIndex(key);
			valueOrder[index] = new(valueOrder[index].Key, value);
		}
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CLIValueOrder"/> class with default capacity (0).
	/// </summary>
	public CLIValueOrder() : this(0) { }
	/// <summary>
	/// Removes all items from the <see cref="CLIValueOrder"/>.
	/// </summary>
	public void Clear() => Array.Clear(valueOrder, _size = 0, Capacity);
	/// <summary>
	/// Copies the elements of the <see cref="CLIValueOrder"/> to an array, starting at a particular array index.
	/// </summary>
	/// <param name="array">The destination array.</param>
	/// <param name="arrayIndex">The zero‑based index in <paramref name="array"/> at which copying begins.</param>
	public void CopyTo(KeyValuePair<CLIKey, string?>[] array, int arrayIndex)
		=> Array.Copy(valueOrder, 0, array, arrayIndex, Count);
	/// <summary>
	/// Adds a key-value pair to the collection.
	/// </summary>
	/// <param name="item">The pair to add.</param>
	/// <exception cref="InvalidOperationException">Thrown if a pair with the same key already exists.</exception>
	public void Add(KeyValuePair<CLIKey, string?> item) {
		if (ContainsKey(item.Key))
			throw new InvalidOperationException();
		++_size;
		if (_size > Capacity)
			Array.Resize(ref valueOrder, _size);
		valueOrder[_size - 1] = item;
	}
	/// <summary>
	/// Adds an element with the provided key and value to the collection.
	/// </summary>
	/// <param name="key">The key of the element to add.</param>
	/// <param name="value">The value of the element to add.</param>
	/// <exception cref="InvalidOperationException">Thrown if the key already exists.</exception>
	public void Add(CLIKey key, string? value) => Add(new(key, value));
	/// <summary>
	/// Determines whether the collection contains a specific key-value pair.
	/// </summary>
	/// <param name="item">The pair to locate.</param>
	/// <returns><see langword="true"/> if found; otherwise, <see langword="false"/>.</returns>
	public bool Contains(KeyValuePair<CLIKey, string?> item) => IndexOf(item) >= 0;
	/// <summary>
	/// Returns the index of the specified key-value pair in the internal array.
	/// </summary>
	/// <param name="item">The pair to locate.</param>
	/// <returns>The zero‑based index if found; otherwise, -1.</returns>
	public int IndexOf(KeyValuePair<CLIKey, string?> item) {
		for (int I = 0; I < _size; I++)
			if (valueOrder[I].Key == item.Key && valueOrder[I].Value == item.Value)
				return I;
		return -1;
	}
	/// <summary>
	/// Determines whether the collection contains an element with the specified key.
	/// </summary>
	/// <param name="key">The key to locate.</param>
	/// <returns><see langword="true"/> if the key exists; otherwise, <see langword="false"/>.</returns>
	public bool ContainsKey(CLIKey key) => KeyIndex(key) >= 0;
	/// <summary>
	/// Returns the index of the element with the specified key.
	/// </summary>
	/// <param name="key">The key to locate.</param>
	/// <returns>The zero‑based index if found; otherwise, -1.</returns>
	public int KeyIndex(CLIKey key) {
		for (int I = 0; I < _size; I++)
			if (valueOrder[I].Key == key)
				return I;
		return -1;
	}
	/// <summary>
	/// Removes the element with the specified key from the collection.
	/// </summary>
	/// <param name="key">The key of the element to remove.</param>
	/// <returns><see langword="true"/> if the element was successfully removed; otherwise, <see langword="false"/>.</returns>
	public bool Remove(CLIKey key) {
		int index = KeyIndex(key);
		if (index >= 0) {
			--_size;
			for (int I = index; I < Capacity; I++)
				valueOrder[I] = I > _size - 1 ? default : valueOrder[I + 1];
			return true;
		}
		return false;
	}
	/// <summary>
	/// Removes the first occurrence of a specific key-value pair from the collection.
	/// </summary>
	/// <param name="item">The pair to remove.</param>
	/// <returns><see langword="true"/> if removed; otherwise, <see langword="false"/>.</returns>
	public bool Remove(KeyValuePair<CLIKey, string?> item) => Remove(item.Key);
	/// <summary>
	/// Attempts to get the value associated with the specified key.
	/// </summary>
	/// <param name="key">The key to locate.</param>
	/// <param name="value">When this method returns, contains the value associated with the key, if found; otherwise, an empty string.</param>
	/// <returns><see langword="true"/> if the key exists; otherwise, <see langword="false"/>.</returns>
	public bool TryGetValue(CLIKey key, out string? value) {
		int index = KeyIndex(key);
		if (index >= 0) {
			value = valueOrder[index].Value;
			return true;
		}
		value = string.Empty;
		return false;
	}
	/// <summary>
	/// Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator for the key-value pairs.</returns>
	public IEnumerator<KeyValuePair<CLIKey, string?>> GetEnumerator() {
		for (int I = 0; I < _size; I++)
			yield return valueOrder[I];
	}
	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() {
		for (int I = 0; I < _size; I++)
			yield return valueOrder[I];
	}
}