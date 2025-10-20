using Million.Backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Million.Backend.Repositories
{
    public class PropertyRepository
    {
        private readonly IMongoCollection<Property> _properties;

        public PropertyRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _properties = database.GetCollection<Property>(settings.Value.CollectionName);
        }

        public async Task<List<Property>> GetAllAsync()
        {
            return await _properties.Find(_ => true).ToListAsync();
        }

        public async Task<Property?> GetByIdAsync(string id)
        {
            return await _properties.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Property>> GetFilteredAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
        {
            var filterBuilder = Builders<Property>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(name))
                filter &= filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

            if (!string.IsNullOrEmpty(address))
                filter &= filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(address, "i"));

            if (minPrice.HasValue)
                filter &= filterBuilder.Gte(p => p.Price, minPrice.Value);

            if (maxPrice.HasValue)
                filter &= filterBuilder.Lte(p => p.Price, maxPrice.Value);

            return await _properties.Find(filter).ToListAsync();
        }

        public async Task CreateAsync(Property property)
        {
            await _properties.InsertOneAsync(property);
        }

        public async Task UpdateAsync(string id, Property property)
        {
            await _properties.ReplaceOneAsync(p => p.Id == id, property);
        }

        public async Task DeleteAsync(string id)
        {
            await _properties.DeleteOneAsync(p => p.Id == id);
        }
    }
}
