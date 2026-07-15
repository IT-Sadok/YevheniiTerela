using BookingApp.Application.DTOs.Auth;
using BookingApp.Application.Interfaces;
using BookingApp.Domain;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace BookingApp.Infrastructure.Services;

public class IdentityAccountCreator : IAccountCreator
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private static readonly IReadOnlyDictionary<string, string> _authPrivacyProtectedErrorCodesMap = new Dictionary<string, string>
    {
        // TODO - create non-hardcoded code-errors enum / class with constants
        ["DuplicateUserName"] = "InvalidUserName",
        ["DuplicateEmail"] = "InvalidUserName"
    };

    public IdentityAccountCreator(AppDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    private List<string> ExtractIdentityErrorCodes(IEnumerable<IdentityError> errors)
    {
        return errors.Select(error => _authPrivacyProtectedErrorCodesMap.ContainsKey(error.Code) ? _authPrivacyProtectedErrorCodesMap[error.Code] : error.Code).ToList();
    }
    
    public async Task<AuthResult<AccountCreationResult>> CreateWithRoleAsync(User user, string password, string role, CancellationToken cancellationToken)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            return new AuthResult<AccountCreationResult>(
                false,
                // TODO - create non-hardcoded code-errors enum / class with constants
                ["CouldNotCreateAccount", "InvalidUserRoleProvided"]
            );
        }
        
        await using var registerDbContextTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var createNewUserResult = await _userManager.CreateAsync(user, password);
        if (!createNewUserResult.Succeeded)
        {
            await registerDbContextTransaction.RollbackAsync(cancellationToken);
            
            var errorCodes = ExtractIdentityErrorCodes(createNewUserResult.Errors);
            // TODO - create non-hardcoded code-errors enum / class with constants
            errorCodes.Add("CouldNotCreateAccount");
            
            return new AuthResult<AccountCreationResult>(false,  errorCodes);
        }
        
        var addUserToRoleResult = await _userManager.AddToRoleAsync(user, role);
        if (!addUserToRoleResult.Succeeded)
        {
            await registerDbContextTransaction.RollbackAsync(cancellationToken);
            
            var errorsCodes = ExtractIdentityErrorCodes(addUserToRoleResult.Errors);
            // TODO - create non-hardcoded code-errors enum / class with constants
            errorsCodes.Add("CouldNotCreateAccount");
            
            return new AuthResult<AccountCreationResult>(false, errorsCodes);
        }
            
        await registerDbContextTransaction.CommitAsync(cancellationToken);

        return new AuthResult<AccountCreationResult>(
            true,
            null,
            user.Adapt<AccountCreationResult>()
        );
    }
}