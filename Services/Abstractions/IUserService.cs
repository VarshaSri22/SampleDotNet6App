using SampleDotNet6App.Domain.Models;

namespace SampleDotNet6App.Services.Abstractions;

public record LoginResult(bool Success, string? Token, string? Error);

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> RegisterAsync(string username, string password, UserRole role);
    Task<LoginResult> LoginAsync(string username, string password);
}
