using Bulwark.Admin.Api.Models;
using Bulwark.Admin.Api.Payloads;
using Bulwark.Admin.Repositories.Exceptions;

namespace Bulwark.Admin.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
	private readonly IAccountRepository _accountRepository;
    private readonly IAuthTokenRepository _authTokenRepository;
    private readonly IMagicCodeRepository _magicCodeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AccountsController(IAccountRepository accountRepository,
        IAuthTokenRepository authTokenRepository,
        IMagicCodeRepository magicCodeRepository,
        IRoleRepository roleRepository, 
        IPermissionRepository permissionRepository)
    {
		_accountRepository = accountRepository;
        _authTokenRepository = authTokenRepository;
        _magicCodeRepository = magicCodeRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

	[HttpGet]
	[Route("read")]
	public ActionResult<List<AccountModel>> ReadAll(string sortField,
		int page, int perPage){

		return _accountRepository.ReadAll(page, perPage, sortField);
	}
	
	[HttpGet]
    [Route("read/{email}")]
    public async Task<ActionResult<AccountDetails>> ReadByEmail(string email)
    {
        try
        {
            var account = await _accountRepository.ReadByEmail(email);
            var authTokens =
                await _authTokenRepository.Read(account.Id);
            var magicCodes = await _magicCodeRepository.Read(account.Id);
            var roles = await _roleRepository
                .ReadByAccount(account.Id);

            var permissions = await _permissionRepository
                .ReadByAccount(account.Id);

            var accountDetails = new AccountDetails()
            {
                Id = account.Id,
                Email = account.Email,
                IsVerified = account.IsVerified,
                IsEnabled = account.IsEnabled,
                IsDeleted = account.IsDeleted,
                SocialProviders = account.SocialProviders,
                AuthTokens = authTokens,
                MagicCodes = magicCodes,
                Roles = roles,
                Permissions = permissions,
                Created = account.Created,
                Modified = account.Modified
            };
            
            return accountDetails;
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
    }
    
    [HttpGet]
    [Route("read/id/{id}")]
    public async Task<ActionResult<AccountDetails>> ReadById(string id)
    {
        try
        {
            var account = await _accountRepository.ReadById(id);
            var authTokens =
                await _authTokenRepository.Read(account.Id);
            var magicCodes = await _magicCodeRepository.Read(account.Id);
            var roles = await _roleRepository
                .ReadByAccount(account.Id);

            var permissions = await _permissionRepository
                .ReadByAccount(account.Id);

            var accountDetails = new AccountDetails()
            {
                Id = account.Id,
                Email = account.Email,
                IsVerified = account.IsVerified,
                IsEnabled = account.IsEnabled,
                IsDeleted = account.IsDeleted,
                SocialProviders = account.SocialProviders,
                AuthTokens = authTokens,
                MagicCodes = magicCodes,
                Roles = roles,
                Permissions = permissions,
                Created = account.Created,
                Modified = account.Modified
            };
            
            return accountDetails;
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult> Create(CreateAccountPayload payload)
    {
        var newAccount = new AccountModel();
        newAccount.Email = payload.Email;
        newAccount.IsVerified = payload.IsVerified;
        if (payload.IsVerified)
        {
            newAccount.IsEnabled = true;
        }

        try
        {
            await _accountRepository.Create(newAccount);
        }
        catch (BulwarkAdminDbDuplicateException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        catch(BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Database Error",
                detail: exception.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return Ok();
    }

    [HttpPut]
    [Route("update/email")]
    public async Task<ActionResult> UpdateEmail(UpdateEmailPayload payload)
    {
        try
        {
            var accountModel = await _accountRepository.ReadByEmail(payload.Email);
            accountModel.Email = payload.NewEmail;
            await _accountRepository.Update(accountModel);
        }
        catch(BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        catch (BulwarkAdminDbDuplicateException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        return Ok();
    }
    
    [HttpPut]
    [Route("disable/{email}")]
    public async Task<ActionResult> Disable(string email)
    {
        try
        {
            var accountModel = await _accountRepository.ReadByEmail(email);
            accountModel.IsEnabled = false;
            await _accountRepository.Update(accountModel);
        }
        catch(BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        
        return Ok();
    }

    [HttpPut] 
    [Route("enable/{email}")] 
    public async Task<ActionResult> Enable(string email)
    {
        try
        {
            var accountModel = await _accountRepository.ReadByEmail(email);
            accountModel.IsEnabled = true;
            accountModel.IsVerified = true;
            await _accountRepository.Update(accountModel);
        }
        catch(BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        return Ok();
    }

    /// <summary>
    /// Does a soft delete of the account
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("delete/{email}")]
    public async Task<ActionResult> SoftDelete(string email)
    {
        try
        {
            var account = await _accountRepository.ReadByEmail(email);
            account.IsDeleted = true;
            await _accountRepository.Update(account);
        }
        catch(BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        return Ok();
    }

    /// <summary>
    /// Does a hard delete of the account
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete/{email}")]
    public async Task<ActionResult> HardDelete(string email)
    {
        try
        {
            await _accountRepository.Delete(email);
        }
        catch(BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Bad Input",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        
        return Ok();
    }
}


