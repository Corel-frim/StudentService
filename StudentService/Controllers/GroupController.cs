using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentService.Domain.Interfaces;
using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IStudentsInGroupsService _studentsInGroupsService;

        public GroupController(IGroupService groupService, IStudentsInGroupsService studentsInGroupsService)
        {
            _groupService = groupService;
            _studentsInGroupsService = studentsInGroupsService;
        }

        /// <summary>
        /// Получение списка групп с возможностью фильтрации по имени группы
        /// </summary>
        /// <param name="name">Имя группы или ее часть для поиска</param>
        /// <returns>Список групп</returns>
        [AllowAnonymous]
        [HttpGet("list")]
        [ProducesResponseType(typeof(List<GroupListItem>), 200)]
        public async Task<IActionResult> GetGroupList([FromQuery] string name = null)
        {
            var result = await _groupService.GetGroups(name);

            return new JsonResult(result);
        }

        /// <summary>
        /// Добавление студента в группу. Функционал дублируется с
        /// <see cref="StudentController">контроллера студентов</see> для возможной оптимизации на фронте
        /// </summary>
        /// <param name="group">Guid группы, в которую будет добавлен студент</param>
        /// <param name="student">Guid студента, который будет добавлен в группу</param>
        /// <returns>true, если студент добавлен в группу</returns>
        [Authorize]
        [HttpPost("{group}/student/{student}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddStudentInGroup([FromRoute] Guid group, Guid student)
        {
            var result = await _studentsInGroupsService.AddStudentInGroup(student, group);

            return new JsonResult(result);
        }


        /// <summary>
        /// Удаление студента из группы. Функционал дублируется с
        /// <see cref="StudentController">контроллера студентов</see> для возможной оптимизации на фронте
        /// </summary>
        /// <param name="group">Guid группы, из которой будет удален студент</param>
        /// <param name="student">Guid студента, который будет удален из группы</param>
        /// <returns>true, если студент удален из группы</returns>
        [Authorize]
        [HttpDelete("{group}/student/{student}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteStudentFromGroup([FromRoute] Guid group, Guid student)
        {
            var result = await _studentsInGroupsService.DeleteStudentFromGroup(student, group);

            return new JsonResult(result);
        }

        /// <summary>
        /// Получение данных группы по Guid
        /// </summary>
        /// <param name="guid">Guid группы</param>
        /// <returns>Полные данные группы</returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(GroupView), 200)]
        public async Task<IActionResult> GetGroup(Guid guid)
        {
            var result = await _groupService.GetGroup(guid);

            return new JsonResult(result);
        }

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="guid">Guid группы</param>
        /// <returns>true, если группа удалена</returns>
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteGroup([FromQuery] Guid guid)
        {
            var result = await _groupService.DeleteGroup(guid);

            return new JsonResult(result);
        }

        /// <summary>
        /// Создание группы
        /// </summary>
        /// <param name="guid">Данные группы</param>
        /// <returns>true, если группа создана</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateGroup([FromBody] GroupView group)
        {
            var result = await _groupService.CreateGroup(group);

            return new JsonResult(result);
        }

        /// <summary>
        /// Обновление данных группы
        /// </summary>
        /// <param name="guid">Данные группы</param>
        /// <returns>true, если группа обновлена</returns>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdateGroup([FromBody] GroupView group)
        {
            var result = await _groupService.UpdateGroup(group);

            return new JsonResult(result);
        }
    }
}
