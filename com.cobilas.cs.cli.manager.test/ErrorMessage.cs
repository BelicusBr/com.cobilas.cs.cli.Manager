using System;

class ErrorMessage {
	public int ErroCode { get; set; }
	public string Message { get; set; }
	public Guid Id { get; private set; }

	public static ErrorMessage Default => new();

	public ErrorMessage() {
		ErroCode = -1;
		Message = nameof(string.Empty);
		Id = Guid.NewGuid();
	}

	public override int GetHashCode() => Id.GetHashCode() >> 1 ^ ErroCode;

	public override string ToString()
		=> $"{nameof(ErroCode)}: {ErroCode}\r\n{nameof(Id)}: {Id}\r\n{nameof(Message)}:{{\r\n{Message}\r\n}}";
}