namespace Bulwark.Auth.Admin.Repository.Tests;

public class AccountRepositoryTests : IClassFixture<MongoDbRandomFixture>
{
    private readonly IAccountRepository _accountRepository;
    public AccountRepositoryTests(MongoDbRandomFixture dbFixture)
    {
        _accountRepository = new MongoDbAccount(dbFixture.Db);
    }

    [Fact]
    public async void CreateReadUpdateDelete()
    {
        var accountModel = GetAccountModel();
        var email = accountModel.Email;
        await _accountRepository.Create(accountModel);
        accountModel = await _accountRepository.ReadByEmail(accountModel.Email);
        Assert.True(accountModel.Email == email);
        accountModel.IsEnabled = true;
        await _accountRepository.Update(accountModel);
        accountModel = await _accountRepository.ReadByEmail(accountModel.Email);
        Assert.True(accountModel.IsEnabled);
        await _accountRepository.Delete(accountModel.Email);

        try{
            accountModel = await _accountRepository.ReadByEmail(accountModel.Email);
        }
        catch (Exception e)
        {
            Assert.Contains(accountModel.Email, e.Message);
        }
    }

    private AccountModel GetAccountModel()
    {
        var accountModel = new AccountModel()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Email = GenerateEmail(),
            IsVerified = false,
            IsEnabled = false,
            IsDeleted = false
        };
        return accountModel;
    }
 
    private string GenerateEmail()
    {
        return Guid.NewGuid().ToString() + "@lateflip.io";
    }
}
