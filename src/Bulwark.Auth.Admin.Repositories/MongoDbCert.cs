namespace Bulwark.Admin.Repositories;

public class MongoDbCert : ICertRepository
{
    private readonly IMongoCollection<CertModel> _mongoCollection;

    public MongoDbCert(IMongoDatabase db)
    {
        _mongoCollection = db.GetCollection<CertModel>("certs");
    }

    public async Task Create(string privateKey, string publicKey)
    {
        var latest = await GetLatestCert();
        var generation = 1;
        
        if (latest != null)
        {
            generation = latest.Generation + 1;
        }
        
        var newCert = new CertModel
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Generation = generation,
            PrivateKey = privateKey,
            PublicKey = publicKey,
            Created = DateTime.Now,
        };

        await _mongoCollection.InsertOneAsync(newCert);
    }

    public async void Delete(int generation)
    {
        var result = await _mongoCollection
            .DeleteOneAsync(a => a.Generation == generation);

        if (result.DeletedCount != 1)
        {
            throw new BulwarkAdminDbException("Could not delete cert");
        }
    }

    public async Task<CertModel> Read(int generation)
    {
        var cert = _mongoCollection.AsQueryable()
            .Where(c => c.Generation == generation)
            .FirstOrDefaultAsync();

        return await cert;
    }

    public async Task<List<CertModel>> ReadAll()
    {
        var certs = _mongoCollection.AsQueryable()
            .OrderByDescending(c => c.Generation)
            .ToListAsync<CertModel>();

        return await certs;
    }

    private async Task<CertModel> GetLatestCert()
    {
        var cert = _mongoCollection.AsQueryable()
            .OrderByDescending(c => c.Generation)
            .FirstOrDefaultAsync();

        return await cert;
    }
}