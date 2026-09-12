using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Models;

namespace SanayiRandevu.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await db.Database.MigrateAsync();

            var roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var adminEmail = configuration["SeedAdmin:Email"];
            var adminPassword = configuration["SeedAdmin:Password"];
            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        FullName = "Dükkan Sahibi"
                    };

                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
                else if (!await userManager.IsInRoleAsync(admin, "Admin"))
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            if (!await db.WorkingHours.AnyAsync())
            {
                var workingHours = new List<WorkingHour>();
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    workingHours.Add(new WorkingHour
                    {
                        DayOfWeek = day,
                        IsClosed = day == DayOfWeek.Sunday,
                        OpenTime = TimeSpan.FromHours(9),
                        CloseTime = TimeSpan.FromHours(18)
                    });
                }

                db.WorkingHours.AddRange(workingHours);
                await db.SaveChangesAsync();
            }

            if (!await db.Services.AnyAsync())
            {
                var services = new List<Service>
                {
                    new Service { Name = "Yağ Değişimi", Description = "Motor yağı ve filtre değişimi", DurationMinutes = 30, Price = 1000m },
                    new Service { Name = "Lastik Değişimi", Description = "Lastik değiştirme ve balans", DurationMinutes = 45, Price = 600m },
                    new Service { Name = "Genel Bakım", Description = "Periyodik genel bakım", DurationMinutes = 90, Price = 2400m },
                    new Service { Name = "Fren Kontrolü", Description = "Fren sistemi kontrolü ve küçük ayarlar", DurationMinutes = 30, Price = 800m },
                    new Service { Name = "Arıza Tespiti", Description = "Elektronik arıza teşhisi", DurationMinutes = 60, Price = 700m }
                };

                db.Services.AddRange(services);
                await db.SaveChangesAsync();
            }
        }
    }
}
