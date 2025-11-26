using WebAPI.DTOs;

namespace WebAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductDTO>> GetAll();
        Task<int> GetCountAsync();
        Task<ReadProductDTO?> GetByIdAsync(Guid id);
        Task<ReadProductDTO> CreateAsync(CreateProductDTO dto);
        Task<bool> UpdateAsync(Guid id, UpdateProductDTO dto);
        Task<bool> DeleteAsync(Guid id);

    }
}
