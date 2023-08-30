using Bulwark.Admin.Repositories.Models;

namespace Bulwark.Auth.Admin.Core.Domain;

public class AccountDetails
{
    public string Id { get; set; }
    public string Email { get; set; }
    public bool IsVerified { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsDeleted { get; set; }
    public List<SocialProvider>? SocialProviders { get; set; }
    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
    public List<AuthTokenModel> AuthTokens { get; set; }
    public List<MagicCodeModel> MagicCodes { get; set; }
    
    public DateTime Created { get; set; } 
    public DateTime Modified { get; set; }
    
    public AccountDetails()
    {
        Id = string.Empty;
        Email = string.Empty;
        Roles = new List<string>();
        Permissions = new List<string>();
        AuthTokens = new List<AuthTokenModel>();
        MagicCodes = new List<MagicCodeModel>();
    }
}

