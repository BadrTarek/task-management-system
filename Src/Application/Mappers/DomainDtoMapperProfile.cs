using Application.Dtos.UserDtos;
using Application.UserTaskDtos.Dtos;
using AutoMapper;
using UserEntity = Domain.Entities.User;
using UserTaskEntity = Domain.Entities.UserTask;

namespace Data.Mappers
{
    public class DomainDtoMapperProfile : Profile
    {
        public DomainDtoMapperProfile()
        {
            CreateMap<UserEntity, UserDto>().ReverseMap();
            CreateMap<UserEntity, SignupDto>().ReverseMap();
            CreateMap<UserTaskEntity, UserTaskDto>().ReverseMap();
            CreateMap<IEnumerable<UserTaskEntity>, IEnumerable<UserTaskDto>>().ReverseMap();
            CreateMap<CreateUserTaskDto, UserTaskEntity>().ReverseMap();
        }
    }
}