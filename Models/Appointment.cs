using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanayiRandevu.Models
{
    // Müşteri, araç ve hizmet seçimini tek bir randevu kaydında birleştiren model.
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; } = null!;

        // Randevuyu oluşturan müşteri
        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser? Customer { get; set; }

        [Required]
        public int VehicleId { get; set; }

        // Randevuya ait araç
        public Vehicle? Vehicle { get; set; }

        [Required]
        public int ServiceId { get; set; }

        // Seçilen hizmet
        public Service? Service { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Saat dilimi zorunludur.")]
        [StringLength(5)]
        public string TimeSlot { get; set; } = null!; // Örnek: "09:00"

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        // Müşteriye ait ek not veya mesaj
        [StringLength(1000)]
        public string? Notes { get; set; }

        // Yöneticinin randevuya verdiği yanıt
        [StringLength(1000)]
        public string? AdminReply { get; set; }

        public DateTime? AdminReplyAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
