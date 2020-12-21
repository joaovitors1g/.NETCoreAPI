using Api.Domain.DTOs.User;
using Api.Domain.Models;
using AutoMapper;

namespace Api.CrossCuting.Mappings
{
    public class DTOToModelProfile : Profile
    {
        public DTOToModelProfile()
        {
            CreateMap<UserModel, UserDTO>().ReverseMap();
            CreateMap<UserModel, UserDTOCreate>().ReverseMap();
            CreateMap<UserModel, UserDTOUpdate>().ReverseMap();
        }
    }
}
