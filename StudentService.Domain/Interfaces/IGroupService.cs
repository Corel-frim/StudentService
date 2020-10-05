using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentService.Domain.Interfaces
{
        /// <summary>
        /// Сервис работы с группами
        /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Получение списка групп с возможностью фильтрации по имени группы
        /// </summary>
        /// <param name="name">Имя группы</param>
        /// <returns>Список групп для отображения</returns>
        Task<List<GroupListItem>> GetGroups(string name);

        /// <summary>
        /// Получение данных группы по Guid
        /// </summary>
        /// <param name="guid">Guid группы</param>
        /// <returns>Данные группы</returns>
        Task<GroupView> GetGroup(Guid guid);

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="guid">Guid удаляемой группы</param>
        /// <returns>true, если группа удалена</returns>
        Task<bool> DeleteGroup(Guid guid);

        /// <summary>
        /// Обновление данных группы
        /// </summary>
        /// <param name="group">Обновленные данные группы</param>
        /// <returns>true, если данные группы были обновлены</returns>
        Task<bool> UpdateGroup(GroupView group);

        /// <summary>
        /// Создание группы
        /// </summary>
        /// <param name="group">Данные группы</param>
        /// <returns>Guid созданной группы</returns>
        Task<Guid> CreateGroup(GroupView group);
    }
}
