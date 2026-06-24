namespace SMB.APPLICATION.DTOs.Auth;

public class ExternalLoginRequest
{
    public string Provider { get; set; } = null!;
    public string IdToken { get; set; } = null!;
    public string? DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public string CurrencyCode { get; set; } = "NIO";
    public string LanguageCode { get; set; } = "es";
}