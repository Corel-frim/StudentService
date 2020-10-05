using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentService.Data.Models;
using StudentService.Domain.Interfaces;
using StudentService.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentService.Data.Services
{
    /// <summary>
    /// Сервис работы с студентами
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly StudentContext _context;
        private readonly IMapper _mapper;

        public StudentService(StudentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Проверка необязательного уникального идентификатора
        /// </summary>
        /// <param name="nickname">Идентификатор</param>
        /// <returns>true, если идентификатор пустой или не занят</returns>
        public async Task<bool> CheckNickname(string nickname)
        {
            var isAvailable = string.IsNullOrEmpty(nickname) ||
                !await _context.Students.AnyAsync(s => s.Nickname.ToUpper() == nickname.ToUpper());

            return isAvailable;
        }

        /// <summary>
        /// Создание студента
        /// </summary>
        /// <param name="student">Данные создаваемого студента</param>
        /// <returns>Guid созданного студента</returns>
        public async Task<Guid> CreateStudent(StudentView student)
        {
            var entity = _mapper.Map<Student>(student);

            if(!await CheckNickname(entity.Nickname))
            {
                entity.Nickname = null;
            }

            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        /// <summary>
        /// Удаление студента по Guid
        /// </summary>
        /// <param name="guid">Guid студента</param>
        /// <returns>true, если студент был удален</returns>
        public async Task<bool> DeleteStudent(Guid guid)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == guid);

            if (student == default) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Получение данных студента по Guid
        /// </summary>
        /// <param name="guid">Guid студента</param>
        /// <returns>Данные студента</returns>
        public async Task<StudentView> GetStudent(Guid guid)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == guid);
            var result = _mapper.Map<StudentView>(student);

            return result;
        }

        /// <summary>
        /// Получение списка студентов с возможностью пагинации и фильтрации
        /// </summary>
        /// <param name="filter">Фильтры поиска</param>
        /// <returns>Список студентов и количество доступных студентов с данными фильтрами</returns>
        public async Task<StudentList> GetStudents(StudentListFilter filter)
        {
            var result = new StudentList();
            var filteredList = FilterStudents(filter);

            result.Count = filteredList.Count();

            result.Students = await filteredList
                .Paging(filter.Take, filter.Skip)
                .Select(s => (StudentListItem)s)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Обновление данных клиента студента
        /// </summary>
        /// <param name="student">Обновленные данные клиента</param>
        /// <returns>true, если данные были обновлены</returns>
        public async Task<bool> UpdateStudent(StudentView student)
        {
            if (student.Id == Guid.Empty || !_context.Students.Any(s => s.Id == student.Id))
            {
                return false;
            }

            var entity = _mapper.Map<Student>(student);

            if(!await CheckNickname(entity.Nickname))
            {
                entity.Nickname = null;
            }

            _context.Students.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Начальная фильтрация студентов
        /// </summary>
        /// <param name="filter">Фильтры</param>
        /// <returns>Отфильтрованный список студентов</returns>
        private IQueryable<Student> FilterStudents(StudentListFilter filter)
        {
            IQueryable<Student> query = _context.Students
                // В EF Core LazyLoading организована через прокси, так что при обращении извне нужен Include
                .Include(s => s.Groups)
                .ThenInclude(sg => sg.Group);

            foreach (var condition in filter.GetConditionExpressions())
            {
                query = query.Where(condition);
            }

            return query;
        }
    }
}
