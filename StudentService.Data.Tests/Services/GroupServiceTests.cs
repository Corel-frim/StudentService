using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentService.Data.Models;
using StudentService.Data.Mappers;
using StudentService.Domain.Interfaces;
using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentService.Domain;

namespace StudentService.Data.Services.Tests
{
    [TestClass]
    public class GroupServiceTests
    {
        private readonly StudentContext _studentContext;
        private readonly IGroupService _groupService;

        public GroupServiceTests()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<GroupProfile>();
            });
            var mapper = new Mapper(config);

            var options = new DbContextOptionsBuilder<StudentContext>()
            .UseInMemoryDatabase(databaseName: "StudentsDb")
            .Options;

            _studentContext = new StudentContext(options);

            _studentContext.Groups.AddRange(new List<Group>
            {
                new Group { Id = Guid.NewGuid(), Name = "123" },
                new Group { Id = Guid.NewGuid(), Name = "234" },
                new Group { Id = Guid.NewGuid(), Name = "345" },
                new Group { Id = Guid.NewGuid(), Name = "456" },
                new Group { Id = Guid.NewGuid(), Name = "567" },
                new Group { Id = Guid.NewGuid(), Name = "678" },
            });

            _studentContext.SaveChanges();

            _groupService = new GroupService(_studentContext, mapper);
        }

        [TestMethod]
        public async Task CreateGroupTest()
        {
            var currentCount = await _studentContext.Groups.CountAsync();
            var group = new GroupView { Name = "test" };

            var result = await _groupService.CreateGroup(group);

            var isAdded = _studentContext.Groups.Any(g => g.Id == result);
            var finalCount = await _studentContext.Groups.CountAsync();

            Assert.IsTrue(isAdded);
            Assert.AreNotEqual(currentCount, finalCount);
        }

        [TestMethod]
        public async Task DeleteGroupTest()
        {
            var currentCount = await _studentContext.Groups.CountAsync();
            var group = await _studentContext.Groups.LastAsync();

            var result = await _groupService.DeleteGroup(group.Id);

            var isDeleted = !await _studentContext.Groups.AnyAsync(g => g.Id == group.Id);
            var finalCount = await _studentContext.Groups.CountAsync();

            Assert.IsTrue(result);
            Assert.IsTrue(isDeleted);
            Assert.AreNotEqual(currentCount, finalCount);
        }

        [TestMethod]
        public async Task GetGroupsTest()
        {
            var searchFor = "23";
            var dbCount = _studentContext
                .Groups
                .Where(g => g.Name.Contains(searchFor))
                .ThenWhere(g => g.Name.StartsWith(searchFor))
                .ThenWhere(g => g.Name.Equals(searchFor))
                .Count();

            var result = await _groupService.GetGroups(searchFor);

            Assert.AreEqual(dbCount, result.Count);
        }
    }
}