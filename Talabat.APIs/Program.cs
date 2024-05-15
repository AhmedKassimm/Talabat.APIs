using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services Works With DI
            // Add services to the container.

            builder.Services.AddControllers();//add service asp web apis
            //---------------------------------------------------------------------
            //---DataBases
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            //---------------------------------------------------------------------
            //Extention Services
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddSwaggerSevices();

            #endregion

            var app = builder.Build();

            #region Update-database inside Main
            //Explicitly = manual
            var scope = app.Services.CreateScope();//Services Scoped
            var services = scope.ServiceProvider;//DI
            //LoggerFactory
            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>();//Ask Clr to Create Object from Store Context Explicitly
                await dbContext.Database.MigrateAsync();//update-database
                await StoreContextSeed.SeedAsync(dbContext);

                var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync();//update-database

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContext.SeedUsersAsync(userManager);

            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex,"an error occured during apply the migration");
            }
            #endregion

            #region Configure request into Piplines
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();//Controller
            #endregion

            app.Run();
        }
    }
}