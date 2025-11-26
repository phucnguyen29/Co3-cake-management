using WebClient.DTOs;

namespace WebAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductDTO>> GetAllAsync();
        Task<ReadProductDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(CreateProductDTO dto);
        Task UpdateAsync(Guid id, UpdateProductDTO dto);
        Task DeleteAsync(Guid id);

        Task<string> GetRawAsync(string format);
    }
}
