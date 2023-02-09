namespace Bulwark.Admin.Repositories;

public interface IAccountRepository
{
    Task Create(AccountModel accountModel);
    Task<AccountModel> ReadByEmail(string email);
    Task<AccountModel> ReadById(string id);
    List<AccountModel> ReadAll(int page, int size, string sortBy);
    Task Update(AccountModel accountModel);
    Task Delete(string email);
}

