
using Cafe_Management.CustomMiddleWares;
using Cafe_Mangement.Extentions;
using Domain.Contracts;
using Domain.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Data.DataSeeding;
using Persistence.Repositories;
using Presentation.Hubs;
using Service;
using ServiceAbstraction;
using StackExchange.Redis;
using System.Text;

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
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManger, ServiceManger>();
            builder.Services.AddAutoMapper(typeof(ServicesAssemblyReference).Assembly);
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IFcmService, FcmService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IDashboardNotificationService, DashboardNotificationService>();
            var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConnectionString!)
            );
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSignalR", policy =>
                {
                    policy.AllowAnyMethod()
                          .AllowAnyHeader()
                          .SetIsOriginAllowed(origin => true) // السطر ده هو السحر اللي بيسمح للـ null أو أي دومين
                          .AllowCredentials(); // السطر ده إجباري عشان SignalR يشتغل
                });
            });
            builder.Services
                .AddIdentity<User, IdentityRole<int>>()
                 .AddEntityFrameworkStores<CafeDbContext>();
            builder.Services.AddAuthentication(Option =>
            {
                Option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Option =>
            {
                Option.SaveToken = true;
                Option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!)),
                };

            });
            var firebaseKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "Firebase", "cafemanagement-592bb-firebase-adminsdk-fbsvc-fdf35b2e1e.json");

            if (File.Exists(firebaseKeyPath))
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(firebaseKeyPath)
                    });
                    Console.WriteLine("Firebase initialized successfully!");
                }
            }
            else
            {
                Console.WriteLine("WARNING: Firebase JSON file is missing! Notifications will not work.");
            }

            #endregion
            var app = builder.Build();


            #region Configure the HTTP request pipeline
            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            await app.MigrateDatabaseAsync();
            await app.SeedDataAsync();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseCors("AllowSignalR");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<AdminDashboardHub>("/hubs/adminDashboard");
            #endregion

            app.Run();
        }
    }
}
