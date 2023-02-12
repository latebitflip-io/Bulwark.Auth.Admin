namespace Bulwark.Auth.Admin.Repository.Tests;

public class RoleRepositoryTests : IClassFixture<MongoDbRandomFixture>
{
	private readonly MongoDbRole _roleRepository;
	private readonly MongoDbAccount _accountRepository;
	private readonly MongoDbPermission _permissionRepository;

	public RoleRepositoryTests(MongoDbRandomFixture dbFixture)
	{
		_roleRepository =
			new MongoDbRole(dbFixture.Db);
		_accountRepository =
			new MongoDbAccount(dbFixture.Db);
		_permissionRepository = new MongoDbPermission(dbFixture.Db);
	}
		
	[Fact]
	public async Task CreateDeleteRole()
	{
		var role = "admin";
		await _roleRepository.Create(role);
		var result = await _roleRepository.Read();
		Assert.True(result.Exists(x => x.Name == role));
		var roleModel = await _roleRepository.Read(role);
		Assert.NotNull(roleModel);
	}

	[Fact]
	public async Task AssignPermissionToRoleAndRemove()
	{
		var role = "admin1";
		var permission = "write:admin1";
		await _roleRepository.Create(role);
		await _permissionRepository.Create(permission);
		var roleModel = await _roleRepository.Read(role);
		Assert.NotNull(roleModel);
		await _permissionRepository.AddToRole(permission, 
			roleModel.Id.ToString());
		roleModel = await _roleRepository.Read(role);
		Assert.Single(roleModel.Permissions);
		await _permissionRepository.DeleteFromRole(permission, 
			roleModel.Id.ToString());
		roleModel = await _roleRepository.Read(role);
		Assert.NotNull(roleModel);
		Assert.Empty(roleModel.Permissions);
	}
		
	[Fact]
	public async Task AssignUserToRoleAndRemove()
	{
		var role = "admin2";
		var permission = "write:admin2";
		await _roleRepository.Create(role);
		var roleModel = await _roleRepository.Read(role);
		Assert.NotNull(roleModel);
		await _permissionRepository.Create(permission);
		await _permissionRepository.AddToRole(permission, 
			roleModel.Id);
		var account = await CreateAccount();
		await _roleRepository.AddToAccount(roleModel.Id,
			account.Id);
		var roles = await _roleRepository.ReadByAccount(account.Id);
		Assert.Single(roles);
		var permissions = await 
			_permissionRepository
				.ReadByAccount(account.Id);

		Assert.Single(permissions);

		await _roleRepository.DeleteFromAccount(roleModel.Id,
			account.Id);
		roles = await _roleRepository.ReadByAccount(account.Id);
		Assert.Empty(roles);
	}
		
	[Fact]
	public async Task UpdateRole()
	{
		var role = "admin3";
		var permission = "write:admin3";
		await _roleRepository.Create(role);
		var roleModel = await _roleRepository.Read(role);
		await _permissionRepository.Create(permission);
		await _permissionRepository.AddToRole(permission, 
			roleModel.Id);
		var account1 = await CreateAccount();
		await _roleRepository.AddToAccount(roleModel.Id,
			account1.Id);
		var roles = await _roleRepository.ReadByAccount(account1.Id);
		Assert.Single(roles);
		await _roleRepository.DeleteFromAccount(roleModel.Id,
			account1.Id);
		roles = await _roleRepository.ReadByAccount(account1.Id);
		Assert.Empty(roles);
		var account2 = await CreateAccount();
		await _roleRepository.AddToAccount(roleModel.Id,
			account2.Id);
		roleModel = await _roleRepository.Read(role);
		Assert.NotNull(roleModel);
		string name = "admin4";
		await _roleRepository.Update(roleModel.Id,name);
		var roleModel2 = await _roleRepository.Read(name);
		Assert.NotNull(roleModel2);
		Assert.Equal(name, roleModel2.Name);
		Assert.Equal(roleModel.Id, roleModel2.Id);
	}

	private async Task<AccountModel> CreateAccount()
	{
		var account = GetAccountModel();
		await _accountRepository.Create(account);
		return account;
	}
		
	private AccountModel GetAccountModel()
	{
		var accountModel = new AccountModel()
		{
			Id = ObjectId.GenerateNewId().ToString(),
			Email = $"{Guid.NewGuid().ToString()}@lateflip.io",
			IsVerified = true,
			IsEnabled = true,
			IsDeleted = false
		};
			
		return accountModel;
	}
}