namespace Bulwark.Admin.Repositories;
public class MongoDbAccount : IAccountRepository
{
    private readonly IMongoCollection<AccountModel> _mongoCollection;

    public MongoDbAccount(IMongoDatabase db)
    {
        _mongoCollection = db.GetCollection<AccountModel>("account");
    }

    public async Task Create(AccountModel accountModel)
    {
        try
        {
            await _mongoCollection.InsertOneAsync(accountModel);
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new BulwarkAdminDbDuplicateException($"Duplicate key, email: {accountModel.Email}");
            }

            throw new BulwarkAdminDbException($"Error creating account, email: {accountModel.Email}", e);
        }
    }
    
    public async Task<AccountModel> ReadByEmail(string email)
    {
        var account = await _mongoCollection.AsQueryable()
            .Where(a => a.Email == email)
            .FirstOrDefaultAsync();
        if(account == null)
        {
            throw new BulwarkAdminDbException($"Could not find account with email: {email}");
        }
        
        return account;
    }
    
    public Task<AccountModel> ReadById(string id)
    {
        var account = _mongoCollection.AsQueryable()
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();
        if (account == null)
        { 
            throw new BulwarkAdminDbException($"Could not find account with id: {id}");
        }
        return account;
    }

    public List<AccountModel> ReadAll(int page, int size, string sortBy)
    {
        if (page == 1)
        {
            page = 0;
        }

        var accounts = _mongoCollection.AsQueryable()
            .Skip(page * size)
            .Take(size)
            .OrderBy(x => x.Email)
            .ToList();

        return accounts;
    }
    
    public async Task Update(AccountModel accountModel)
    {
        try
        {
            var update = Builders<AccountModel>.Update
                .Set(a => a.IsEnabled, accountModel.IsEnabled)
                .Set(a => a.Email, accountModel.Email)
                .Set(a => a.IsVerified, accountModel.IsVerified)
                .Set(a => a.IsDeleted, accountModel.IsDeleted)
                .Set(a => a.Modified, DateTime.Now);

            var result = await _mongoCollection.UpdateOneAsync(a => a.Id == accountModel.Id, 
                update);

            if (result.ModifiedCount < 1)
            {
                throw new BulwarkAdminDbException("Could not update account");
            }
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new BulwarkAdminDbDuplicateException("Duplicate key");
            }

            throw new BulwarkAdminDbException("Error updating account", e);
        }
    }

    public async Task Delete(string email)
    {
        var result = await _mongoCollection.DeleteOneAsync(
            a => a.Email == email);
        if (result.DeletedCount < 1)
        {
            throw new BulwarkAdminDbException("Could not delete tokens");
        }
    }
}