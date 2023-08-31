namespace Bulwark.Admin.Repositories;

public class MongoDbSigningKey : ISigningKeyRepository
{
    private readonly IMongoCollection<SigningKeyModel> _keyCollection;

    public MongoDbSigningKey(IMongoDatabase db)
    {
        _keyCollection = db.GetCollection<SigningKeyModel>("signingKeys");
        CreateIndexes();
    }

    private void CreateIndexes()
    { 
        var indexKeysDefine = Builders<SigningKeyModel>
            .IndexKeys
            .Ascending(indexKey => indexKey.KeyId);

        _keyCollection.Indexes.CreateOne(
            new CreateIndexModel<SigningKeyModel>(indexKeysDefine,
                new CreateIndexOptions()
                {
                    Unique = true,
                    Name = "Attr_Unique"
                }));
    }

    public void Create(string privateKey, string publicKey, string algorithm = "RS256")
    {
        var newKey = new SigningKeyModel
        {
            Id = ObjectId.GenerateNewId().ToString(),
            KeyId = Guid.NewGuid().ToString(),
            Format = "PKCS#1",
            PrivateKey = privateKey,
            PublicKey = publicKey,
            Algorithm = algorithm,
            Created = DateTime.Now,
        };

        _keyCollection.InsertOne(newKey);
    }

    public SigningKeyModel Read(string keyId)
    {
        var key = _keyCollection.AsQueryable()
            .Where(c => c.KeyId == keyId)
            .FirstOrDefault();

        return key;
    }

    public SigningKeyModel Latest()
    {
        var key = _keyCollection.AsQueryable()
            .OrderByDescending(c => c.Created)
            .FirstOrDefault();

        return key;
    }
    
    public async void Delete(string keyId)
    {
        var result = await _keyCollection
            .DeleteOneAsync(a => a.KeyId == keyId);

        if (result.DeletedCount != 1)
        {
            throw new BulwarkAdminDbException("Could not delete cert");
        }
    }

    public List<SigningKeyModel> ReadAll()
    {
        var keys = _keyCollection.AsQueryable()
            .OrderByDescending(c => c.Created)
            .ToList();

        return keys;
    }
}