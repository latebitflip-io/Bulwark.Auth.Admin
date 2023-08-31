namespace Bulwark.Admin.Repositories;

public interface ISigningKeyRepository
{
    void Create(string privateKey, string publicKey, string algorithm = "RS256");
    void Delete(string keyId);
    SigningKeyModel Read(string keyId);
    SigningKeyModel Latest();
    List<SigningKeyModel> ReadAll();
}