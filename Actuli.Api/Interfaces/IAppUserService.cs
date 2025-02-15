

using Actuli.Api.Models;

namespace Actuli.Api.Interfaces;

public interface IAppUserService
{
    Task AddUserAsync(AppUser user);
    Task<AppUser> GetUserByIdAsync(string id);
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task UpdateUserAsync(string id, AppUser user);
    Task DeleteUserAsync(string id);
}
