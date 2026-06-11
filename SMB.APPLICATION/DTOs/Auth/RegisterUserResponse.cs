namespace SMB.APPLICATION.DTOs.Auth;

public class RegisterUserResponse
{
    public long PeopleId { get; set; }
    public long UserId { get; set; }
    public long WalletId { get; set; }

    public string FullName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}