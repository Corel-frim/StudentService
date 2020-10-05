using StudentService.Domain.Enums;

namespace StudentService.Domain.Models
{
    /// <summary>
    /// Фильтрация списка студентов
    /// </summary>
    public class StudentListFilter
    {
        /// <summary>
        /// Пол
        /// </summary>
        public StudentSex? Sex { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Необязательный уникальный идентификатор
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Кол-во необходимых объектов
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Кол-во пропускаемых объектов
        /// </summary>
        public int Skip { get; set; }
    }
}
