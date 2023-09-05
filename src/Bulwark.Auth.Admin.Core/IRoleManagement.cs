using Bulwark.Admin.Repositories.Models;

namespace Bulwark.Auth.Admin.Core;

public interface IRoleManagement
{
    Task Create(string roleName);
    Task<List<RoleModel>> ReadAll();
    Task<RoleModel> ReadByName(string name);
    Task Update(string id, string newRoleName);
    Task Delete(string name);
    Task<List<string>> ReadRolesByAccount(string accountId);
    Task AddRoleToAccount(string accountId, string roleId);
    Task DeleteRoleFromAccount(string accountId, string roleId);
}