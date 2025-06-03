using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace Presentation.Services;

public class AuthServiceSender 
{
    private readonly ServiceBusSender _sender;

    public AuthServiceSender(ServiceBusClient client)
    {
        _sender = client.CreateSender("email-service");
    }
    public async Task SendMessageAsync<T>(T message, string type, string? token = null)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(jsonMessage)
        {
            Subject = type
        };
        if (!string.IsNullOrWhiteSpace(token))
        {
            serviceBusMessage.ApplicationProperties["ServiceBus"] = token;
        }
        await _sender.SendMessageAsync(serviceBusMessage);
    }
}
