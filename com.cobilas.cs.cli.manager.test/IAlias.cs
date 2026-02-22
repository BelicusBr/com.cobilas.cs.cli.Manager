interface IAlias {
	string Alias { get; }
	long TypeCode { get; }
	bool IsAlias(string alias);
}
