using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace StudentService.Controllers
{
    /// <summary>
    /// Контроллер для авторизации и аутентификации
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Аутентификация через JWT
        /// </summary>
        /// <returns>Токен</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Authenticate()
        {
            var accessToken = await GenerateTokenAsync();

            // Добавление в куки в целом для тестирования через сваггер
            HttpContext.Response.Cookies.Append(
                _configuration["Jwt:Cookie"],
                accessToken,
                // Длительность чисто для примера
                new CookieOptions { MaxAge = TimeSpan.FromDays(30) });

            return Ok(new { access_token = accessToken });
        }

        /// <summary>
        /// Вынес отдельно генерацию токена, чтобы можно было использовать в дальнейшем для авторизации
        /// </summary>
        /// <returns>Токен</returns>
        private Task<string> GenerateTokenAsync()
        {
            var secretBytes = Encoding.UTF8.GetBytes(_configuration["JwtSecret"]);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var credentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                        // Парсинг класса из конфигов не помешает, но пока не в приоритете
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        notBefore: DateTime.Now,
                        // Длительность чисто для примера
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(accessToken);
        }
    }
}
