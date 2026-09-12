using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SanayiRandevu.Models
{
    // IdentityUser'dan geniþletilmiþ uygulama kullanýcýsý
    public class ApplicationUser : IdentityUser
    {
        // Tam ad (opsiyonel)
        public string? FullName { get; set; }

        // Kullanýcýnýn araçlarý ve randevularý (navigation)
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
