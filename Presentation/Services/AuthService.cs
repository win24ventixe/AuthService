using Azure;
using Azure.Messaging.ServiceBus;
using Presentation.Models;
using Presentation.Providers;
using System.Text.Json;

namespace Presentation.Services;

public interface IAuthService
{
    Task<AuthResult> AlreadyExists(string email);
    Task<AuthResult<LogInResponse>> LoginAsync(LogInRequest request);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(SignUpRequest request);
}

/* Rewrite using ChatGPT */
public class AuthService(AuthServiceSender sender, ITokenProvider tokenProvider, ServiceBusReceiver receiver) : IAuthService
{
    private readonly AuthServiceSender _sender = sender;
    private readonly ServiceBusReceiver _receiver = receiver;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<AuthResult> SignUpAsync(SignUpRequest request)
    {
        var token = await _tokenProvider.GetTokenAsync();

        var message = new AuthServiceMessage
        {
            Action = "signup",
            Payload = request,
            Token = token
        };

        await _sender.SendMessageAsync(message, "signup", token);

        var receivedMessage = await _receiver.ReceiveMessageAsync();
        if (receivedMessage != null)
        {
            var response = JsonSerializer.Deserialize<AuthResult>(receivedMessage.Body.ToString());
            return response ?? new AuthResult { Success = false, Error = "No response received" };
        }

        return new AuthResult { Success = false, Error = "No response received" };
    }

    public async Task<AuthResult<LogInResponse>> LoginAsync(LogInRequest request)
    {
        var token = await _tokenProvider.GetTokenAsync();

        var message = new AuthServiceMessage
        {
            Action = "login",
            Payload = request,
            Token = token
        };

        await _sender.SendMessageAsync(message, "login", token);

        var receivedMessage = await _receiver.ReceiveMessageAsync();
        if (receivedMessage != null)
        {
            var response = JsonSerializer.Deserialize<AuthResult<LogInResponse>>(receivedMessage.Body.ToString());
            return response ?? new AuthResult<LogInResponse> { Success = false, Error = "No response received" };
        }

        return new AuthResult<LogInResponse> { Success = false, Error = "No response received" };
    }

    public async Task<AuthResult> SignOutAsync()
    {
        var token = await _tokenProvider.GetTokenAsync();

        var message = new AuthServiceMessage
        {
            Action = "signout",
            Payload = null!,
            Token = token
        };

        await _sender.SendMessageAsync(message, "signout", token);

        var receivedMessage = await _receiver.ReceiveMessageAsync();
        if (receivedMessage != null)
        {
            var response = JsonSerializer.Deserialize<AuthResult>(receivedMessage.Body.ToString());
            return response ?? new AuthResult { Success = false, Error = "No response received" };
        }

        return new AuthResult { Success = false, Error = "No response received" };
    }

    public async Task<AuthResult> AlreadyExists(string email)
    {
        var token = await _tokenProvider.GetTokenAsync();

        var message = new AuthServiceMessage
        {
            Action = "exists",
            Payload = email,
            Token = token
        };

        await _sender.SendMessageAsync(message, "exists", token);

        var receivedMessage = await _receiver.ReceiveMessageAsync();
        if (receivedMessage != null)
        {
            var response = JsonSerializer.Deserialize<AuthResult>(receivedMessage.Body.ToString());
            return response ?? new AuthResult { Success = false, Error = "No response received" };
        }

        return new AuthResult { Success = false, Error = "No response received" };
    }
}
