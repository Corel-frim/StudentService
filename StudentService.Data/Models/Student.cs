using StudentService.Domain.Enums;
using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StudentService.Data.Models
{
    /// <summary>
    /// Сущность студента
    /// </summary>
    public class Student
    {
        public Student()
        {
            Groups = new HashSet<StudentInGroup>();
        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public StudentSex Sex { get; set; }

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
        /// Необязательный уникальный идентификатор, т.н. позывной
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Список групп, в которые входит студент
        /// </summary>
        public virtual ICollection<StudentInGroup> Groups { get; set; }

        /// <summary>
        /// Аналогично такому же в группах: вдруг интересно будет
        /// </summary>
        /// <param name="student"></param>
        public static explicit operator StudentListItem(Student student)
        {
            var result = new StudentListItem
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                MiddleName = student.MiddleName,
                Nickname = student.Nickname,
                Groups = string.Join(", ", student.Groups.Select(group => group.Group.Name))
            };

            return result;
        }
    }
}
