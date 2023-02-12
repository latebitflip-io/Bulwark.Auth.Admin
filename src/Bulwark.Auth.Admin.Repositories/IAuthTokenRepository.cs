namespace Bulwark.Admin.Repositories;
public interface IAuthTokenRepository
{
    Task<AuthTokenModel> Read(string userId, string deviceId);
    Task<List<AuthTokenModel>> Read(string userId);
    Task Delete(string userId, string deviceId);
}


