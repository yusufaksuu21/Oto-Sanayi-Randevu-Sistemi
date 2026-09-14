using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanayiRandevu.Models
{
    // Kullanıcının sahip olduğu araca ait temel bilgiler ve plakayı tutan model.
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; } = null!;

        // Aracın sahibi
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser? Owner { get; set; }

        [Required(ErrorMessage = "Plaka zorunludur.")]
        [StringLength(15, ErrorMessage = "Plaka en fazla 15 karakter olabilir.")]
        public string Plate { get; set; } = null!;

        [Required(ErrorMessage = "Marka zorunludur.")]
        [StringLength(50)]
        public string Brand { get; set; } = null!;

        [Required(ErrorMessage = "Model zorunludur.")]
        [StringLength(50)]
        public string Model { get; set; } = null!;

        [Range(1980, 2026, ErrorMessage = "Yıl 1980 ile 2026 arasında olmalıdır.")]
        public int Year { get; set; }
    }
}
