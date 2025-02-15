using Microsoft.Azure.Cosmos;
using System.Net;
using Actuli.Api.Interfaces;

namespace Actuli.Api.Repositories
{
    public class CosmosDbRepository<T> : ICosmosDbRepository<T> where T : class
    {
        private readonly Container _container;

        public CosmosDbRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(T item)
        {
            // Dynamically extract the 'Id' property for partition key
            dynamic dynamicItem = item;
            string id = dynamicItem.Id ?? throw new ArgumentException("Item does not contain 'Id' property.");

            // Ensure id isn't null and matches the partition key of the CosmosDB container
            await _container.CreateItemAsync(item, new PartitionKey(id));
        }

        public async Task<T> GetItemAsync(string id)
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

        public async Task<IEnumerable<T>> GetAllItemsAsync()
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

        public async Task UpdateItemAsync(string id, T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }

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
}