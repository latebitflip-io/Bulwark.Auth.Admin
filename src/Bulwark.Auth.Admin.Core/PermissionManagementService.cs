using Bulwark.Admin.Repositories;
using Bulwark.Admin.Repositories.Models;

namespace Bulwark.Auth.Admin.Core;

public class PermissionManagementService : IPermissionManagement
{
    private readonly IPermissionRepository _permissionRepository;
    
    public PermissionManagementService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }
    
    public async Task Create(string name)
    {
        await _permissionRepository.Create(name);
    }

    public async Task<List<PermissionModel>> ReadAll()
    {
        return await _permissionRepository.Read();
    }

    public async Task Delete(string name)
    {
        await _permissionRepository.Delete(name);
    }

    public async Task AddPermissionToRole(string roleId, string permission)
    {
        await _permissionRepository.AddToRole(permission, roleId);
    }

    public async Task DeletePermissionFromRole(string roleId, string permission)
    {
        await _permissionRepository.DeleteFromRole(permission, roleId);
    }

    public async Task<List<string>> ReadByRole(string roleId)
    {
        return await _permissionRepository.ReadByRole(roleId);
    }

    public async Task<List<string>> ReadByAccount(string accountId)
    {
        return await _permissionRepository.ReadByAccount(accountId);
    }
}