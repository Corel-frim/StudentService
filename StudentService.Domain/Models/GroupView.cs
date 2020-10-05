using System;
using System.ComponentModel.DataAnnotations;

namespace StudentService.Domain.Models
{
    /// <summary>
    /// Группа
    /// </summary>
    public class GroupView
    {
        public GroupView()
        {
            Id = Guid.Empty;
        }

        public Guid Id { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
    }
}
