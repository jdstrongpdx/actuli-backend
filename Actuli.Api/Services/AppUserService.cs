using Actuli.Api.Models;
using Actuli.Api.Interfaces;

namespace Actuli.Api.Services;

public class AppUserService : IAppUserService
{
    private readonly ICosmosDbRepository<AppUser> _appUserRepository;

    public AppUserService(ICosmosDbRepository<AppUser> appUserRepository)
    {
        _appUserRepository = appUserRepository;
    }

    public async Task AddUserAsync(AppUser user)
    {
        user.MarkAsModified();
        await _appUserRepository.AddItemAsync(user);
    }

    public async Task<AppUser> GetUserByIdAsync(string id)
    {
        return await _appUserRepository.GetItemAsync(id);
    }

    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        return await _appUserRepository.GetAllItemsAsync();
    }

    public async Task UpdateUserAsync(string id, AppUser user)
    {
        user.MarkAsModified();
        await _appUserRepository.UpdateItemAsync(id, user);
    }

    public async Task DeleteUserAsync(string id)
    {
        await _appUserRepository.DeleteItemAsync(id);
    }
}
