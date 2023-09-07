namespace TestFixtures;
public class MongoDbRandomFixture : IDisposable
{
    private MongoClient Client { get; set; }
    public IMongoDatabase Db { get; private set; }
    private const string _connection = "mongodb://localhost:27017";
    private string _testDb;

    public MongoDbRandomFixture()
    {
        var rnd = new Random();
        var  num = rnd.Next();

        _testDb = $"bulwark_tests_{num}";
        Client = new MongoClient(_connection);
        Db = Client.GetDatabase(_testDb);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Client.DropDatabase(_testDb);
        }
    }
}
