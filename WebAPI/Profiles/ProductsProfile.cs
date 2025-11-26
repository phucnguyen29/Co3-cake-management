using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Products, ReadProductDTO>();
            CreateMap<ReadProductDTO, Products>();

            CreateMap<CreateProductDTO, Products>();
            CreateMap<Products, CreateProductDTO>();

            CreateMap<UpdateProductDTO, Products>();
            CreateMap<Products, UpdateProductDTO>();

        }
    }
}
