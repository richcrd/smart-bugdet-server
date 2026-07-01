namespace SMB.APPLICATION.DTOs.ExpoPush;

public class ExpoPushTicket
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public ExpoPushTicketDetails? Details { get; set; }
}