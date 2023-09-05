using Bulwark.Admin.Repositories.Exceptions;
using Bulwark.Auth.Admin.Core;
using Bulwark.Auth.Admin.Core.Domain;
using Bulwark.Auth.Admin.Models;
using Bulwark.Auth.Admin.Payloads;

namespace Bulwark.Auth.Admin.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountManagement _accountManagement;

    public AccountsController(IAccountManagement accountManagement)
    {
		_accountManagement = accountManagement;
    }

	[HttpGet]
	[Route("read")]
	public ActionResult<List<AccountModel>> ReadAll(string sortField,
		int page, int perPage){

		return _accountManagement.ReadAll(sortField, page, perPage);
	}
	
	[HttpGet]
    [Route("read/{email}")]
    public async Task<ActionResult<AccountDetails>> ReadDetailsByEmail(string email)
    {
        try
        {
            return await _accountManagement.ReadDetailsByEmail(email);
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
    public async Task<ActionResult<AccountDetails>> ReadDetailsById(string id)
    {
        try
        {
            return await _accountManagement.ReadDetailsById(id);
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
        try
        {
            await _accountManagement.Create(payload.Email, payload.IsVerified);
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
            await _accountManagement.UpdateEmail(payload.Email, payload.NewEmail, 
                false);
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
            await _accountManagement.Disable(email);
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
            await _accountManagement.Enable(email);
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
            await _accountManagement.SoftDelete(email);
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
            await _accountManagement.HardDelete(email);
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


