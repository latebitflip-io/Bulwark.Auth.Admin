namespace Bulwark.Auth.Admin.Controllers;
[ApiController]
[Route("[controller]")]
public class SigningKeyController : ControllerBase
{
	private readonly ISigningKeyRepository _signingKeyRepository;

	public SigningKeyController(ISigningKeyRepository signingKeyRepository)
	{
		_signingKeyRepository = signingKeyRepository;
	}

    [HttpGet]
    [Route("read")]
    public ActionResult<List<SigningKeyModel>> Read()
    {
        return _signingKeyRepository.ReadAll();
    }

    [HttpGet]
    [Route("read/{keyId}")]
    public ActionResult<SigningKeyModel> Read(string keyId)
    {
        return _signingKeyRepository.Read(keyId);
    }

    [HttpPost]
    [Route("create")]
    public void CreateCert(string privateKey, string publicKey)
    {
        _signingKeyRepository.Create(privateKey, publicKey);
    }
}


