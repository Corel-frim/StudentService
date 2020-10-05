using StudentService.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace StudentService.Domain.Models
{
    /// <summary>
    /// Сущность студента для UI
    /// </summary>
    public class StudentView
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public StudentSex Sex { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        [StringLength(40)]
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [StringLength(40)]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Required]
        [StringLength(60)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Необязательный уникальный идентификатор
        /// </summary>
        [StringLength(16, MinimumLength = 6)]
        public string Nickname { get; set; }
    }
}
