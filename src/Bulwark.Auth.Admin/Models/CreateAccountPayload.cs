namespace Bulwark.Auth.Admin.Models;

public class CreateAccountPayload
{
    public string Email { get; set; }
    public bool IsVerified { get; set; }

    public CreateAccountPayload()
    {
        Email = String.Empty;
        IsVerified = false;
    }
}