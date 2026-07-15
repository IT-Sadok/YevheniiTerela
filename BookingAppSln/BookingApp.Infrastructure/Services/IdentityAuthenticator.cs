using BookingApp.Application.DTOs.Auth;
using BookingApp.Application.Interfaces;
using BookingApp.Domain;
using Microsoft.AspNetCore.Identity;

namespace BookingApp.Infrastructure.Services;

public class IdentityAuthenticator : IAuthenticator
{
    private readonly UserManager<User> _userManager;
    
    public IdentityAuthenticator(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<AuthResult<AuthenticationResult>> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            // TODO - create non-hardcoded code-errors enum / class with constants
            return new AuthResult<AuthenticationResult>(false, ["InvalidEmailOrPassword"]);
        }
        
        // TODO - refactor to pass JWT token later - when JWT authentication is added
        return new AuthResult<AuthenticationResult>(true);
    }
}