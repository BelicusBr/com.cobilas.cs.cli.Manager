using Cobilas.CLI.Manager.Collections;
using Cobilas.CLI.Manager.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobilas.CLI.Manager;

public class TokenExecutor {
	private readonly InfoTokenData data;

	public Action<InfoTokenData>? InvokFunction { get; set; } = null;

	public TokenExecutor()
	{
		data = new InfoTokenData();
	}

	public void AddTokenInfo(string? token, int argCount, long tokenID) {
		ExceptionMessages.ThrowIfNullOrEmpty(token);
		InfoTokenData info = new();
		info["count"] = argCount;
		info["next-count"] = 0;
		info["id"] = tokenID;
		data[token] = info;
	}

	public void Invok(TokenList? list, TokenExecutorFunction? func) {
		ExceptionMessages.ThrowIfNull(list);
		ExceptionMessages.ThrowIfNull(func);
		ExceptionMessages.ThrowIfNull(InvokFunction);
		InfoTokenData info = new();

		foreach (KeyValuePair<string, long> item in list)
			func(item, info, data);

		InvokFunction(info);
	}
}
