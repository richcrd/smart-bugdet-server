namespace SMB.API.Contracts;

public class Answer<T>
{
    public required string Message { get; init; }
    public required int Code { get; init; }
    public T? Response { get; init; }
}