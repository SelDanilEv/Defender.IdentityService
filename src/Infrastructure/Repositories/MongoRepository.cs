using MongoDB.Driver;
using MongoDB.Bson;
using Defender.IdentityService.Application.Configuration.Options;
using Defender.IdentityService.Application.Helpers;
using Defender.Common.Entities;

namespace Defender.IdentityService.Infrastructure.Repositories;

public class MongoRepository<T> : BaseMongoRepository<T> where T : IBaseModel, new()
{
    private const string ErrorMessage = "Error occur in mongo repository";

    public MongoRepository(MongoDbOption mongoOption) : base(mongoOption)
    {
    }

    protected override async Task<IList<T>> GetItemsAsync()
    {
        var result = new List<T>();

        try
        {
            result = await _mongoCollection.Find(new BsonDocument()).ToListAsync();
        }
        catch (Exception e)
        {
            SimpleLogger.Log(e, ErrorMessage);
        }

        return result;
    }

    protected override async Task<T> GetItemWithFilterAsync(FilterDefinition<T> filter)
    {
        var result = new T();

        try
        {
            result = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            SimpleLogger.Log(e, ErrorMessage);
        }

        return result;
    }

    protected override async Task<IList<T>> GetItemsWithFilterAsync(FilterDefinition<T> filter)
    {
        var result = new List<T>();

        try
        {
            result = await _mongoCollection.Find(filter).ToListAsync();
        }
        catch (Exception e)
        {
            SimpleLogger.Log(e, ErrorMessage);
        }

        return result;
    }

    protected override async Task<T> GetItemAsync(Guid id)
    {
        var result = new T();

        try
        {
            var filter = CreateIdFilter(id);

            result = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            SimpleLogger.Log(e, ErrorMessage);
        }

        return result;
    }

    protected override async Task<T> AddItemAsync(T newModel)
    {
        try
        {
            await _mongoCollection.InsertOneAsync(newModel);
        }
        catch (Exception e)
        {
            SimpleLogger.Log(e, ErrorMessage);
        }

        return newModel;
    }

    protected override async Task<T> UpdateItemAsync(T updatedModel)
    {
        try
        {
            var filter = CreateIdFilter(updatedModel.Id);

            await _mongoCollection.ReplaceOneAsync(filter, updatedModel);
        }
        catch (Exception e)
        {
            SimpleLogger.Log(e, ErrorMessage);
        }

        return updatedModel;
    }

    protected override async Task RemoveItemAsync(Guid id)
    {
        try
        {
            var filter = CreateIdFilter(id);

            await _mongoCollection.DeleteOneAsync(filter);
        }
        catch (Exception e)
        {
            SimpleLogger.Log(e, ErrorMessage);
        }
    }

}
