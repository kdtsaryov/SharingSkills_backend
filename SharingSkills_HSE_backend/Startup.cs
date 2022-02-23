using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SharingSkills_HSE_backend.Models;
using System;

namespace SharingSkills_HSE_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// ����������� ��������
        /// </summary>
        /// <param name="services">�������</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // ����������� JSON �������������
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            // ����������� ������������
            services.AddControllers();
            // ����������� ��������� ���� ������
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                services.AddDbContext<SharingSkillsContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SharingSkillsContextProd")));
            else
                services.AddDbContext<SharingSkillsContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SharingSkillsContext")));
            services.BuildServiceProvider().GetService<SharingSkillsContext>().Database.Migrate();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
