using Bulwark.Admin.Repositories;
using TestFixtures;

namespace Bulwark.Auth.Admin.Core.Tests;

public class RoleManagementServiceTests : IClassFixture<MongoDbRandomFixture>
{
    private IRoleManagement _roleManagement;
    
    public RoleManagementServiceTests(MongoDbRandomFixture dbFixture)
    {
        var roleRepository = new MongoDbRole(dbFixture.Db);
        _roleManagement = new RoleManagementService(roleRepository);
    }
    
    [Fact]
    public async Task CreatReadAllAndDelete()                     
    {
        await _roleManagement.Create("test");
        await _roleManagement.Create("test2");
        var roles = await _roleManagement.ReadAll();
        Assert.True(roles.Count >= 2);
        var role = await _roleManagement.ReadByName("test2");
        Assert.NotNull(role);
        await _roleManagement.Delete(role.Name);
        try{
            role = await _roleManagement.ReadByName("test2");
            Assert.True(false);
        }
        catch(Exception e)
        {
            Assert.NotNull(e);
        }
    }
}