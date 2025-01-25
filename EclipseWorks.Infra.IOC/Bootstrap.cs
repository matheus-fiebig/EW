using EclipseWorks.Application.Projects.Handlers;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain.Histories.Interfaces;
using EclipseWorks.Domain.Projects.Interfaces;
using EclipseWorks.Domain.Reports.Interfaces;
using EclipseWorks.Domain.Tasks.Interfaces;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;
using EclipseWorks.Infra.Data;
using EclipseWorks.Infra.Data.Context;
using EclipseWorks.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace EclipseWorks.Infra.IOC
{
    public static class Bootstrap
    {
        public static void InjectDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommandHandler).Assembly));

            services.AddScoped<IQueryTaskRepository, TaskRepository>();
            services.AddScoped<ICommandTaskRepository, TaskRepository>();
            services.AddScoped<IQueryProjectRepository, ProjectRepository>();
            services.AddScoped<ICommandProjectRepository, ProjectRepository>();
            services.AddScoped<IQueryUserRepository, UserRepository>();
            services.AddScoped<ICommandHistoryRepository, HistoryRepository>();
            services.AddScoped<IQueryReportRepository, ReportRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<MainContext>(x =>
                x.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("Main")!)
            );
        }

        public static IHost Migrate<T>(this IHost webHost) where T : MainContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();

                    if(!db.Set<UserEntity>().Any())
                    {
                        db.Add(UserEntity.Create("Davi", "gerente"));
                        db.Add(UserEntity.Create("Brito", "usuario"));
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.InnerException?.Message);
                    throw;
                }
            }

            return webHost;
        }
    }
}
