namespace Bulwark.Admin.Repositories;
public interface ICertRepository
{
    Task Create(string privateKey, string publicKey);
    void Delete(int generation);
    Task<CertModel> Read(int generation);
    Task<List<CertModel>> ReadAll();
}

