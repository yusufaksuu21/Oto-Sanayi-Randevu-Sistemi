using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Pages.Vehicles;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [BindProperty]
    public VehicleInput Input { get; set; } = new();

    public class VehicleInput
    {
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

        [Required(ErrorMessage = "Yıl zorunludur.")]
        [Range(1980, 2026)]
        [Display(Name = "Yıl")]
        public int Year { get; set; } = DateTime.Now.Year;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        _db.Vehicles.Add(new Vehicle
        {
            OwnerId = user.Id,
            Plate = Input.Plate.Trim().ToUpperInvariant(),
            Brand = Input.Brand.Trim(),
            Model = Input.Model.Trim(),
            Year = Input.Year
        });
        await _db.SaveChangesAsync();

        TempData["Success"] = "Aracınız başarıyla eklendi.";
        return RedirectToPage("./Index");
    }
}
