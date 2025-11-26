using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories.Interfaces;
using WebAPI.Data;
using WebAPI.Models;

namespace TestPE_0511.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly WebAPIContext _context;

        public ProductsRepository(WebAPIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Products>> GetAll()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<Products?> GetByIdAsync(Guid id)
            => await _context.Products.FindAsync(id);

        public async Task AddAsync(Products Product)
        {
            await _context.Products.AddAsync(Product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Products Product)
        {
            _context.Products.Update(Product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Products Product)
        {
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
        }

    }

}
