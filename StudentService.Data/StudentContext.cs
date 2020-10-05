using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentService.Data.Models;

namespace StudentService.Data
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<StudentInGroup> StudentsInGroups { get; set; }

        public StudentContext()
        {
            Database.EnsureCreated();
        }

        public StudentContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Пусть и на прокси, но все же в целом помогает
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasKey(student => student.Id);
            modelBuilder.Entity<Group>().HasKey(group => group.Id);
            modelBuilder.Entity<StudentInGroup>().HasKey(sg => new { sg.StudentGuid, sg.GroupGuid });

            // Опять же, в EF Core нет неявного многие-ко-многим
            modelBuilder.Entity<StudentInGroup>().HasOne(sg => sg.Group).WithMany(g => g.Students).HasForeignKey(sg => sg.GroupGuid);
            modelBuilder.Entity<StudentInGroup>().HasOne(sg => sg.Student).WithMany(g => g.Groups).HasForeignKey(sg => sg.StudentGuid);
        }
    }
}
