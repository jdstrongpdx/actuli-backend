namespace Actuli.Api.Interfaces;

public interface ICosmosDbRepository<T> where T : class
{
    Task AddItemAsync(T item);
    Task<T> GetItemAsync(string id);
    Task<IEnumerable<T>> GetAllItemsAsync();
    Task UpdateItemAsync(string id, T item);
    Task DeleteItemAsync(string id);
}
