using System;

namespace StudentService.Data.Models
{
    /// <summary>
    /// Т.к. в EF Core нельзя неявно реализовать многие-ко-многим, то пришлось добавить
    /// </summary>
    public class StudentInGroup
    {
        public Guid StudentGuid { get; set; }

        public Guid GroupGuid { get; set; }

        public virtual Student Student { get; set; }

        public virtual Group Group { get; set; }
    }
}
