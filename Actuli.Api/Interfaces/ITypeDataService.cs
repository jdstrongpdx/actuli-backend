using Actuli.Api.Types;

namespace Actuli.Api.Interfaces;

public interface ITypeDataService
{
    Task<IEnumerable<TypeData>> GetAllTypesAsync();
    Task UpdateTypesAsync();
}