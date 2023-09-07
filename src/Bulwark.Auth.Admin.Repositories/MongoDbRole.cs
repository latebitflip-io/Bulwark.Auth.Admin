namespace Bulwark.Admin.Repositories;
public class MongoDbRole : IRoleRepository
{
    private readonly IMongoCollection<RoleModel> _mongoRoleCollection;
    private readonly IMongoCollection<AccountModel> _mongoAccountCollection;
    
    public MongoDbRole(IMongoDatabase db)
    {
        _mongoRoleCollection =
            db.GetCollection<RoleModel>("role");
        _mongoAccountCollection =
            db.GetCollection<AccountModel>("account");
       
        CreateIndexes();
    }
    
    public async Task DeleteFromAccount(string roleId, string accountId)
    {
        try
        {
            var accountModel = await _mongoAccountCollection
                .Find(x => x.Id == accountId)
                .FirstOrDefaultAsync();
            var roleModel = await _mongoRoleCollection
                .Find(x => x.Id == roleId)
                .FirstOrDefaultAsync();
            
            if (roleModel == null)
            {
                throw new BulwarkAdminDbException($"Role - {roleId} not found");
            }
            
            if(accountModel == null)
            {
                throw new BulwarkAdminDbException($"Account - {accountId} not found");
            }
            
            accountModel.Roles.Remove(roleModel.Id);
            
            var update = Builders<AccountModel>.Update
                .Set(a => a.Roles, accountModel.Roles)
                .Set(a => a.Modified, DateTime.Now);


            await _mongoAccountCollection
                .UpdateOneAsync(a => a.Id == accountModel.Id, update);

        }
        catch (MongoWriteException e)
        {
            throw new BulwarkAdminDbException("Error assigning permission to role", e);
        }
    }
    
    public async Task Create(string role)
    {
        try
        {
            var roleModel = new RoleModel()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = role,
                Permissions = new List<string>(),
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            await _mongoRoleCollection.InsertOneAsync(roleModel);
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new BulwarkAdminDbDuplicateException($"Duplicate key: {role}", e);
            }

            throw new BulwarkAdminDbException($"Error creating role: {role}", e);
        }
    }

    public async Task Delete(string name)
    {
        var roleModel = await _mongoRoleCollection
            .Find(x => x.Name == name)
            .FirstOrDefaultAsync();
        
        if(roleModel == null)
        {
            throw new BulwarkAdminDbException($"Role - {name} not found");
        }
        
        await _mongoRoleCollection.DeleteOneAsync(
            a => a.Id == roleModel.Id);
        await _mongoAccountCollection.UpdateManyAsync(
            Builders<AccountModel>.Filter.Eq("Roles", roleModel.Id),
            Builders<AccountModel>.Update.Pull("Roles", roleModel.Id));
    }

    public async Task<RoleModel> Read(string name)
    {
        try
        {
            var role = await _mongoRoleCollection
                .Find(x => x.Name == name)
                .FirstOrDefaultAsync();
            if (role == null)
            {
                throw new BulwarkAdminDbException($"Role: {name} not found");
            }

            return role;
        }
        catch (MongoException e)
        {
            throw new BulwarkAdminDbException($"Error reading role: {name}", e);
        }
    }

    public async Task<List<RoleModel>> Read()
    {
        return await _mongoRoleCollection.Find(_ => true).ToListAsync();
    }
    
    public async Task<List<string>> ReadByAccount(string accountId)
    {
        var accountModel = await _mongoAccountCollection
            .Find(x => x.Id == accountId)
            .FirstOrDefaultAsync();
            
        if(accountModel == null)
        {
            throw new BulwarkAdminDbException($"Account - {accountId} not found");
        }
            
        var roles = await _mongoRoleCollection
            .Find(x => accountModel.Roles.Contains(x.Id))
            .ToListAsync();
            
        return roles.Select(x => x.Name).ToList();
    }
    public async Task Update(string id, string name)
    {
        try
        {
            var roleModel = await _mongoRoleCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (roleModel == null)
            {
                throw new BulwarkAdminDbException($"Role - {id} not found");
            }

            var update = Builders<RoleModel>.Update
                .Set(a => a.Name, name)
                .Set(a => a.Modified, DateTime.Now);

            var result = await _mongoRoleCollection
                .UpdateOneAsync(r => r.Id == id, update);

            if (result.ModifiedCount < 1)
            {
                throw new BulwarkAdminDbException("Could not update role");
            }
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new BulwarkAdminDbDuplicateException($"Duplicate role: {name}", e);
            }

            throw new BulwarkAdminDbException("Unknown error", e);
        }
    }
    
    public async Task AddToAccount(string roleId, string accountId)
    {
        try
        {
            var accountModel = await _mongoAccountCollection
                .Find(x => x.Id == accountId)
                .FirstOrDefaultAsync();
            var roleModel = await _mongoRoleCollection
                .Find(x => x.Id == roleId)
                .FirstOrDefaultAsync();

            if (roleModel == null)
            {
                throw new BulwarkAdminDbException($"Role - {roleId} not found");
            }

            if (accountModel == null)
            {
                throw new BulwarkAdminDbException($"Account - {accountId} not found");
            }

            accountModel.Roles.Add(roleModel.Id);

            var update = Builders<AccountModel>.Update
                .Set(a => a.Roles, accountModel.Roles)
                .Set(a => a.Modified, DateTime.Now);

            await _mongoAccountCollection
                .UpdateOneAsync(a => a.Id == accountModel.Id, update);
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
    private void CreateIndexes()
    {
        CreateRoleIndexes();
    }
    private void CreateRoleIndexes()
    {
        var indexKeysDefine = Builders<RoleModel>
            .IndexKeys
            .Ascending(indexKey => indexKey.Name);

        _mongoRoleCollection.Indexes.CreateOne(
            new CreateIndexModel<RoleModel>(indexKeysDefine,
                new CreateIndexOptions() { Unique = true, Name = "Role_Name_Unique" }));
    }
}