using BookingApp.Application.DTOs.Auth;

namespace BookingApp.Application.Interfaces;

public interface IAuthenticator
{
    Task<AuthResult<AuthenticationResult>> AuthenticateAsync(string email, string password);
}