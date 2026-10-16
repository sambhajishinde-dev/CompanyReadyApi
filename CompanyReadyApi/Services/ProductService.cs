
using CompanyReadyApi.DTOs;
using CompanyReadyApi.Models;
using CompanyReadyApi.Repositories;

namespace CompanyReadyApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();

            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name.Trim(),
                Price = dto.Price
            };

            var created = await _repository.AddAsync(product);

            return new ProductResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                Price = created.Price
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            product.Name = dto.Name.Trim();
            product.Price = dto.Price;

            return await _repository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
