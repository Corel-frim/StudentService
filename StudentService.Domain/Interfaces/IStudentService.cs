using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentService.Domain.Interfaces
{
    /// <summary>
    /// Сервис работы с студентами
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Получение списка студентов с возможностью пагинации и фильтрации
        /// </summary>
        /// <param name="filter">Фильтры поиска</param>
        /// <returns>Список студентов и количество доступных студентов с данными фильтрами</returns>
        Task<StudentList> GetStudents(StudentListFilter filter);

        /// <summary>
        /// Получение данных студента по Guid
        /// </summary>
        /// <param name="guid">Guid студента</param>
        /// <returns>Данные студента</returns>
        Task<StudentView> GetStudent(Guid guid);

        /// <summary>
        /// Удаление студента по Guid
        /// </summary>
        /// <param name="guid">Guid студента</param>
        /// <returns>true, если студент был удален</returns>
        Task<bool> DeleteStudent(Guid guid);

        /// <summary>
        /// Обновление данных клиента студента
        /// </summary>
        /// <param name="student">Обновленные данные клиента</param>
        /// <returns>true, если данные были обновлены</returns>
        Task<bool> UpdateStudent(StudentView student);

        /// <summary>
        /// Создание студента
        /// </summary>
        /// <param name="student">Данные создаваемого студента</param>
        /// <returns>Guid созданного студента</returns>
        Task<Guid> CreateStudent(StudentView student);

        /// <summary>
        /// Проверка необязательного уникального идентификатора
        /// </summary>
        /// <param name="nickname">Идентификатор</param>
        /// <returns>true, если идентификатор пустой или не занят</returns>
        Task<bool> CheckNickname(string nickname);
    }
}
