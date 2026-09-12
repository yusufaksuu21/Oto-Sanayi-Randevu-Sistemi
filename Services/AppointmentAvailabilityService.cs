using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Data;
using SanayiRandevu.Models;

namespace SanayiRandevu.Services
{
    // Randevu müsaitlik kontrol servisi
    public class AppointmentAvailabilityService
    {
        private readonly ApplicationDbContext _db;
        private const int MaxAppointmentsPerSlot = 2;
        private const int DefaultStartHour = 9; // 09:00

        public AppointmentAvailabilityService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Belirli tarih için müsait saat dilimlerini döndürür (örn "09:00")
        public async Task<List<string>> GetAvailableTimeSlotsAsync(DateTime date)
        {
            var target = date.Date;

            // Kapalý tarih kontrolü
            var isBlocked = await _db.BlockedDates.AnyAsync(b => b.Date.Date == target);
            if (isBlocked) return new List<string>();

            // Çalýþma saatleri kontrolü
            var wh = await _db.WorkingHours.FirstOrDefaultAsync(w => w.DayOfWeek == target.DayOfWeek);
            if (wh == null || wh.IsClosed) return new List<string>();

            var openHour = (int)wh.OpenTime.TotalHours;
            var closeHour = (int)wh.CloseTime.TotalHours;
            if (openHour < DefaultStartHour) openHour = DefaultStartHour;

            var result = new List<string>();
            for (int h = openHour; h < closeHour; h++)
            {
                var slot = $"{h:00}:00";
                var count = await _db.Appointments
                    .Where(a => a.AppointmentDate == target && a.TimeSlot == slot && a.Status != AppointmentStatus.Cancelled)
                    .CountAsync();

                if (count < MaxAppointmentsPerSlot) result.Add(slot);
            }

            return result;
        }

        // Tek bir slotun hâlâ müsait olup olmadýðýný kontrol eder
        public async Task<bool> IsSlotAvailableAsync(DateTime date, string timeSlot)
        {
            var target = date.Date;

            // Kapalý tarih kontrolü
            var isBlocked = await _db.BlockedDates.AnyAsync(b => b.Date.Date == target);
            if (isBlocked) return false;

            var wh = await _db.WorkingHours.FirstOrDefaultAsync(w => w.DayOfWeek == target.DayOfWeek);
            if (wh == null || wh.IsClosed) return false;

            if (!TimeSpan.TryParse(timeSlot, out var ts)) return false;
            var hour = ts.Hours;

            var openHour = (int)wh.OpenTime.TotalHours;
            var closeHour = (int)wh.CloseTime.TotalHours;
            if (openHour < DefaultStartHour) openHour = DefaultStartHour;

            if (hour < openHour || hour >= closeHour) return false;

            var count = await _db.Appointments
                .Where(a => a.AppointmentDate == target && a.TimeSlot == timeSlot && a.Status != AppointmentStatus.Cancelled)
                .CountAsync();

            return count < MaxAppointmentsPerSlot;
        }
    }
}
