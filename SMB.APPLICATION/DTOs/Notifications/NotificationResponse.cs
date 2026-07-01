namespace SMB.APPLICATION.DTOs.Notifications;

public class NotificationResponse
{
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
