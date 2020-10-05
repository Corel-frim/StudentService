using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentService.Data;
using StudentService.Data.Mappers;
using StudentService.Data.Services;
using StudentService.Domain.Interfaces;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using StudentServiceLogic = StudentService.Data.Services.StudentService;

namespace StudentService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private const string _swaggerVersion = "v1";
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Добавил использование JWT
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes(Configuration["JwtSecret"]);
                    var key = new SymmetricSecurityKey(secretBytes);

                    // Получение токена из куки, нужно при тестировании через сваггер
                    config.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.ContainsKey(Configuration["Jwt:Cookie"]))
                            {
                                context.Token = context.Request.Cookies[Configuration["Jwt:Cookie"]];
                            }

                            return Task.CompletedTask;
                        }
                    };

                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = key
                    };
                });

            // Вынес добавление ConnectionString сюда, чтобы можно было в тестах создавать копию с InMemory базой
            services.AddDbContext<StudentContext>(options =>
            {
                options.UseSqlServer(Configuration["DbConnectionString"]);
            });

            services.AddControllersWithViews();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc(_swaggerVersion, new OpenApiInfo
                {
                    Title = "Service with student and group info",
                    Version = _swaggerVersion
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();

            // Хоть профиля сейчас и два, но зарегать достаточно по одному с проекта
            services.AddAutoMapper(typeof(StudentProfile));

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddTransient(typeof(StudentContext));
            services.AddTransient<IStudentService, StudentServiceLogic>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IStudentsInGroupsService, StudentsInGroupsService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            // На всякий добавил правила для куки
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseAuthentication();
            // Авторизация на будущее
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{_swaggerVersion}/swagger.json", "StudentService API");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = Path.Join(env.ContentRootPath, "ClientApp");

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
