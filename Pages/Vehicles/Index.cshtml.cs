using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Pages.Vehicles;

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

    public IList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return;

        Vehicles = await _db.Vehicles
            .Where(v => v.OwnerId == user.Id)
            .OrderByDescending(v => v.Id)
            .ToListAsync();
    }
}
