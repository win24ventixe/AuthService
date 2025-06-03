namespace Presentation.Models;

public class AuthServiceMessage
{
    public string Action { get; set; } = null!;
    public object Payload { get; set; } = default!;
    public string Token { get; set; } = null!;
}
