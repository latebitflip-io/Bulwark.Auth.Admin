using Bulwark.Admin.Repositories.Exceptions;

namespace Bulwark.Auth.Admin.Controllers;

[ApiController]
[Route("[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionRepository _permissionRepository;
    
    public PermissionsController(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }
    
    [HttpPost]
    [Route("create/{name}")]
    public async Task<IActionResult> Create(string name)
    {
        try
        {
            await _permissionRepository.Create(name);
            return Ok();
        }
        catch (BulwarkAdminDbDuplicateException)
        {
            return Problem(
                title: "Permission already exists",
                detail: $"Permission with name {name} already exists",
                statusCode: 409);
        }
    }
    
    [HttpGet]
    [Route("read")]
    public async Task<IActionResult> Read()
    {
        try
        {
            var permissions = await _permissionRepository.Read();
            return Ok(permissions);
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error reading permissions",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    [HttpDelete]
    [Route("delete/{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        try
        {
            await _permissionRepository.Delete(name);
            return Ok();
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error deleting permission",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    
    [HttpPut]
    [Route("{permission}/add/role/{roleId}")]
    public async Task<IActionResult> AddToRole(string roleId, string permission)
    {
        try
        {
            await _permissionRepository.AddToRole(permission, roleId);
            return Ok();
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error adding permission to role",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    
    [HttpDelete]
    [Route("{permission}/delete/role/{roleId}")]
    public async Task<IActionResult> DeleteFromRole(string roleId, string permission)
    {
        try
        {
            await _permissionRepository.DeleteFromRole(permission, roleId);
            return Ok();
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error deleting permission from role",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    
    [HttpGet]
    [Route("read/role/{roleId}")]
    public async Task<IActionResult> ReadByRole(string roleId)
    {
        try
        {
            var permissions = await _permissionRepository.ReadByRole(roleId);
            return Ok(permissions);
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error reading role permissions",
                detail: exception.Message,
                statusCode: 500);
        }
    }
    
    [HttpGet]
    [Route("read/account/{accountId}")]
    public async Task<IActionResult> ReadByAccount(string accountId)
    {
        try
        {
            var permissions = await _permissionRepository.ReadByAccount(accountId);
            return Ok(permissions);
        }
        catch (BulwarkAdminDbException exception)
        {
            return Problem(
                title: "Error reading account permissions",
                detail: exception.Message,
                statusCode: 500);
        }
    }
}