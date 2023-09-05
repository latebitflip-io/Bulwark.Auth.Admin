using Bulwark.Auth.Admin.Core;

namespace Bulwark.Auth.Admin.Controllers;
[ApiController]
[Route("[controller]")]
public class SigningKeyController : ControllerBase
{
    private readonly ISigningKeyManagement _signingKeyManagement;
	public SigningKeyController(ISigningKeyManagement signingKeyManagement)
	{
		_signingKeyManagement = signingKeyManagement;
	}

    [HttpGet]
    [Route("read")]
    public ActionResult<List<SigningKeyModel>> Read()
    {
        return _signingKeyManagement.ReadAll();
    }

    [HttpGet]
    [Route("read/{keyId}")]
    public ActionResult<SigningKeyModel> Read(string keyId)
    {
        return _signingKeyManagement.Read(keyId);
    }

    [HttpPost]
    [Route("create")]
    public void CreateCert(string privateKey, string publicKey, string algorithm)
    {
        _signingKeyManagement.Create(privateKey, publicKey, algorithm);
    }
}