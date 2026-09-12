using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;
using SanayiRandevu.Services;

var builder = WebApplication.CreateBuilder(args);

// SQLite baùlantù dizesi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=otosanayi.db";
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

// Identity yapùlandùrmasù
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Admin", "/", "Admin");
});

// Mùsaitlik servisini DI'ye ekle (Scoped)
builder.Services.AddScoped<AppointmentAvailabilityService>();

var app = builder.Build();

// Uygulama baùùnda DB migration ve seed iùlemlerini ùalùùtùr
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.InitializeAsync(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
