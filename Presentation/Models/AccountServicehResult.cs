namespace Presentation.Models;

public class AccountServicehResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
}
public class AccountServiceResult<T> : AccountServicehResult
{
    public T? Result { get; set; }
}