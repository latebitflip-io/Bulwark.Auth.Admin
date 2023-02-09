﻿namespace TestFixtures;
public class MongoDbRandomFixture : IDisposable
{
    public MongoClient Client { get; private set; }
    public IMongoDatabase Db { get; private set; }
    private const string _connection = "mongodb://localhost:27017";
    private string _testDb;

    public MongoDbRandomFixture()
    {
        Random rnd = new Random();
        int num = rnd.Next();

        _testDb = $"bulwark_tests_{num}";
        Client = new MongoClient(_connection);
        Db = Client.GetDatabase(_testDb);
    }

    public void Dispose()
    {
        Client.DropDatabase(_testDb);
    }
}
