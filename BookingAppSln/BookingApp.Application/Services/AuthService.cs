using BookingApp.Application.DTOs.Auth;
using BookingApp.Application.Interfaces;
using BookingApp.Domain;
using Mapster;

namespace BookingApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAccountCreator _accountCreator;
    private readonly IAuthenticator _authenticator;
    
    public AuthService(IAccountCreator accountCreator, IAuthenticator authenticator)
    {
        _accountCreator = accountCreator;
        _authenticator = authenticator;
    }
    
    public async Task<AuthResult<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (!Roles.RolesAvailableForPublicRegistration.Contains(request.Role))
        {
            return new AuthResult<RegisterResponse>(
                false,
                // TODO - create non-hardcoded code-errors enum / class with constants
                ["CouldNotCreateAccount", "InvalidUserRoleProvided"]
            );
        }
        
        var accountCreationResult = await _accountCreator.CreateWithRoleAsync(request.Adapt<User>(), request.Password, request.Role, cancellationToken);
        if (!accountCreationResult.Succeeded)
        {
            return new AuthResult<RegisterResponse>(
                false,
                accountCreationResult.Errors
            );
        }
        
        return new AuthResult<RegisterResponse>(
            true, 
            null, 
            new RegisterResponse(accountCreationResult.Response.Id)
        );
    }

    public async Task<AuthResult<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var authenticationResult = await _authenticator.AuthenticateAsync(request.Email, request.Password);

        if (!authenticationResult.Succeeded)
        {
            return new AuthResult<LoginResponse>(
                false,
                authenticationResult.Errors
            );
        }
        
        // TODO - refactor to return JWT token here when JWT authentication is implemented 
        return new AuthResult<LoginResponse>(
            true
        ); 
    }
}