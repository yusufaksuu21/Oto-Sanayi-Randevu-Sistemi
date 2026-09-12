using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Pages;

public class ServicesModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public ServicesModel(ApplicationDbContext db) => _db = db;
    public IList<Service> Services { get; private set; } = new List<Service>();

    public async Task OnGetAsync() => Services = await _db.Services.OrderBy(s => s.Name).ToListAsync();
}
