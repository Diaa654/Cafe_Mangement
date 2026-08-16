
using Cafe_Mangement.Extentions;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Data.DataSeeding;

namespace Cafe_Management
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<CafeDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
            });
            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services
                .AddIdentity<User, IdentityRole<int>>()
                 .AddEntityFrameworkStores<CafeDbContext>();
            #endregion
            var app = builder.Build();


            #region Configure the HTTP request pipeline
            await app.MigrateDatabaseAsync();
            await app.SeedDataAsync();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
