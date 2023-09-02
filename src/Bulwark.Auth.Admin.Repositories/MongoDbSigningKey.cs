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

    /// <summary>
    /// This will add a new signing key, the latest key added will be the one used for signing. Other keys will exist
    /// but are for the main purpose of allowing previously signed keys to be validated.
    /// Currently the keys must be in PKCS#1 format.
    /// </summary>
    /// <param name="privateKey"></param>
    /// <param name="publicKey"></param>
    /// <param name="algorithm">The default algorithm in RS256, but all RS keys are supported</param>
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

    /// <summary>
    /// This will return a key via the keyId parameter.
    /// </summary>
    /// <param name="keyId"></param>
    /// <returns></returns>
    public SigningKeyModel Read(string keyId)
    {
        var key = _keyCollection.AsQueryable()
            .Where(c => c.KeyId == keyId)
            .FirstOrDefault();

        return key;
    }

    /// <summary>
    /// This will return the currently used signing key
    /// </summary>
    /// <returns></returns>
    public SigningKeyModel Latest()
    {
        var key = _keyCollection.AsQueryable()
            .OrderByDescending(c => c.Created)
            .FirstOrDefault();

        return key;
    }
    /// <summary>
    /// This method will delete a key from being used. If a key is deleted any accounts using the signature will be
    /// invalid and the user will need to re-authenticate.
    /// </summary>
    /// <param name="keyId"></param>
    /// <exception cref="BulwarkAdminDbException"></exception>
    public async void Delete(string keyId)
    {
        var result = await _keyCollection
            .DeleteOneAsync(a => a.KeyId == keyId);

        if (result.DeletedCount != 1)
        {
            throw new BulwarkAdminDbException("Could not delete cert");
        }
    }

    /// <summary>
    /// List all the keys currently in the database.
    /// </summary>
    /// <returns></returns>
    public List<SigningKeyModel> ReadAll()
    {
        var keys = _keyCollection.AsQueryable()
            .OrderByDescending(c => c.Created)
            .ToList();

        return keys;
    }
}