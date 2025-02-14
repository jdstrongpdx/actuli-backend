using Microsoft.Azure.Cosmos;
using System.Net;


namespace Actuli.Api.Services;

public class CosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
    }

    // Create Item
    public async Task AddItemAsync<T>(T item) where T : class
    {
        // Dynamically extract the 'Id' property for partition key
        dynamic dynamicItem = item;
        string id = dynamicItem.Id ?? throw new ArgumentException("Item does not contain 'Id' property.");
    
        // Ensure id isn't null and matches the partition key of the CosmosDB container
        await _container.CreateItemAsync(item, new PartitionKey(id));
    }
    
    // Read Item by ID
    public async Task<T> GetItemAsync<T>(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    // Read All Items
    public async Task<IEnumerable<T>> GetAllItemsAsync<T>()
    {
        var query = _container.GetItemQueryIterator<T>();
        var results = new List<T>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }
        return results;
    }

    // Update Item
    public async Task UpdateItemAsync<T>(string id, T item)
    {
        await _container.UpsertItemAsync(item, new PartitionKey(id));
    }

    // Delete Item
    public async Task DeleteItemAsync(string id)
    {
        try
        {
            await _container.DeleteItemAsync<object>(id, new PartitionKey(id));
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            // TODO Handle item not found
        }
    }
}