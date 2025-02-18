using Actuli.Api.Interfaces;
using Actuli.Api.Models;

namespace Actuli.Api.Services;

public class TypeService : ITypeService
{
    private readonly ICosmosDbRepository<TypeGroup> _typeRepository;

    public TypeService(ICosmosDbRepository<TypeGroup> typeRepository)
    {
        _typeRepository = typeRepository;
    }

    public async Task AddTypeAsync(TypeGroup type)
    {

        await _typeRepository.AddItemAsync(type);
    }

    public async Task<TypeGroup> GetTypeByIdAsync(string id)
    {
        return await _typeRepository.GetItemAsync(id);
    }

    public async Task<Dictionary<string, TypeGroup>> GetAllTypesAsync()
    {
        var items = await _typeRepository.GetAllItemsAsync();
        return items.ToDictionary<TypeGroup, string>(item => item.Name);    
    }

    public async Task UpdateTypeAsync(string id, TypeGroup type)
    {
        type.MarkAsUpdated();
        await _typeRepository.UpdateItemAsync(id, type);
    }

    public async Task DeleteTypeAsync(string id)
    {
        await _typeRepository.DeleteItemAsync(id);
    }
}