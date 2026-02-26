using System;
using System.Linq;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager;

public class TokenList : IDisposable {
	private int index;
	private bool disposedValue;
	private KeyValuePair<string, long>[] list;

	public int Count => list.Length;
	public int CurrentIndex => index;
	public string CurrentKey => list[index].Key;
	public long CurrentValue => list[index].Value;
	public KeyValuePair<string, long> Current => list[index];
	public KeyValuePair<string, long> GetValueAndMove {
		get {
			KeyValuePair<string, long> value = list[index];
			Move();
			return value;
		}
	}

	~TokenList() => Dispose(disposing: false);

	public TokenList(IEnumerable<KeyValuePair<string, long>> list) {
		int l_index = 0;
		this.list = new KeyValuePair<string, long>[list.Count()];
		foreach (KeyValuePair<string, long> item in list)
			this.list[l_index++] = item;
		index = -1;
	}

	public void Move(int count) => index += count;

	public void Move() => Move(1);

	public void Reset() => index = -1;

	public void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing) {
		if (!disposedValue) {
			if (disposing) {
				Array.Clear(list, 0, Count);
				list = null;
			}
			disposedValue = true;
		}
	}
}