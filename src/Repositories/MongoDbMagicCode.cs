namespace Bulwark.Admin.Repositories;

public class MongoDbMagicCode : IMagicCodeRepository
{
    private readonly IMongoCollection<MagicCodeModel> _mongoCollection;

    public MongoDbMagicCode(IMongoDatabase db)
    {
        _mongoCollection = db.GetCollection<MagicCodeModel>("magicCode");
    }

    public async Task Delete(string userId, string code)
    {
        var result = await _mongoCollection
            .DeleteOneAsync(mc => mc.UserId == userId
                                  && mc.Code == code);

        if (result.DeletedCount != 1)
        {
            throw new BulwarkAdminDbException("Could not delete magic code");
        }
    }

    public Task<List<MagicCodeModel>> Read(string userId)
    {
        var magicCodeModels = _mongoCollection
            .AsQueryable<MagicCodeModel>()
            .Where(mc => mc.UserId == userId)
            .ToListAsync();

        return magicCodeModels;
    }
}