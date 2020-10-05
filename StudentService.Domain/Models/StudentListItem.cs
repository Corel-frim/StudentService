using System;

namespace StudentService.Domain.Models
{
    /// <summary>
    /// Сущность студента для списка
    /// </summary>
    public class StudentListItem
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Необязательный уникальный идентификатор
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Список групп через запятую
        /// </summary>
        public string Groups { get; set; }
    }
}
