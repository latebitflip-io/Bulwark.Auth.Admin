using Bulwark.Admin.Repositories;
using Bulwark.Admin.Repositories.Models;
using Bulwark.Auth.Admin.Core.Domain;

namespace Bulwark.Auth.Admin.Core;
/// <summary>
/// This service is responsible for managing accounts.
/// </summary>
public class AccountManagementService : IAccountManagement
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthTokenRepository _authTokenRepository;
    private readonly IMagicCodeRepository _magicCodeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    public AccountManagementService(IAccountRepository accountRepository,
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
   
    public List<AccountModel> ReadAll(string sortField, int page, int perPage)
    {
        return _accountRepository.ReadAll(page, perPage, sortField);
    }

    public async Task<AccountDetails> ReadByEmail(string email)
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

    public async Task<AccountDetails> ReadById(string id)
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

    public async Task Create(string email, bool isVerified)
    {
        var newAccount = new AccountModel
        {
            Email = email,
            IsVerified = isVerified
        };
        
        if (isVerified)
        {
            newAccount.IsEnabled = true;
        }
        
        await _accountRepository.Create(newAccount);
        
    }

    public async Task UpdateEmail(string email, string newEmail, bool isVerified)
    {
        var accountModel = await _accountRepository.ReadByEmail(email);
        accountModel.Email = newEmail;
        accountModel.IsVerified = isVerified;
        
        if (isVerified)
        {
            accountModel.IsEnabled = true;
        }
        
        await _accountRepository.Update(accountModel);
    }

    public async Task Disable(string email)
    {
        var accountModel = await _accountRepository.ReadByEmail(email);
        accountModel.IsEnabled = false;
        await _accountRepository.Update(accountModel);
    }

    public async Task Enable(string email)
    {
        var accountModel = await _accountRepository.ReadByEmail(email);
        accountModel.IsEnabled = true;
        accountModel.IsVerified = true;
        await _accountRepository.Update(accountModel);
    }

    public async Task SoftDelete(string email)
    {
        var account = await _accountRepository.ReadByEmail(email);
        account.IsDeleted = true;
        await _accountRepository.Update(account);
    }

    public async Task HardDelete(string email)
    {
        await _accountRepository.Delete(email);
    }
}