using CompanyReadyApi.Data;
using CompanyReadyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyReadyApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _database;

        public ProductRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _database.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _database.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            product.Id = 0;

            _database.Products.Add(product);
            await _database.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var exsting = await _database.Products.FindAsync(product.Id);

            if(exsting == null)
            {
                return false;
            }

            exsting.Name = product.Name;
            exsting.Price = product.Price;

            await _database.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteAsync(int id) 
        {
            var product = await _database.Products.FindAsync(id);
            if(product == null)
            {
                return false;
            }

            _database.Products.Remove(product);
            await _database.SaveChangesAsync();

            return true;
        }

                
            
    }
}
