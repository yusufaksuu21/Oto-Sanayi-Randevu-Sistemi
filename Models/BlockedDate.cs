using System;
using System.ComponentModel.DataAnnotations;

namespace SanayiRandevu.Models
{
    // Randevu alınmasını engelleyen özel tarih ve sebep kaydı.
    public class BlockedDate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [StringLength(250)]
        public string? Reason { get; set; }
    }
}
