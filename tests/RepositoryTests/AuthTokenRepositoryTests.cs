namespace RepositoryTests
{
	public class AuthTokenRepositoryTests : IClassFixture<MongoDbRandomFixture>
    {
        private readonly IAuthTokenRepository _authTokenRepository;
        private readonly MongoDbRandomFixture _dbFixture;

        public AuthTokenRepositoryTests(MongoDbRandomFixture dbFixture)
		{
            _dbFixture = dbFixture;
            _authTokenRepository = new MongoDbAuthToken(_dbFixture.Db);
        }

        [Fact]
        public async void ReadDeleteToken()
        {
            var userId = ObjectId.GenerateNewId().ToString();
            var deviceId = Guid.NewGuid().ToString();

            await CreateAuthToken(userId, deviceId);
            var authToken = await _authTokenRepository.Read(userId, deviceId);
            Assert.Equal(authToken.UserId.ToString(), userId);
            Assert.Equal(authToken.DeviceId, deviceId);
            await _authTokenRepository.Delete(userId, deviceId);
            authToken = await _authTokenRepository.Read(userId, deviceId);
            Assert.Null(authToken);
        }

        [Fact]
        public async void ReadAllTokens()
        {
            var userId = ObjectId.GenerateNewId().ToString();
            var deviceId = Guid.NewGuid().ToString();

            await CreateAuthToken(userId, deviceId);
            await CreateAuthToken(userId, Guid.NewGuid().ToString());
            await CreateAuthToken(userId, Guid.NewGuid().ToString());
            var authTokens = await _authTokenRepository.Read(userId);
            Assert.Equal(3, authTokens.Count);
        }

        private async Task CreateAuthToken(string userId, string deviceId)
        {
            var authTokenCollection =
                _dbFixture.Db
                .GetCollection<AuthTokenModel>("authToken");

            var now = DateTime.Now;
            var update = Builders<AuthTokenModel>.Update
                  .Set(p => p.AccessToken, Guid.NewGuid().ToString())
                  .Set(p => p.RefreshToken, Guid.NewGuid().ToString())
                  .Set(p => p.Modified, now)
                  .SetOnInsert(p => p.Created, now);

            var updateOptions = new UpdateOptions() { IsUpsert = true };

            var result = await authTokenCollection.
                UpdateOneAsync(a => a.UserId == userId &&
                a.DeviceId == deviceId,
                update, updateOptions);

            if (!(result.ModifiedCount == 1 || result.UpsertedId != null))
            {
                throw
                    new BulwarkAdminDbException("Acknowledgement failed");
            }
        }
	}
}

