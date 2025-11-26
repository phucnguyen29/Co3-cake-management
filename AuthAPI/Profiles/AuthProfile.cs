using AuthAPI.DTOs;
using AuthAPI.Models;
using AutoMapper;

namespace AuthAPI.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDTO, User>();
            CreateMap<LoginDTO, User>();
            CreateMap<User, RegisterDTO>();
            CreateMap<User, LoginDTO>();
        }
    }
}
