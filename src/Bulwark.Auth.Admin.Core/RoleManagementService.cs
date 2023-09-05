using Bulwark.Admin.Repositories;
using Bulwark.Admin.Repositories.Models;

namespace Bulwark.Auth.Admin.Core;

public class RoleManagementService : IRoleManagement
{
    private readonly IRoleRepository _roleRepository;
    
    public RoleManagementService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public async Task Create(string roleName)
    {
        await _roleRepository.Create(roleName);
    }

    public async Task<List<RoleModel>> ReadAll()
    {
        return await _roleRepository.Read();
    }

    public async Task<RoleModel> ReadByName(string name)
    {
        return await _roleRepository.Read(name);
    }

    public async Task Update(string id, string newRoleName)
    {
        await _roleRepository.Update(id, newRoleName);
    }

    public async Task Delete(string name)
    {
        await _roleRepository.Delete(name);
    }

    public async Task<List<string>> ReadRolesByAccount(string accountId)
    {
        return await _roleRepository.ReadByAccount(accountId);
    }

    public async Task AddRoleToAccount(string accountId, string roleId)
    {
        await _roleRepository.AddToAccount(roleId, accountId);
    }

    public async Task DeleteRoleFromAccount(string accountId, string roleId)
    {
        await _roleRepository.DeleteFromAccount(roleId, accountId);
    }
}