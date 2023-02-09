namespace Bulwark.Admin.Repositories;

public class MongoDbPermission : IPermissionRepository
{
    private readonly IMongoCollection<PermissionModel> _mongoPermissionCollection;
    private readonly IMongoCollection<RoleModel> _mongoRoleCollection;
    private readonly IMongoCollection<AccountModel> _mongoAccountCollection;
    
    public MongoDbPermission(IMongoDatabase db)
    {
        _mongoPermissionCollection =
            db.GetCollection<PermissionModel>("permission");
        _mongoRoleCollection =
            db.GetCollection<RoleModel>("role");
        _mongoAccountCollection =
            db.GetCollection<AccountModel>("account");
    }
    
    public async Task Create(string permission)
    {
        try
        {
            var permissionModel = new PermissionModel()
            {
                Id = permission,
                Created = DateTime.Now
            };

            await _mongoPermissionCollection.InsertOneAsync(permissionModel);
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new BulwarkAdminDbDuplicateException($"Duplicate key: {permission}", e);
            }

            throw new BulwarkAdminDbException($"Error creating permission: {permission}", e);
        }
    }
    
    public async Task<List<PermissionModel>> Read()
    {
        return await _mongoPermissionCollection.Find(_ => true).ToListAsync();
    }
    
    public async Task Delete(string permission)
    {
        try
        {
            var permissionModel = await _mongoPermissionCollection
                .Find(x => x.Id == permission)
                .FirstOrDefaultAsync();

            if (permissionModel == null)
            {
                throw new BulwarkAdminDbException($"Permission - {permission} not found");
            }

            await _mongoPermissionCollection.DeleteOneAsync(
                a => a.Id == permission);
            await _mongoRoleCollection.UpdateManyAsync(
                Builders<RoleModel>.Filter.Eq("Permissions", permission),
                Builders<RoleModel>.Update.Pull("Permissions", permission));
        }
        catch (Exception e)
        {
            throw new BulwarkAdminDbException($"Error deleting permission: {permission}", e);
        }
    }
    
    public async Task<List<string>> ReadByAccount(string accountId)
    {
        var accountModel = await _mongoAccountCollection
            .Find(x => x.Id == accountId)
            .FirstOrDefaultAsync();

        if (accountModel == null)
        {
            throw new BulwarkAdminDbException($"Account - {accountId} not found");
        }

        var roles = await _mongoRoleCollection
            .Find(x => accountModel.Roles.Contains(x.Id))
            .ToListAsync();
        
        var permissions = new List<string>();
        foreach (var role in roles)
        {
            permissions
                .AddRange(role.Permissions.Select(x => x.ToString()));
        }

        return permissions.Distinct().ToList();

    }
    
    public async Task<List<string>> ReadByRole(string roleId)
    {
        var roleModel = await _mongoRoleCollection
            .Find(x => x.Id == roleId)
            .FirstOrDefaultAsync();

        if (roleModel == null)
        {
            throw new BulwarkAdminDbException($"Role - {roleId} not found");
        }

        var roles = await _mongoPermissionCollection
            .Find(x => roleModel.Permissions.Contains(x.Id))
            .ToListAsync();

        return roles.Select(x => x.Id).ToList();
    }
    
    public async Task AddToRole(string permission, string roleId)
    {
        try
        {
            var roleModel = await _mongoRoleCollection
                .Find(x => x.Id == roleId)
                .FirstOrDefaultAsync();
            var permissionModel = await _mongoPermissionCollection
                .Find(x => x.Id == permission)
                .FirstOrDefaultAsync();

            if (roleModel == null)
            {
                throw new BulwarkAdminDbException($"Role - {roleId} not found");
            }

            if (permissionModel == null)
            {
                throw new BulwarkAdminDbException($"Permission - {permission} not found");
            }
            
            roleModel.Permissions.Add(permissionModel.Id);

            var update = Builders<RoleModel>.Update
                .Set(a => a.Permissions, roleModel.Permissions.Distinct())
                .Set(a => a.Modified, DateTime.Now);

            var result = await _mongoRoleCollection
                .UpdateOneAsync(r => r.Id == roleModel.Id, update);

            if (result.ModifiedCount < 1)
            {
                throw new BulwarkAdminDbException("Could not update role with permission");
            }
        }
        catch (MongoWriteException e)
        {
            throw new BulwarkAdminDbException("Error assigning permission to role", e);
        }
    }
    
    public async Task DeleteFromRole(string permission, string roleId)
    {
        try
        {
            var roleModel = await _mongoRoleCollection
                .Find(x => x.Id == roleId)
                .FirstOrDefaultAsync();

            if (roleModel == null)
            {
                throw new BulwarkAdminDbException($"Role - {roleId} not found");
            }

            roleModel.Permissions.Remove(permission);

            var update = Builders<RoleModel>.Update
                .Set(a => a.Permissions, roleModel.Permissions)
                .Set(a => a.Modified, DateTime.Now);

            var result = await _mongoRoleCollection
                .UpdateOneAsync(r => r.Id == roleModel.Id, update);

            if (result.ModifiedCount < 1)
            {
                throw new BulwarkAdminDbException("Could not update account");
            }
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new BulwarkAdminDbDuplicateException("Duplicate key in permission role mapping");
            }

            throw new BulwarkAdminDbException("Error assigning permission to role", e);
        }
    }
}