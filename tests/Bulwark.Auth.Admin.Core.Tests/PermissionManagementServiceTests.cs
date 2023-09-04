using Bulwark.Admin.Repositories;
using TestFixtures;

namespace Bulwark.Auth.Admin.Core.Tests;

public class PermissionManagementServiceTests : IClassFixture<MongoDbRandomFixture>
{
    private readonly IPermissionRepository _permissionRepository;
    
    public PermissionManagementServiceTests(MongoDbRandomFixture dbFixture)
    {
        _permissionRepository = new MongoDbPermission(dbFixture.Db);
    }
    
    [Fact]
    public async Task CreatReadAllAndDelete()                     
    {
        await _permissionRepository.Create("test");
        await _permissionRepository.Create("test2");
        var permissions = await _permissionRepository.Read();
        Assert.True(permissions.Count >= 2);
        var permission = permissions.FirstOrDefault();
        await _permissionRepository.Delete(permission.Id);
        permissions = await _permissionRepository.Read();
        Assert.True(permissions.Count == 1);
    }
}