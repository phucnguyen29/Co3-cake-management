using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
namespace WebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductsRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadProductDTO>> GetAll()
        {
            var Productss = await _repo.GetAll();
            //return _mapper.Map<IQueryable<ReadProductsDTO>>(Productss);
            return _mapper.Map<IEnumerable<ReadProductDTO>>(Productss);
        }

        public async Task<int> GetCountAsync()
        {
            return await _repo.GetCountAsync();
        }

        public async Task<ReadProductDTO?> GetByIdAsync(Guid id)
        {
            var Products = await _repo.GetByIdAsync(id);
            return Products == null ? null : _mapper.Map<ReadProductDTO>(Products);
        }

        public async Task<ReadProductDTO> CreateAsync(CreateProductDTO dto)
        {
            var newProducts = _mapper.Map<Products>(dto);
            newProducts.Id = Guid.NewGuid();
            await _repo.AddAsync(newProducts);
            return _mapper.Map<ReadProductDTO>(newProducts);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateProductDTO dto)
        {
            var Products = await _repo.GetByIdAsync(id);
            if (Products == null) return false;

            _mapper.Map(dto, Products);
            await _repo.UpdateAsync(Products);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var Products = await _repo.GetByIdAsync(id);
            if (Products == null) return false;

            Products.IsDeleted = true;
            Products.DeletedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(Products);
            return true;
        }
    }




}
