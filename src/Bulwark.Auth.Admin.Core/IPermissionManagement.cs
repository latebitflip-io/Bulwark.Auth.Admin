using Bulwark.Admin.Repositories.Models;

namespace Bulwark.Auth.Admin.Core;

public interface IPermissionManagement
{
    Task Create(string name);
    Task<List<PermissionModel>> ReadAll();
    Task Delete(string name);
    Task AddPermissionToRole(string roleId, string permission);
    Task DeletePermissionFromRole(string roleId, string permission);
    Task<List<string>> ReadByRole(string roleId);
    Task<List<string>> ReadByAccount(string accountId);
}