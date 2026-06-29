namespace BookingApp.Domain;

public class User
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public List<UserRole> UserRoles { get; set; } = [];
}