using Actuli.Api.Models;

namespace Actuli.Api.Interfaces;

public interface ITypeService
{
    Task AddTypeAsync(TypeGroup type);

    Task<TypeGroup> GetTypeByIdAsync(string id);
    
    Task<Dictionary<string, TypeGroup>> GetAllTypesAsync();

    Task UpdateTypeAsync(string id, TypeGroup type);
    
    Task DeleteTypeAsync(string id);

}