
namespace RepositoryTests
{
	public class MagicCodeRepositoryTests : IClassFixture<MongoDbRandomFixture>
    {
       
        private readonly IMagicCodeRepository _magicCodeRepository;
        private readonly MongoDbRandomFixture _dbFixture;
       

        public MagicCodeRepositoryTests(MongoDbRandomFixture dbFixture)
		{
            _dbFixture = dbFixture;
            _magicCodeRepository = new MongoDbMagicCode(_dbFixture.Db);
        }

        [Fact]
        public async void ReadAndDelete()
        {
            try
            {
                var userId = ObjectId.GenerateNewId();
                var code = "008009";
                await AddMagicCode(userId.ToString(), code,
                    DateTime.Now.AddMinutes(60));

                var doc = await _magicCodeRepository.Read(userId.ToString());
                Assert.Single(doc);
                Assert.True(doc[0].UserId == userId.ToString());
                await _magicCodeRepository.Delete(userId.ToString(),
                    code);

                doc = await _magicCodeRepository.Read(userId.ToString());
                Assert.Empty(doc);
            }
            catch (Exception exception)
            {
                Assert.True(false, exception.Message);
            }
        }

        private async Task AddMagicCode(string userId, string code,
            DateTime expires)
        {
            var magicCodeCollection = _dbFixture.Db
                .GetCollection<MagicCodeModel>("magicCode");

            var magicCode = new MagicCodeModel()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = userId,
                Code = code,
                Expires = expires,
                Created = DateTime.Now
            };

            var update = Builders<MagicCodeModel>.Update
                   .Set(c => c.Code, magicCode.Code)
                   .Set(c => c.Expires, magicCode.Expires)
                   .Set(c => c.Created, magicCode.Created);

            var result = await magicCodeCollection.
                UpdateOneAsync(c => c.UserId == magicCode.UserId,
                update, new UpdateOptions() { IsUpsert = true });
        }
    }
}


