namespace SMB.APPLICATION.DTOs.Auth;

public class LoginRequest
{
    public string EmailOrPhone { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public string? DeviceId { get; set; }
    public string? DeviceName { get; set; }
}