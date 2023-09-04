using Bulwark.Admin.Repositories;
using TestFixtures;

namespace Bulwark.Auth.Admin.Core.Tests;

public class SigningKeyManagementServiceTests : IClassFixture<MongoDbRandomFixture>
{
    private ISigningKeyManagement _signingKeyManagement;
    
    public SigningKeyManagementServiceTests(MongoDbRandomFixture dbFixture)
    {
        var signingKeyRepository = new MongoDbSigningKey(dbFixture.Db);
        _signingKeyManagement = new SigningKeyManagementService(signingKeyRepository);
    }
    
    [Fact]
    public void CreatReadAllAndDelete()                     
    {
        _signingKeyManagement.GenerateKey();
        _signingKeyManagement.GenerateKey();
        var signingKeys = _signingKeyManagement.ReadAll();
        Assert.True(signingKeys.Count >= 2);
        var latest = _signingKeyManagement.Latest();
        Assert.NotNull(latest);
        var latestKey = signingKeys.AsQueryable()
            .OrderByDescending(c => c.Created)
            .FirstOrDefault();
        Assert.Equal(latest.KeyId, latestKey.KeyId);
        _signingKeyManagement.Delete(latest.KeyId);
        var signingKeysAfterDelete = _signingKeyManagement.Read(latestKey.KeyId);
        
        if (signingKeysAfterDelete != null)
        {
            Assert.True(false);
        }
    }
}