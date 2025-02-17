using Newtonsoft.Json;
using Actuli.Api.Interfaces;
using Actuli.Api.Types;

namespace Actuli.Api.Services;

public class TypeDataService : ITypeDataService
{
    private readonly ICosmosDbRepository<TypeData> _typeDataRepository;

    public TypeDataService(ICosmosDbRepository<TypeData> typeDataRepository)
    {
        _typeDataRepository = typeDataRepository;
    }

    public async Task<IEnumerable<TypeData>> GetAllTypesAsync()
    {
        return await _typeDataRepository.GetAllItemsAsync();
    }

    public async Task UpdateTypesAsync()
    {
        // Build the file path in a cleaner way
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Types", "types.json");

        // Check if the JSON file exists before reading
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"JSON file not found at path: {filePath}");
        }

        // Read and parse the JSON file
        var json = await File.ReadAllTextAsync(filePath);
        var typeItems = JsonConvert.DeserializeObject<List<TypeItem>>(json);
        if (typeItems == null || !typeItems.Any())
        {
            throw new InvalidOperationException("The JSON file is empty or contains invalid data.");
        }

        // Retrieve existing records
        var existingTypes = await _typeDataRepository.GetAllItemsAsync();

        // Prepare the ID and object for saving
        var newTypeData = new TypeData
        {
            Id = existingTypes?.FirstOrDefault()?.Id ?? Guid.NewGuid().ToString(),
            Types = typeItems
        };

        // Create or update the record
        if (existingTypes == null || !existingTypes.Any())
        {
            await _typeDataRepository.AddItemAsync(newTypeData);
        }
        else
        {
            await _typeDataRepository.UpdateItemAsync(newTypeData.Id, newTypeData);
        }
    }
}