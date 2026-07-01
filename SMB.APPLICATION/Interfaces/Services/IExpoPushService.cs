namespace SMB.APPLICATION.Interfaces.Services;

public interface IExpoPushService
{
    /// <summary>Sends a push to each token and returns the subset that came back permanently invalid (app uninstalled / token revoked) so callers can prune them.</summary>
    Task<List<string>> SendAsync(List<string> expoPushTokens, string title, string body);
}
