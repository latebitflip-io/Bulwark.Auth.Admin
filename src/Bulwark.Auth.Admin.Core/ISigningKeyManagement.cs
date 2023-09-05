using Bulwark.Admin.Repositories.Models;

namespace Bulwark.Auth.Admin.Core;

public interface ISigningKeyManagement
{
    void GenerateKey(string algorithm = "RS256");
    List<SigningKeyModel> ReadAll();
    SigningKeyModel Read(string keyId);
    SigningKeyModel Latest();
    void Delete(string keyId);
    void Create(string privateKey, string publicKey, string algorithm);
}