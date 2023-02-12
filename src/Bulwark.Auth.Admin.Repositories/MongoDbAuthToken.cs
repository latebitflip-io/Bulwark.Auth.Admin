namespace Bulwark.Admin.Repositories;
public class MongoDbAuthToken : IAuthTokenRepository
{
    private IMongoCollection<AuthTokenModel> _mongoCollection;

    public MongoDbAuthToken(IMongoDatabase db)
    {
        _mongoCollection = db.GetCollection<AuthTokenModel>("authToken");
    }

    public async Task Delete(string userId, string deviceId)
    {
        var result = await _mongoCollection
            .DeleteOneAsync(a => a.UserId == userId
                                 && a.DeviceId == deviceId);

        if (result.DeletedCount != 1)
        {
            throw new BulwarkAdminDbException("Could not delete token");
        }
    }

    public async Task<AuthTokenModel> Read(string userId, string deviceId)
    {
        var authToken = _mongoCollection.AsQueryable()
            .Where(r => r.UserId == userId &&
                        r.DeviceId == deviceId)
            .FirstOrDefaultAsync();

        return await authToken;
    }

    public async Task<List<AuthTokenModel>> Read(string userId)
    {
        var authToken = _mongoCollection.AsQueryable()
            .Where(r => r.UserId == userId)
            .ToListAsync();

        return await authToken;
    }
}