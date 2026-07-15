using BookingApp.Application.DTOs.Auth;
using BookingApp.Domain;

namespace BookingApp.Application.Interfaces;

public interface IAccountCreator
{
    Task<AuthResult<AccountCreationResult>> CreateWithRoleAsync(User user, string password, string role, CancellationToken cancellationToken);
}