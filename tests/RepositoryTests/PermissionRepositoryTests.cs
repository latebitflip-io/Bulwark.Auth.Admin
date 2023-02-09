namespace RepositoryTests;

public class PermissionRepositoryTests : IClassFixture<MongoDbRandomFixture>
{
    private readonly MongoDbPermission _permissionRepository;
    
    public PermissionRepositoryTests(MongoDbRandomFixture dbFixture)
    {
        _permissionRepository =
            new MongoDbPermission(dbFixture.Db);
    }
    
    [Fact]
    public async Task CreateDeletePermission()
    {
        var permission = "write:bulwark-test";
        await _permissionRepository.Create(permission);
        var result = await _permissionRepository.Read();
        Assert.True(result.Exists(x => x.Id == permission));
        await _permissionRepository.Delete(permission);
        result = await _permissionRepository.Read();
        Assert.False(result.Exists(x => x.Id == permission));
    }
}