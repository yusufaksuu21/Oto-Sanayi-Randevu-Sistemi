using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Pages.Appointments;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IList<Appointment> Appointments { get; set; } = new List<Appointment>();

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return;

        Appointments = await _db.Appointments
            .Include(a => a.Vehicle)
            .Include(a => a.Service)
            .Where(a => a.CustomerId == user.Id)
            .OrderByDescending(a => a.AppointmentDate)
            .ThenByDescending(a => a.TimeSlot)
            .ToListAsync();
    }
}
