using System.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager.Collections;

public class TokenListReadOnly : IReadOnlyList<KeyValuePair<string, long>> {

	private KeyValuePair<string, long>[] list;

	public int Count => list.Length;

	public KeyValuePair<string, long> this[int index] => list[index];

	public KeyValuePair<string, long> this[string token] => list[IndexOf(token)];


	public TokenListReadOnly(KeyValuePair<string, long>[]? list)
	{
		this.list = list ?? [];
	}

	public TokenListReadOnly(IEnumerable<KeyValuePair<string, long>>? collection) {
		if (collection is null) {
			list = [];
			return;
		}
		list = [.. collection];
	}

	public int IndexOf(string token) {
		for (int I = 0; I < Count; I++)
			if (list[I].Key == token)
				return I;
		return -1;
	}

	public bool TryGetValue(string token, out long id) {
		int index = IndexOf(token);
		id = 0;
		if (index < 0) return false;
		KeyValuePair<string, long> temp = this[index];
		id = temp.Value;
		return true;
	}

	public IEnumerator<KeyValuePair<string, long>> GetEnumerator() {
		foreach (KeyValuePair<string, long> item in list)
			yield return item;
	}

	IEnumerator IEnumerable.GetEnumerator() {
		foreach (KeyValuePair<string, long> item in list)
			yield return item;
	}
}
