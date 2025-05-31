namespace Presentation.Models;

public class LogInResponse
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
}