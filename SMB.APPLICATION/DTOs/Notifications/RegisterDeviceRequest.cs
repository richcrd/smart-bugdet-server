namespace SMB.APPLICATION.DTOs.Notifications;

public class RegisterDeviceRequest
{
    public string ExpoPushToken { get; set; } = null!;
    public string Platform { get; set; } = null!;
}
