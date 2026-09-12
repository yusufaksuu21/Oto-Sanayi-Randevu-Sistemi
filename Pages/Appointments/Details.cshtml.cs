using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Pages.Appointments;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public DetailsModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public Appointment Appointment { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var appointment = await GetOwnedAppointmentAsync(id);
        if (appointment == null) return NotFound();
        Appointment = appointment;
        return Page();
    }

    public async Task<IActionResult> OnPostCancelAsync(int id)
    {
        var appointment = await GetOwnedAppointmentAsync(id);
        if (appointment == null) return NotFound();

        if (appointment.Status is AppointmentStatus.Completed or AppointmentStatus.Cancelled)
        {
            TempData["Error"] = "Bu randevu iptal edilemez.";
            return RedirectToPage(new { id });
        }

        var appointmentStart = appointment.AppointmentDate.Date + TimeSpan.Parse(appointment.TimeSlot);
        if (appointmentStart <= DateTime.Now.AddHours(24))
        {
            TempData["Error"] = "Randevular baslangic saatinden en az 24 saat once iptal edilebilir.";
            return RedirectToPage(new { id });
        }

        appointment.Status = AppointmentStatus.Cancelled;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Randevunuz iptal edildi.";
        return RedirectToPage("./Index");
    }

    private async Task<Appointment?> GetOwnedAppointmentAsync(int? id)
    {
        if (id == null) return null;
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return null;

        return await _db.Appointments
            .Include(a => a.Vehicle)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id && a.CustomerId == user.Id);
    }
}
