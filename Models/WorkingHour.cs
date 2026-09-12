using System;
using System.ComponentModel.DataAnnotations;

namespace SanayiRandevu.Models
{
    // Haftalýk çalýþma saatleri kaydý
    public class WorkingHour
    {
        public int Id { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required(ErrorMessage = "Açýlýþ saati zorunludur.")]
        public TimeSpan OpenTime { get; set; }

        [Required(ErrorMessage = "Kapanýþ saati zorunludur.")]
        public TimeSpan CloseTime { get; set; }

        // O gün kapalý mý
        public bool IsClosed { get; set; } = false;
    }
}
