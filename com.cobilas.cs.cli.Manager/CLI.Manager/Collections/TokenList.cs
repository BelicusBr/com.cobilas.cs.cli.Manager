using System;
using System.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.Manager; 
public class TokenList : IEnumerable<KeyValuePair<string, long>>, IDisposable {
	private readonly List<KeyValuePair<string, long>> list;

	public TokenList()
	{
		list = [];
	}

	public void Add(KeyValuePair<string, long> item)
		=> list.Add(item);

	public void Add(string key, long id)
		=> Add(new KeyValuePair<string, long>(key, id));

	public void Dispose()
	{
		list.Clear();
		list.Capacity = 0;
	}

	public IEnumerator<KeyValuePair<string, long>> GetEnumerator()
		=> ((IEnumerable<KeyValuePair<string, long>>)list).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
		=> ((IEnumerable)list).GetEnumerator();
}
