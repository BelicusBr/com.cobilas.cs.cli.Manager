using System;
using System.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager;

public class CLIValueOrder(int capacity) : IDictionary<CLIKey, string> {

	private int _size;
	private KeyValuePair<CLIKey, string>[] valueOrder = new KeyValuePair<CLIKey, string>[capacity];

	public bool IsReadOnly => false;
	public int Count => _size;
	public int Capacity
	{
		get => valueOrder.Length;
		set
		{
			if (value < 0) throw new IndexOutOfRangeException();
			Array.Resize(ref valueOrder, value);
		}
	}

	ICollection<CLIKey> IDictionary<CLIKey, string>.Keys => Array.ConvertAll(valueOrder, (l) => l.Key);
	ICollection<string> IDictionary<CLIKey, string>.Values => Array.ConvertAll(valueOrder, (l) => l.Value);

	public string this[CLIKey key]
	{
		get => valueOrder[KeyIndex(key)].Value;
		set
		{
			int index = KeyIndex(key);
			valueOrder[index] = new(valueOrder[index].Key, value);
		}
	}

	public CLIValueOrder() : this(0) { }

	public void Clear() => Array.Clear(valueOrder);

	public void CopyTo(KeyValuePair<CLIKey, string>[] array, int arrayIndex)
		=> Array.Copy(valueOrder, 0, array, arrayIndex, Count);

	public void Add(KeyValuePair<CLIKey, string> item)
	{
		if (ContainsKey(item.Key))
			throw new InvalidOperationException();
		++_size;
		if (_size > Capacity)
			Array.Resize(ref valueOrder, _size);
		valueOrder[_size - 1] = item;
	}

	public void Add(CLIKey key, string value) => Add(new(key, value));

	public bool Contains(KeyValuePair<CLIKey, string> item) => IndexOf(item) >= 0;

	public int IndexOf(KeyValuePair<CLIKey, string> item)
	{
		for (int I = 0; I < _size; I++)
			if (valueOrder[I].Key == item.Key && valueOrder[I].Value == item.Value)
				return I;
		return -1;
	}

	public bool ContainsKey(CLIKey key) => KeyIndex(key) >= 0;

	public int KeyIndex(CLIKey key)
	{
		for (int I = 0; I < _size; I++)
			if (valueOrder[I].Key == key)
				return I;
		return -1;
	}

	public bool Remove(CLIKey key)
	{
		int index = KeyIndex(key);
		if (index >= 0)
		{
			--_size;
			for (int I = index; I < Capacity; I++)
			{
				if (_size - 1 > I)
				{
					valueOrder[I] = default;
					continue;
				}
				valueOrder[I] = valueOrder[I + 1];
			}
			return true;
		}
		return false;
	}

	public bool Remove(KeyValuePair<CLIKey, string> item) => Remove(item.Key);

	public bool TryGetValue(CLIKey key, out string value)
	{
		int index = KeyIndex(key);
		if (index >= 0)
		{
			value = valueOrder[index].Value;
			return true;
		}
		value = string.Empty;
		return false;
	}

	public IEnumerator<KeyValuePair<CLIKey, string>> GetEnumerator()
	{
		foreach (KeyValuePair<CLIKey, string> item in valueOrder)
			yield return item;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		foreach (KeyValuePair<CLIKey, string> item in valueOrder)
			yield return item;
	}
}