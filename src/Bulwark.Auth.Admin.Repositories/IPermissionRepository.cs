namespace Bulwark.Admin.Repositories;

public interface IPermissionRepository
{
    Task Create(string permission);
    Task Delete(string permission);
    Task<List<PermissionModel>> Read();
    Task<List<string>> ReadByAccount(string accountId);
    Task<List<string>> ReadByRole(string roleId);
    Task AddToRole(string permission, string roleId);
    Task DeleteFromRole(string permission, string roleId);

}