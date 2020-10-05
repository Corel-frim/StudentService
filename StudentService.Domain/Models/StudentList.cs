using System.Collections.Generic;

namespace StudentService.Domain.Models
{
    /// <summary>
    /// Сущность с списком студентов
    /// </summary>
    public class StudentList
    {
        /// <summary>
        /// Список студентов для отображения в таблице
        /// </summary>
        public List<StudentListItem> Students { get; set; }

        /// <summary>
        /// Количество студентов, соответствующее фильтрам
        /// </summary>
        public int Count { get; set; }
    }
}
