using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Areas.Admin.Pages.Appointments;

[Authorize(Roles = "Admin")]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public DetailsModel(ApplicationDbContext db) => _db = db;

    public Appointment Appointment { get; set; } = null!;

    [BindProperty]
    public AdminActionInput Input { get; set; } = new();

    public class AdminActionInput
    {
        public int AppointmentId { get; set; }

        [StringLength(1000)]
        [Display(Name = "Geri Bildirim Mesaji")]
        public string? AdminReply { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var appointment = await LoadAppointmentAsync(id);
        if (appointment == null) return NotFound();

        Appointment = appointment;
        Input = new AdminActionInput
        {
            AppointmentId = appointment.Id,
            AdminReply = appointment.AdminReply
        };
        return Page();
    }

    public async Task<IActionResult> OnPostConfirmAsync()
    {
        return await UpdateStatusAsync(AppointmentStatus.Confirmed, "Randevu onaylandi.");
    }

    public async Task<IActionResult> OnPostCompleteAsync()
    {
        return await UpdateStatusAsync(AppointmentStatus.Completed, "Randevu tamamlandi olarak isaretlendi.");
    }

    public async Task<IActionResult> OnPostCancelAsync()
    {
        return await UpdateStatusAsync(AppointmentStatus.Rejected, "Randevu reddedildi.");
    }

    public async Task<IActionResult> OnPostReplyAsync()
    {
        var appointment = await LoadAppointmentAsync(Input.AppointmentId);
        if (appointment == null) return NotFound();

        appointment.AdminReply = string.IsNullOrWhiteSpace(Input.AdminReply) ? null : Input.AdminReply.Trim();
        appointment.AdminReplyAt = appointment.AdminReply == null ? null : DateTime.UtcNow;
        await _db.SaveChangesAsync();

        TempData["Success"] = "Geri bildirim mesaji kaydedildi.";
        return RedirectToPage(new { id = appointment.Id });
    }

    private async Task<IActionResult> UpdateStatusAsync(AppointmentStatus status, string message)
    {
        var appointment = await LoadAppointmentAsync(Input.AppointmentId);
        if (appointment == null) return NotFound();

        appointment.Status = status;
        if (!string.IsNullOrWhiteSpace(Input.AdminReply))
        {
            appointment.AdminReply = Input.AdminReply.Trim();
            appointment.AdminReplyAt = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();

        TempData["Success"] = message;
        return RedirectToPage(new { id = appointment.Id });
    }

    private async Task<Appointment?> LoadAppointmentAsync(int? id)
    {
        if (id == null) return null;
        return await _db.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Vehicle)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
