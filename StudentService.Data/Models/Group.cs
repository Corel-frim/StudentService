using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentService.Data.Models
{
    /// <summary>
    /// Сущность группы
    /// </summary>
    public class Group
    {
        public Group()
        {
            Students = new HashSet<StudentInGroup>();
        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Студенты, входящие в группу
        /// </summary>
        public virtual ICollection<StudentInGroup> Students { get; set; }

        /// <summary>
        /// Обычно через автомаппер такие маппинги делаются, но вдруг интересно будет
        /// </summary>
        /// <param name="group">Данные группы</param>
        public static explicit operator GroupListItem(Group group)
        {
            var result = new GroupListItem
            {
                Id = group.Id,
                Name = group.Name,
                StudentsCount = group.Students.Count
            };

            return result;
        }
    }
}
