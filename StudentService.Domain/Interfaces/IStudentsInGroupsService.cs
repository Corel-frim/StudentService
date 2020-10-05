using System;
using System.Threading.Tasks;

namespace StudentService.Domain.Interfaces
{
    /// <summary>
    /// Сервис студентов в группах
    /// </summary>
    public interface IStudentsInGroupsService
    {
        /// <summary>
        /// Удаление студента из группы
        /// </summary>
        /// <param name="studentGuid">Guid студента</param>
        /// <param name="groupGuid">Guid группы</param>
        /// <returns>true, если студент был удален из группы</returns>
        Task<bool> DeleteStudentFromGroup(Guid student, Guid group);

        /// <summary>
        /// Добавление клиента в группу
        /// </summary>
        /// <param name="studentGuid">Guid студента</param>
        /// <param name="groupGuid">Guid группы</param>
        /// <returns>true, если студент был добавлен в группу</returns>
        Task<bool> AddStudentInGroup(Guid student, Guid group);
    }
}
