using Million.Backend.Models;
using Million.Backend.Repositories;

namespace Million.Backend.Services
{
    public class PropertyService
    {
        private readonly PropertyRepository _repository;

        public PropertyService(PropertyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Property>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Property?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Property>> GetFilteredAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
        {
            return await _repository.GetFilteredAsync(name, address, minPrice, maxPrice);
        }

        public async Task CreateAsync(Property property)
        {
            await _repository.CreateAsync(property);
        }

        public async Task UpdateAsync(string id, Property property)
        {
            await _repository.UpdateAsync(id, property);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
