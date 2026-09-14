using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SanayiRandevu.Models
{
    // Kimlik doğrulaması için genişletilmiş kullanıcı profili.
    public class ApplicationUser : IdentityUser
    {
        // Tam ad bilgisi (isteğe bağlı)
        public string? FullName { get; set; }

        // Kullanıcının araçları ve randevuları
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
