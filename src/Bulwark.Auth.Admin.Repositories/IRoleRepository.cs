namespace Bulwark.Admin.Repositories;

public interface IRoleRepository
{
	Task Create(string role);
	Task Delete(string role);
	Task<RoleModel> Read(string name);
	Task<List<string>> ReadByAccount(string accountId);
	Task<List<RoleModel>> Read();
	Task Update(string id, string name);
	Task AddToAccount(string roleId, string accountId);
	Task DeleteFromAccount(string role, string accountId);
}


