using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentService.Domain.Enums;
using StudentService.Domain.Interfaces;
using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IStudentsInGroupsService _studentsInGroupsService;

        public StudentController(IStudentService studentService, IStudentsInGroupsService studentsInGroupsService)
        {
            _studentService = studentService;
            _studentsInGroupsService = studentsInGroupsService;
        }

        /// <summary>
        /// Получение списка студентов с пагинацией и возможностью фильтрации
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="middleName">Отчество</param>
        /// <param name="sex">Пол</param>
        /// <param name="nickname">Уникальный идентификатор</param>
        /// <param name="groupName">Название группы</param>
        /// <param name="skip">Количество пропускаемых записей</param>
        /// <param name="take">Количество необходимых записей</param>
        /// <returns>Список студентов и их количество</returns>
        [AllowAnonymous]
        [HttpGet("list")]
        [ProducesResponseType(typeof(StudentList), 200)]
        public async Task<IActionResult> GetAllStudents(
            [FromQuery] string firstName = "",
            string lastName = "",
            string middleName = "",
            StudentSex? sex = null,
            string nickname = "",
            string groupName = "",
            int skip = 0,
            int take = 0)
        {
            var filter = new StudentListFilter
            {
                GroupName = groupName,
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Nickname = nickname,
                Sex = sex,
                Skip = skip,
                Take = take
            };

            var result = await _studentService.GetStudents(filter);

            return new JsonResult(result);
        }

        /// <summary>
        /// Добавление студента в группу
        /// </summary>
        /// <param name="student">Guid студента, добавляемого в группу</param>
        /// <param name="group">Guid группы, в которую добавляют студента</param>
        /// <returns>true, если студент добавлен в группу</returns>
        [Authorize]
        [HttpPost("{student}/group/{group}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddStudentInGroup([FromRoute] Guid student, Guid group)
        {
            var result = await _studentsInGroupsService.AddStudentInGroup(student, group);

            return new JsonResult(result);
        }

        /// <summary>
        /// Удаление студента из группы
        /// </summary>
        /// <param name="student">Guid студента, удаляемого из группы</param>
        /// <param name="group">Guid группы, из которой удаляют студента</param>
        /// <returns>true, если студент удален из группы</returns>
        [Authorize]
        [HttpDelete("{student}/group/{group}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteStudentFromGroup([FromRoute] Guid student, Guid group)
        {
            var result = await _studentsInGroupsService.DeleteStudentFromGroup(student, group);

            return new JsonResult(result);
        }

        /// <summary>
        /// Получение данных студента по Guid
        /// </summary>
        /// <param name="guid">Guid студента</param>
        /// <returns>Полные данные студента</returns>
        [AllowAnonymous]
        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(StudentView), 200)]
        public async Task<IActionResult> GetStudent([FromRoute] Guid guid)
        {
            var result = await _studentService.GetStudent(guid);

            return new JsonResult(result);
        }

        /// <summary>
        /// Проверка идентификатора студента на уникальность
        /// </summary>
        /// <param name="nickname">Идентификатор студента</param>
        /// <returns>true, если идентификатор пустой или не занят</returns>
        [Authorize]
        [HttpGet("check/{nickname}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CheckNickname([FromRoute] string nickname)
        {
            var result = !string.IsNullOrEmpty(nickname) && nickname.Length >= 6 && nickname.Length <= 16
                ? false
                : await _studentService.CheckNickname(nickname);

            return new JsonResult(result);
        }

        /// <summary>
        /// Удаление студента
        /// </summary>
        /// <param name="guid">Guid удаляемого студента</param>
        /// <returns>true, если студент удален</returns>
        [Authorize]
        [HttpDelete("{guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid guid)
        {
            var result = await _studentService.DeleteStudent(guid);

            return new JsonResult(result);
        }

        /// <summary>
        /// Создание студента
        /// </summary>
        /// <param name="student">Данные студента</param>
        /// <returns>true, если студент создан</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CreateStudent([FromBody] StudentView student)
        {
            var result = await _studentService.CreateStudent(student);

            return new JsonResult(result);
        }

        /// <summary>
        /// Обновление данных студента
        /// </summary>
        /// <param name="student">Обновленные данные студента</param>
        /// <returns>true, если данные студента обновлены</returns>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentView student)
        {
            var result = await _studentService.UpdateStudent(student);

            return new JsonResult(result);
        }
    }
}
