using WebAPI.Models;
namespace WebAPI.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Products>> GetAll();
        Task<int> GetCountAsync();
        Task<Products?> GetByIdAsync(Guid id);
        Task AddAsync(Products Products);
        Task UpdateAsync(Products Products);
        Task DeleteAsync(Products Products);


    }
}
