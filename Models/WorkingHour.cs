using System;
using System.ComponentModel.DataAnnotations;

namespace SanayiRandevu.Models
{
    // Her gün için çalışma kapı saatlerini tanımlayan plan kaydı.
    public class WorkingHour
    {
        public int Id { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required(ErrorMessage = "Açılış saati zorunludur.")]
        public TimeSpan OpenTime { get; set; }

        [Required(ErrorMessage = "Kapanış saati zorunludur.")]
        public TimeSpan CloseTime { get; set; }

        // Bu gün kapalı mı?
        public bool IsClosed { get; set; } = false;
    }
}
