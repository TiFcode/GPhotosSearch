using Google.Apis.Json;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InMemoryDataStore : IDataStore
{
    private readonly Dictionary<string, string> _store = new Dictionary<string, string>();

    public Task ClearAsync()
    {
        _store.Clear();
        return Task.CompletedTask;
    }

    public Task DeleteAsync<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key MUST have a value");
        }

        _store.Remove(key);
        return Task.CompletedTask;
    }

    public Task<T> GetAsync<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key MUST have a value");
        }

        _store.TryGetValue(key, out string value);
        return Task.FromResult(value == null ? default(T) : NewtonsoftJsonSerializer.Instance.Deserialize<T>(value));
    }

    public Task StoreAsync<T>(string key, T value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key MUST have a value");
        }

        _store[key] = NewtonsoftJsonSerializer.Instance.Serialize(value);
        return Task.CompletedTask;
    }
}
