using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Data.DataSeeding
{
    public class DataInitializer : IDataInitializer
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ILogger<DataInitializer> _logger;
        private readonly CafeDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public DataInitializer(RoleManager<IdentityRole<int>> roleManager,
            ILogger<DataInitializer> logger, CafeDbContext dbContext, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task InitializeDataAsync()
        {
            try
            {
                await SeedRolesAsync();
                await SeedAdminAsync();
                await SeedWaitersAsync();
                await SeedBaristaAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing data.");
            }
        }

        private async Task SeedRolesAsync()
        {
            var roles = Enum.GetNames(typeof(AppRoles));
            foreach (var role in roles)
            {
                var exists = await _roleManager.RoleExistsAsync(role);
                if (!exists)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }
        }
        
        private async Task SeedAdminAsync()
        {
            var adminEmail = "admin99@gmail.com";

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    FullName="AbuElNaked",
                    UserName = "adminuser",
                    Email = adminEmail,
                    EmailConfirmed = true,

                    PhoneNumber = "01000000000"
                };

                var result = await _userManager.CreateAsync(newAdmin, "P@ssw0rd");

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(newAdmin, nameof(AppRoles.Admin));
                    _logger.LogInformation("تم إنشاء حساب الأدمن الافتراضي بنجاح.");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"فشل في إنشاء حساب الأدمن: {errors}");
                }
            }
        }

        private async Task SeedWaitersAsync()
        {
            var waiters = new List<(string FullName, string UserName, string Email, string PhoneNumber)>
    {
        ("Mohamed Ali", "WaiterUser1", "MohamedAli@gmail.com", "01000000001"),
        ("Ahmed Hassan", "WaiterUser2", "AhmedHassan@gmail.com", "01000000002"),
        ("Mahmoud Ali", "WaiterUser3", "MahmoudAli@gmail.com", "01000000003"),
        ("Omar Mohamed", "WaiterUser4", "OmarMohamed@gmail.com", "01000000004")
    };

            foreach (var waiter in waiters)
            {
                var existingWaiter = await _userManager.FindByEmailAsync(waiter.Email);

                if (existingWaiter != null)
                    continue;

                var newWaiter = new User
                {
                    FullName = waiter.FullName,
                    UserName = waiter.UserName,
                    Email = waiter.Email,
                    EmailConfirmed = true,
                    PhoneNumber = waiter.PhoneNumber
                };

                var result = await _userManager.CreateAsync(newWaiter, "P@ssw0rd");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(
                        newWaiter,
                        nameof(AppRoles.Waiter));

                    _logger.LogInformation(
                        "تم إنشاء حساب الويتر {UserName} بنجاح.",
                        newWaiter.UserName);
                }
                else
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description));

                    _logger.LogError(
                        "فشل في إنشاء حساب الويتر {UserName}: {Errors}",
                        newWaiter.UserName,
                        errors);
                }
            }
        }

        private async Task SeedBaristaAsync()
        {
            var BaristaEmail = "Barista@gmail.com";

            var BaristaUser = await _userManager.FindByEmailAsync(BaristaEmail);

            if (BaristaUser == null)
            {
                var newBarista = new User
                {
                    FullName = "Barista",
                    UserName = "Baristauser",
                    Email = BaristaEmail,
                    EmailConfirmed = true,

                    PhoneNumber = "01000000000"
                };

                var result = await _userManager.CreateAsync(newBarista, "P@ssw0rd");

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(newBarista, nameof(AppRoles.Barista));
                    _logger.LogInformation("تم إنشاء حساب الباريستا الافتراضي بنجاح.");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"فشل في إنشاء حساب الباريستا: {errors}");
                }
            }
        }
    }
}
