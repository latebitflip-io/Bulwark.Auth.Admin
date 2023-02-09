namespace Bulwark.Admin.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CertController : ControllerBase
{
	private readonly ICertRepository _certRepository;

	public CertController(ICertRepository certRepository)
	{
		_certRepository = certRepository;
	}

    [HttpGet]
    [Route("read")]
    public async Task<ActionResult<List<CertModel>>> Read()
    {
        return await _certRepository.ReadAll();
    }

    [HttpGet]
    [Route("read/{generation}")]
    public async Task<ActionResult<CertModel>> ReadByGen(int generation)
    {
        return await _certRepository.Read(generation);
    }

    [HttpPost]
    [Route("create")]
    public async Task CreateCert(string privateKey, string publicKey)
    {
        await _certRepository.Create(privateKey, publicKey);
    }
}


