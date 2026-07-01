using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using SMB.APPLICATION.DTOs.ExpoPush;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.INFRASTRUCTURE.Services;

public class ExpoPushService(IHttpClientFactory httpClientFactory, ILogger<ExpoPushService> logger) : IExpoPushService
{
    private const string PushEndpoint = "https://exp.host/--/api/v2/push/send";

    public async Task<List<string>> SendAsync(List<string> expoPushTokens, string title, string body)
    {
        if (expoPushTokens.Count == 0)
        {
            return [];
        }

        var messages = expoPushTokens.Select(token => new
        {
            to = token,
            title,
            body,
            sound = "default"
        });

        var client = httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(PushEndpoint, messages);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Expo push request failed with status {StatusCode}", response.StatusCode);
            return [];
        }

        var payload = await response.Content.ReadFromJsonAsync<ExpoPushResponse>();
        var tickets = payload?.Data ?? [];
        var invalidTokens = new List<string>();
        
        for (var i = 0; i < tickets.Count && i < expoPushTokens.Count; i++)
        {
            var ticket = tickets[i];

            if (ticket.Status == "ok")
            {
                continue;
            }

            logger.LogWarning("Expo push ticket error: {Message}", ticket.Message);

            if (ticket.Details?.Error == "DeviceNotRegistered")
            {
                invalidTokens.Add(expoPushTokens[i]);
            }
        }

        return invalidTokens;
    }
}
