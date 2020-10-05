using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentService.Data.Mappers;
using StudentService.Data.Models;
using StudentService.Domain.Enums;
using StudentService.Domain.Interfaces;
using StudentService.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentService.Data.Services.Tests
{
    [TestClass]
    public class StudentServiceTests
    {
        private readonly StudentContext _studentContext;
        private readonly IStudentService _studentService;

        public StudentServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentProfile>();
            });
            var mapper = new Mapper(config);

            var options = new DbContextOptionsBuilder<StudentContext>()
            .UseInMemoryDatabase(databaseName: "StudentsDb")
            .Options;

            _studentContext = new StudentContext(options);

            _studentContext.Students.AddRange(new List<Student>
            {
                new Student { FirstName = "Ted", LastName = "Mosby", MiddleName = "Evelyn", Sex = StudentSex.Male },
                new Student { FirstName = "Bob", LastName = "Green", MiddleName = "Daren", Sex = StudentSex.Male },
                new Student { FirstName = "Alice", LastName = "Lufier", MiddleName = "Jane", Sex = StudentSex.Female },
                new Student { FirstName = "Mary", LastName = "Born", MiddleName = "Anna", Sex = StudentSex.Female },
            });

            _studentContext.SaveChanges();

            _studentService = new StudentService(_studentContext, mapper);
        }

        [TestMethod]
        public async Task CreateStudentTest()
        {
            var currentCount = await _studentContext.Students.CountAsync();
            var student = new StudentView
            {
                FirstName = "test",
                LastName = "test",
                MiddleName = "test",
                Sex = StudentSex.Male
            };

            var result = await _studentService.CreateStudent(student);

            var isAdded = _studentContext.Students.Any(g => g.Id == result);
            var finalCount = _studentContext.Students.Count();

            Assert.IsTrue(isAdded);
            Assert.AreNotEqual(currentCount, finalCount);
        }

        [TestMethod]
        public async Task DeleteStudentTest()
        {
            var currentCount = await _studentContext.Students.CountAsync();
            var student = await _studentContext.Students.FirstAsync();

            var result = await _studentService.DeleteStudent(student.Id);

            var isDeleted = !_studentContext.Students.Any(g => g.Id == student.Id);
            var finalCount = _studentContext.Students.Count();

            Assert.IsTrue(result);
            Assert.IsTrue(isDeleted);
            Assert.AreNotEqual(currentCount, finalCount);
        }
    }
}