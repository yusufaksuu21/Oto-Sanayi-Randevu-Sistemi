namespace SanayiRandevu.Models
{
    // Randevunun hangi aşamada olduğunu belirten durum listesi.
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Rejected,
        Completed,
        Cancelled
    }
}
