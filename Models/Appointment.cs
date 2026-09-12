using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanayiRandevu.Models
{
    // Randevu kayd˜
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; } = null!;

        // M˜˜teri navigation
        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser? Customer { get; set; }

        [Required]
        public int VehicleId { get; set; }

        // Ara˜ navigation
        public Vehicle? Vehicle { get; set; }

        [Required]
        public int ServiceId { get; set; }

        // Hizmet navigation
        public Service? Service { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Saat dilimi zorunludur.")]
        [StringLength(5)]
        public string TimeSlot { get; set; } = null!; // ˜rn "09:00"

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        // M??teri notu / mesaj?
        [StringLength(1000)]
        public string? Notes { get; set; }

        // Admin geri bildirimi
        [StringLength(1000)]
        public string? AdminReply { get; set; }

        public DateTime? AdminReplyAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
