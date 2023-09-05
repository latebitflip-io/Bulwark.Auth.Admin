using Bulwark.Admin.Repositories.Models;
using Bulwark.Auth.Admin.Core.Domain;

namespace Bulwark.Auth.Admin.Core;

public interface IAccountManagement
{
    List<AccountModel> ReadAll(string sortField,
        int page, int perPage);
    Task<AccountDetails> ReadDetailsByEmail(string email);
    Task<AccountDetails> ReadDetailsById(string id);
    Task Create(string email, bool isVerified);
    Task UpdateEmail(string email, string newEmail, bool isVerified);
    Task Disable(string email);
    Task Enable(string email);
    Task SoftDelete(string email);
    Task HardDelete(string email);
}