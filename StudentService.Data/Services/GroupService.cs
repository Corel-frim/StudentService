using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentService.Data.Models;
using StudentService.Domain;
using StudentService.Domain.Interfaces;
using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentService.Data.Services
{
    /// <summary>
    /// Сервис работы с группами
    /// </summary>
    public class GroupService : IGroupService
    {
        private readonly StudentContext _context;
        private readonly IMapper _mapper;

        public GroupService(StudentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Создание группы
        /// </summary>
        /// <param name="group">Данные группы</param>
        /// <returns>Guid созданной группы</returns>
        public async Task<Guid> CreateGroup(GroupView group)
        {
            var entity = _mapper.Map<Group>(group);

            await _context.Groups.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="guid">Guid удаляемой группы</param>
        /// <returns>true, если группа удалена</returns>
        public async Task<bool> DeleteGroup(Guid guid)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(s => s.Id == guid);

            if (group == default) return false;

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Получение данных группы по Guid
        /// </summary>
        /// <param name="guid">Guid группы</param>
        /// <returns>Данные группы</returns>
        public async Task<GroupView> GetGroup(Guid guid)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(s => s.Id == guid);
            var result = _mapper.Map<GroupView>(group);

            return result;
        }

        /// <summary>
        /// Получение списка групп с возможностью фильтрации по имени группы
        /// </summary>
        /// <param name="name">Имя группы</param>
        /// <returns>Список групп для отображения</returns>
        public async Task<List<GroupListItem>> GetGroups(string name)
        {
            IQueryable<Group> groups = _context.Groups;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.ToUpper();

                // Более точная фильтрация, если это возможно
                groups = groups
                    .Where(g => g.Name.ToUpper().Contains(name))
                    .ThenWhere(g => g.Name.ToUpper().StartsWith(name))
                    .ThenWhere(g => g.Name.ToUpper().Equals(name));
            }

            var result = await groups
                // В EF Core LazyLoading организована через прокси, так что при обращении извне нужен Include
                .Include(g => g.Students)
                .Select(g => (GroupListItem)g)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Обновление данных группы
        /// </summary>
        /// <param name="group">Обновленные данные группы</param>
        /// <returns>true, если данные группы были обновлены</returns>
        public async Task<bool> UpdateGroup(GroupView group)
        {
            if (group.Id == Guid.Empty || !_context.Groups.Any(s => s.Id == group.Id))
            {
                return false;
            }

            var entity = _mapper.Map<Group>(group);

            _context.Groups.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
