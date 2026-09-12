using System.ComponentModel.DataAnnotations;

namespace SanayiRandevu.Models
{
    // Sunulan hizmet
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adý zorunludur.")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        // Opsiyonel açýklama
        public string? Description { get; set; }

        [Required(ErrorMessage = "Süre zorunludur.")]
        [Range(1, 1440, ErrorMessage = "Süre dakikasý geçersiz.")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0, 1000000, ErrorMessage = "Fiyat geçersiz.")]
        public decimal Price { get; set; }
    }
}
