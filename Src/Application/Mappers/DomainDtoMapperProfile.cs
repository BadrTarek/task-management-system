using Application.Dtos.UserDtos;
using AutoMapper;
using UserEntity = Domain.Entities.User;

namespace Data.Mappers
{
    public class DomainDtoMapperProfile : Profile
    {
        public DomainDtoMapperProfile()
        {
            CreateMap<UserEntity, UserDto>().ReverseMap();
            CreateMap<UserEntity, SignupDto>().ReverseMap();
        }
    }
}