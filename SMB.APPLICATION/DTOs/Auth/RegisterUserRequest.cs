namespace SMB.APPLICATION.DTOs.Auth;

public class RegisterUserRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }

    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public string CurrencyCode { get; set; } = "NIO";
    public string LanguageCode { get; set; } = "es";
}