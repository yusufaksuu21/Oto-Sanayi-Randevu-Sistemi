using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Areas.Admin.Pages.Appointments;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public IndexModel(ApplicationDbContext db) => _db = db;

    public IList<Appointment> Appointments { get; set; } = new List<Appointment>();

    [BindProperty(SupportsGet = true)]
    public string? StatusFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    [DataType(DataType.Date)]
    public DateTime? DateFilter { get; set; }

    public int PendingCount { get; set; }
    public int ConfirmedCount { get; set; }
    public int TodayCount { get; set; }

    public async Task OnGetAsync()
    {
        var query = _db.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Vehicle)
            .Include(a => a.Service)
            .AsQueryable();

        if (!string.IsNullOrEmpty(StatusFilter) && Enum.TryParse<AppointmentStatus>(StatusFilter, out var status))
            query = query.Where(a => a.Status == status);

        if (DateFilter.HasValue)
            query = query.Where(a => a.AppointmentDate.Date == DateFilter.Value.Date);

        Appointments = await query
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.TimeSlot)
            .ToListAsync();

        PendingCount = await _db.Appointments.CountAsync(a => a.Status == AppointmentStatus.Pending);
        ConfirmedCount = await _db.Appointments.CountAsync(a => a.Status == AppointmentStatus.Confirmed);
        TodayCount = await _db.Appointments.CountAsync(a =>
            a.AppointmentDate.Date == DateTime.Today &&
            a.Status != AppointmentStatus.Cancelled);
    }
}
