using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Pages.Vehicles;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [BindProperty]
    public Vehicle Vehicle { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var vehicle = await GetOwnedVehicleAsync(id);
        if (vehicle == null) return NotFound();
        Vehicle = vehicle;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var vehicle = await GetOwnedVehicleAsync(id);
        if (vehicle == null) return NotFound();

        var hasAppointments = await _db.Appointments.AnyAsync(a => a.VehicleId == vehicle.Id);
        if (hasAppointments)
        {
            TempData["Error"] = "Bu araca bağlı randevular var. Önce randevuları iptal edin.";
            return RedirectToPage("./Index");
        }

        _db.Vehicles.Remove(vehicle);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Araç silindi.";
        return RedirectToPage("./Index");
    }

    private async Task<Vehicle?> GetOwnedVehicleAsync(int? id)
    {
        if (id == null) return null;
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return null;

        return await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id && v.OwnerId == user.Id);
    }
}
