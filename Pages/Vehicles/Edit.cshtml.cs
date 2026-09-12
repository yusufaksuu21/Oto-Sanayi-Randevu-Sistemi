using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Pages.Vehicles;

[Authorize]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [BindProperty]
    public VehicleInput Input { get; set; } = new();

    public class VehicleInput
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plaka zorunludur.")]
        [StringLength(15)]
        [Display(Name = "Plaka")]
        public string Plate { get; set; } = "";

        [Required(ErrorMessage = "Marka zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Marka")]
        public string Brand { get; set; } = "";

        [Required(ErrorMessage = "Model zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Model")]
        public string Model { get; set; } = "";

        [Required]
        [Range(1980, 2026)]
        [Display(Name = "Yıl")]
        public int Year { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var vehicle = await GetOwnedVehicleAsync(id);
        if (vehicle == null) return NotFound();

        Input = new VehicleInput
        {
            Id = vehicle.Id,
            Plate = vehicle.Plate,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            Year = vehicle.Year
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var vehicle = await GetOwnedVehicleAsync(Input.Id);
        if (vehicle == null) return NotFound();

        vehicle.Plate = Input.Plate.Trim().ToUpperInvariant();
        vehicle.Brand = Input.Brand.Trim();
        vehicle.Model = Input.Model.Trim();
        vehicle.Year = Input.Year;

        await _db.SaveChangesAsync();
        TempData["Success"] = "Araç bilgileri güncellendi.";
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
