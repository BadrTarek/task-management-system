using AutoMapper;
using UserDBModel = Data.Database.Models.User;
using UserEntity = Domain.Entities.User;
using UserTaskDBModel = Data.Database.Models.UserTask;
using UserTaskEntity = Domain.Entities.UserTask;

namespace Data.Mappers
{
    public class DomainDataMapperProfile : Profile
    {
        public DomainDataMapperProfile()
        {
            // Ignore password mapping for security
            CreateMap<UserDBModel, UserEntity>()
                .ForMember(dest => dest.UserTasks, opt => opt.MapFrom(src => src.UserTasks))
                .ReverseMap();
            CreateMap<UserTaskDBModel, UserTaskEntity>().ReverseMap();
        }
    }
}