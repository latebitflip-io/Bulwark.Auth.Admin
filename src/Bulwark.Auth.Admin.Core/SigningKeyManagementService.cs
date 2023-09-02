using Bulwark.Admin.Repositories;
using Bulwark.Admin.Repositories.Models;
using Bulwark.Auth.Admin.Core.Util;

namespace Bulwark.Auth.Admin.Core;

public class SigningKeyManagementService : ISigningKeyManagement
{
    private readonly ISigningKeyRepository _signingKeyRepository;
    public SigningKeyManagementService(ISigningKeyRepository signingKeyRepository)
    {
        _signingKeyRepository = signingKeyRepository;
    }
    public void GenerateKey(string algorithm = "RS256")
    {
        var key = RsaKeyGenerator.MakeKey();
        _signingKeyRepository.Create(key.PrivateKey, key.PublicKey, algorithm);
    }

    public List<SigningKeyModel> ReadAll()
    {
        return _signingKeyRepository.ReadAll();
    }

    public SigningKeyModel Read(string keyId)
    {
        return _signingKeyRepository.Read(keyId);
    }

    public SigningKeyModel Latest()
    {
        return _signingKeyRepository.Latest();
    }

    public void Delete(string keyId)
    {
        _signingKeyRepository.Delete(keyId);
    }

    public void Create(string privateKey, string publicKey, string algorithm)
    {
        _signingKeyRepository.Create(privateKey, publicKey, algorithm);
    }
}