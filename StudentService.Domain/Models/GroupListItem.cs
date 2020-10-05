using System;

namespace StudentService.Domain.Models
{
    /// <summary>
    /// Сущность для возврата списка групп
    /// </summary>
    public class GroupListItem
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество студентов в группе
        /// </summary>
        public int StudentsCount { get; set; }
    }
}
