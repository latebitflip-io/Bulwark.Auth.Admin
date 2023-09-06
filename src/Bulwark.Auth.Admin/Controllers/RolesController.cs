using Bulwark.Admin.Repositories.Exceptions;
using Bulwark.Auth.Admin.Core;
using Bulwark.Auth.Admin.Payloads;

namespace Bulwark.Auth.Admin.Controllers;

[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleManagement _roleManagement;
    
    public RolesController(IRoleManagement roleManagement)
    {
        _roleManagement = roleManagement;
    }
    
    [HttpPost]
    [Route("create/{name}")]
    public async Task<IActionResult> Create(string name)
    {
        try{
            await _roleManagement.Create(name);
            return Ok();
        }
        catch (BulwarkAdminDbDuplicateException exception)
        {
            return Problem(
                title: "Role already exists",
                detail: exception.Message,
                statusCode: 409);
        }
    }
    
    [HttpGet]
    [Route("read")]
    public async Task<IActionResult> ReadAll()
    {
        try{
            var roles = await _roleManagement.ReadAll();
            return Ok(roles);
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error reading roles",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    
    [HttpGet]
    [Route("read/{name}")]
    public async Task<IActionResult> Read(string name)
    {
        try{
            var role = await _roleManagement.ReadByName(name);
            return Ok(role);
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error reading role",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateRolePayload updateRolePayload)
    {
        try{
            await _roleManagement.Update(updateRolePayload.Id, updateRolePayload.Name);
            return Ok();
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error updating role",
                detail: exception.Message,
                statusCode: 500);
        }
        catch(BulwarkAdminDbDuplicateException exception)
        {
            return Problem(
                title: "Role already exists",
                detail: exception.Message,
                statusCode: 409);
        }
    }

    [HttpDelete]
    [Route("delete/{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        try{
            await _roleManagement.Delete(name);
            return Ok();
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: $"Error deleting role: {name}",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    [HttpGet]
    [Route("read/account/{accountId}")]
    public async Task<IActionResult> ReadAccount(string accountId)
    {
        try{
            var roles = await _roleManagement.ReadRolesByAccount(accountId);
            return Ok(roles);
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error reading roles",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    
    [HttpPut]
    [Route("{roleId}/add/account/{accountId}")]
    public async Task<IActionResult> AddAccountRole(string accountId, string roleId)
    {
        try{
            await _roleManagement.AddRoleToAccount(accountId, roleId);
            return Ok();
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error adding role to account",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    
    [HttpDelete]
    [Route("{roleId}/delete/account/{accountId}")]
    public async Task<IActionResult> DeleteAccountRole(string accountId, string roleId)
    {
        try{
            await _roleManagement.DeleteRoleFromAccount(accountId, roleId);
            return Ok();
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error removing role from account",
                detail: exception.Message,
                statusCode: 500);
        }
    }
}