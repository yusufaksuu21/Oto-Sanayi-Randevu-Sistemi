using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Areas.Admin.Pages.Customers;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _users;
    public IndexModel(ApplicationDbContext db, UserManager<ApplicationUser> users) => (_db, _users) = (db, users);
    public IList<CustomerRow> Customers { get; private set; } = new List<CustomerRow>();
    [BindProperty] public ResetInput Input { get; set; } = new();

    public class CustomerRow { public string Id { get; set; } = ""; public string? FullName { get; set; } public string? Email { get; set; } public string? Phone { get; set; } public int VehicleCount { get; set; } public int AppointmentCount { get; set; } }
    public class ResetInput { [Required] public string UserId { get; set; } = ""; [Required, StringLength(100, MinimumLength = 6)] public string NewPassword { get; set; } = ""; }

    public async Task OnGetAsync() => Customers = await GetRowsAsync();
    public async Task<IActionResult> OnPostResetPasswordAsync()
    {
        if (!ModelState.IsValid) { Customers = await GetRowsAsync(); return Page(); }
        var user = await _users.FindByIdAsync(Input.UserId);
        if (user == null || await _users.IsInRoleAsync(user, "Admin")) return NotFound();
        var token = await _users.GeneratePasswordResetTokenAsync(user);
        var result = await _users.ResetPasswordAsync(user, token, Input.NewPassword);
        if (!result.Succeeded) { foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description); Customers = await GetRowsAsync(); return Page(); }
        TempData["Success"] = "Musteri sifresi guncellendi.";
        return RedirectToPage();
    }
    private async Task<IList<CustomerRow>> GetRowsAsync() => await _db.Users.Where(u => !_db.UserRoles.Any(r => r.UserId == u.Id && _db.Roles.Any(role => role.Id == r.RoleId && role.Name == "Admin"))).Select(u => new CustomerRow { Id = u.Id, FullName = u.FullName, Email = u.Email, Phone = u.PhoneNumber, VehicleCount = _db.Vehicles.Count(v => v.OwnerId == u.Id), AppointmentCount = _db.Appointments.Count(a => a.CustomerId == u.Id) }).ToListAsync();
}
