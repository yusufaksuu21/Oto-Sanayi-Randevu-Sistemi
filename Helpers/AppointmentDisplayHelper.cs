using SanayiRandevu.Models;

namespace SanayiRandevu.Helpers
{
    public static class AppointmentDisplayHelper
    {
        public static string GetStatusText(AppointmentStatus status) => status switch
        {
            AppointmentStatus.Pending => "Onay Bekliyor",
            AppointmentStatus.Confirmed => "Onaylandı",
            AppointmentStatus.Rejected => "Reddedildi",
            AppointmentStatus.Completed => "Tamamlandı",
            AppointmentStatus.Cancelled => "İptal Edildi",
            _ => status.ToString()
        };

        public static string GetStatusBadgeClass(AppointmentStatus status) => status switch
        {
            AppointmentStatus.Pending => "status-pending",
            AppointmentStatus.Confirmed => "status-confirmed",
            AppointmentStatus.Rejected => "status-cancelled",
            AppointmentStatus.Completed => "status-completed",
            AppointmentStatus.Cancelled => "status-cancelled",
            _ => "status-pending"
        };

        public static string GetDayName(DayOfWeek day) => day switch
        {
            DayOfWeek.Monday => "Pazartesi",
            DayOfWeek.Tuesday => "Salı",
            DayOfWeek.Wednesday => "�arsamba",
            DayOfWeek.Thursday => "Perşembe",
            DayOfWeek.Friday => "Cuma",
            DayOfWeek.Saturday => "Cumartesi",
            DayOfWeek.Sunday => "Pazar",
            _ => day.ToString()
        };
    }
}
