using Microsoft.EntityFrameworkCore;
using StudentService.Data.Models;
using StudentService.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudentService.Data.Services
{
    /// <summary>
    /// Сервис студентов в группах
    /// </summary>
    public class StudentsInGroupsService : IStudentsInGroupsService
    {
        private readonly StudentContext _context;

        public StudentsInGroupsService(StudentContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавление клиента в группу
        /// </summary>
        /// <param name="studentGuid">Guid студента</param>
        /// <param name="groupGuid">Guid группы</param>
        /// <returns>true, если студент был добавлен в группу</returns>
        public async Task<bool> AddStudentInGroup(Guid studentGuid, Guid groupGuid)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentGuid);
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupGuid);

            var isAdded = await _context.StudentsInGroups.AnyAsync(sg => sg.StudentGuid == studentGuid && sg.GroupGuid == groupGuid);

            if (student == default || group == default || isAdded) return false;

            await _context.StudentsInGroups.AddAsync(new StudentInGroup
            {
                Student = student,
                StudentGuid = studentGuid,
                Group = group,
                GroupGuid = groupGuid
            });
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Удаление студента из группы
        /// </summary>
        /// <param name="studentGuid">Guid студента</param>
        /// <param name="groupGuid">Guid группы</param>
        /// <returns>true, если студент был удален из группы</returns>
        public async Task<bool> DeleteStudentFromGroup(Guid studentGuid, Guid groupGuid)
        {
            var studentInGroup = await _context.StudentsInGroups.FirstOrDefaultAsync(sg => sg.StudentGuid == studentGuid && sg.GroupGuid == groupGuid);

            if (studentInGroup == default) return false;

            _context.StudentsInGroups.Remove(studentInGroup);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
