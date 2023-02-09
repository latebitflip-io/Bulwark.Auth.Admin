namespace Bulwark.Admin.Repositories;
public interface IMagicCodeRepository
{
    Task Delete(string userId, string code);
    Task<List<MagicCodeModel>> Read(string userId);
}


