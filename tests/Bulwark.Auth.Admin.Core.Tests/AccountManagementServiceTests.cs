using Bulwark.Admin.Repositories;
using Bulwark.Auth.Admin.Core.Tests.Utils;
using TestFixtures;

namespace Bulwark.Auth.Admin.Core.Tests;

public class AccountManagementServiceTests : IClassFixture<MongoDbRandomFixture>
{
    private AccountManagementService _accountManagementService;
    
    public AccountManagementServiceTests(MongoDbRandomFixture dbFixture)
    {
        var accountRepository = new MongoDbAccount(dbFixture.Db);
        var authTokenRepository = new MongoDbAuthToken(dbFixture.Db);
        var magicCodeRepository = new MongoDbMagicCode(dbFixture.Db);
        var roleRepository = new MongoDbRole(dbFixture.Db);
        var permissionRepository = new MongoDbPermission(dbFixture.Db);
        _accountManagementService = new AccountManagementService(accountRepository,
            authTokenRepository, magicCodeRepository, roleRepository, permissionRepository);
    }
    
    [Fact]
    public async Task CreateAndReadAll()                     
    {
        var email1 = TestUtils.GenerateEmail();
        await _accountManagementService.Create(email1, true);
        var email2 =  TestUtils.GenerateEmail();
        await _accountManagementService.Create(email2, true);
        var accounts = _accountManagementService.ReadAll("email", 1, 10);
        Assert.True(accounts.Count >= 2);
    }
    
    [Fact]
    public async Task ReadByEmail()
    {
        var email = TestUtils.GenerateEmail();
        await _accountManagementService.Create(email, true);
        var accountDetails = await _accountManagementService.ReadByEmail(email);
        Assert.True(accountDetails.Email == email);
    }

    [Fact]
    public async Task ReadById()
    {
        var email = TestUtils.GenerateEmail();
        await _accountManagementService.Create(email, true);
        var accountDetails = await _accountManagementService.ReadByEmail(email);
        var accountDetailsById = await _accountManagementService.ReadById(accountDetails.Id);
        Assert.True(accountDetailsById.Email == email);
    }

    [Fact]
    public async Task UpdateEmail()
    {
        var email = TestUtils.GenerateEmail();
        var newEmail = TestUtils.GenerateEmail();
        await _accountManagementService.Create(email, true);
        await _accountManagementService.UpdateEmail(email, newEmail, true);
        var accountDetails = await _accountManagementService.ReadByEmail(newEmail);
        Assert.True(accountDetails.Email == newEmail);
    }
    
    [Fact]
    public async Task UpdateEnabledDisabled()
    {
        var email = TestUtils.GenerateEmail();
        await _accountManagementService.Create(email, false);
        var accountDetails = await _accountManagementService.ReadByEmail(email);
        Assert.True(!accountDetails.IsEnabled);
        await _accountManagementService.Enable(email);
        accountDetails = await _accountManagementService.ReadByEmail(email);
        Assert.True(accountDetails.IsEnabled);
        await _accountManagementService.Disable(email);
        accountDetails = await _accountManagementService.ReadByEmail(email);
        Assert.True(!accountDetails.IsEnabled);
    }
    
    [Fact]
    public async Task SoftDelete()
    {
        var email = TestUtils.GenerateEmail();
        await _accountManagementService.Create(email, true);
        var accountDetails = await _accountManagementService.ReadByEmail(email);
        Assert.True(!accountDetails.IsDeleted);
        await _accountManagementService.SoftDelete(email);
        accountDetails = await _accountManagementService.ReadByEmail(email);
        Assert.True(accountDetails.IsDeleted);
    }
    
    [Fact]
    public async Task HardDelete()
    {
        var email = TestUtils.GenerateEmail();
        await _accountManagementService.Create(email, true);
        var accountDetails = await _accountManagementService.ReadByEmail(email);
        Assert.True(!accountDetails.IsDeleted);
        await _accountManagementService.HardDelete(email);
        try
        {
            accountDetails = await _accountManagementService.ReadByEmail(email);
        }
        catch (Exception e)
        {
            Assert.Contains(email, e.Message);
        }
    }
}