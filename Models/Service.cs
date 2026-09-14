using System.ComponentModel.DataAnnotations;

namespace SanayiRandevu.Models
{
    // Sistem içinde sunulan bakım veya servis tanımını saklayan sınıf.
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        // İsteğe bağlı açıklama
        public string? Description { get; set; }

        [Required(ErrorMessage = "Süre zorunludur.")]
        [Range(1, 1440, ErrorMessage = "Süre dakikası geçersiz.")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0, 1000000, ErrorMessage = "Fiyat geçersiz.")]
        public decimal Price { get; set; }
    }
}
