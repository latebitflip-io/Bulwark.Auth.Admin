using Bulwark.Admin.Repositories.Models;
using Bulwark.Auth.Admin.Core.Util;

namespace Bulwark.Auth.Admin.Core;

public class SigningKeyManagementService : ISigningKeyManagement
{
    public void GenerateKey(string algorithm = "RS256")
    {
        throw new NotImplementedException();
    }

    public List<SigningKeyModel> ReadAll()
    {
        throw new NotImplementedException();
    }

    public SigningKeyModel Read(string keyId)
    {
        throw new NotImplementedException();
    }

    public SigningKeyModel Latest()
    {
        throw new NotImplementedException();
    }

    public void Delete(string keyId)
    {
        throw new NotImplementedException();
    }

    public void Create(string privateKey, string publicKey, string algorithm)
    {
        throw new NotImplementedException();
    }
}