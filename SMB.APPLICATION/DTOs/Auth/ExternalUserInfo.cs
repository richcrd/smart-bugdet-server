namespace SMB.APPLICATION.DTOs.Auth;

public class ExternalUserInfo
{
    public string ProviderUserId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? DisplayName { get; set; }
}