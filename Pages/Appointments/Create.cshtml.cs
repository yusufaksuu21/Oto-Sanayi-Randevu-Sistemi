using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;
using SanayiRandevu.Services;

namespace SanayiRandevu.Pages.Appointments;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppointmentAvailabilityService _availability;

    public CreateModel(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        AppointmentAvailabilityService availability)
    {
        _db = db;
        _userManager = userManager;
        _availability = availability;
    }

    [BindProperty]
    public AppointmentInput Input { get; set; } = new();

    public SelectList ServiceOptions { get; set; } = null!;
    public SelectList VehicleOptions { get; set; } = null!;
    public bool HasVehicles { get; set; }

    public class AppointmentInput
    {
        [Required(ErrorMessage = "Hizmet seçiniz.")]
        [Display(Name = "Hizmet")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Araç seçiniz.")]
        [Display(Name = "Araç")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Tarih seçiniz.")]
        [DataType(DataType.Date)]
        [Display(Name = "Tarih")]
        public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "Saat seçiniz.")]
        [Display(Name = "Saat")]
        public string TimeSlot { get; set; } = "";

        [StringLength(1000)]
        [Display(Name = "Mesajınız (opsiyonel)")]
        public string? Notes { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadSelectListsAsync();
        if (!HasVehicles)
            TempData["Error"] = "Randevu alabilmek için önce bir araç eklemelisiniz.";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadSelectListsAsync();

        if (Input.AppointmentDate.Date < DateTime.Today)
            ModelState.AddModelError("Input.AppointmentDate", "Geçmiş bir tarih seçilemez.");

        if (!await _availability.IsSlotAvailableAsync(Input.AppointmentDate, Input.TimeSlot))
            ModelState.AddModelError("Input.TimeSlot", "Seçilen saat artık müsait değil.");

        if (!ModelState.IsValid) return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var ownsVehicle = await _db.Vehicles.AnyAsync(v => v.Id == Input.VehicleId && v.OwnerId == user.Id);
        if (!ownsVehicle)
        {
            ModelState.AddModelError("Input.VehicleId", "Geçersiz araç seçimi.");
            return Page();
        }

        _db.Appointments.Add(new Appointment
        {
            CustomerId = user.Id,
            VehicleId = Input.VehicleId,
            ServiceId = Input.ServiceId,
            AppointmentDate = Input.AppointmentDate.Date,
            TimeSlot = Input.TimeSlot,
            Notes = string.IsNullOrWhiteSpace(Input.Notes) ? null : Input.Notes.Trim(),
            Status = AppointmentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        TempData["Success"] = "Randevunuz alındı! Onay için bekleyin, size geri bildirim yapılacaktır.";
        return RedirectToPage("./Index");
    }

    public async Task<JsonResult> OnGetAvailableSlotsAsync(string date)
    {
        if (!DateTime.TryParse(date, out var parsed))
            return new JsonResult(Array.Empty<string>());

        var slots = await _availability.GetAvailableTimeSlotsAsync(parsed);
        return new JsonResult(slots);
    }

    private async Task LoadSelectListsAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return;

        var services = await _db.Services.OrderBy(s => s.Name).ToListAsync();
        var vehicles = await _db.Vehicles.Where(v => v.OwnerId == user.Id).OrderBy(v => v.Plate).ToListAsync();

        ServiceOptions = new SelectList(services, "Id", "Name");
        VehicleOptions = new SelectList(vehicles, "Id", "Plate");
        HasVehicles = vehicles.Count > 0;
    }
}
