using AutoMapper;
using StudentService.Data.Models;
using StudentService.Domain.Models;

namespace StudentService.Data.Mappers
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupView>();
            CreateMap<GroupView, Group>()
                .ForMember(source => source.Students, options => options.Ignore());
        }
    }
}
