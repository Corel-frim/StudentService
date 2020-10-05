using AutoMapper;
using StudentService.Data.Models;
using StudentService.Domain.Models;

namespace StudentService.Data.Mappers
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentView>();
            CreateMap<StudentView, Student>()
                .ForMember(source => source.Groups, options => options.Ignore());
        }
    }
}
