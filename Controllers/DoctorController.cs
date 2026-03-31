using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcareSystem.Data;
using SmartHealthcareSystem.Models;

namespace SmartHealthcareSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }
         
        // ═══════════════════════════════════════════════════════════
        // Dashboard - الصفحة الرئيسية
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> Dashboard()
        {
            // التحقق من تسجيل الدخول
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Doctor")
            {
                TempData["ErrorMessage"] = "يجب تسجيل الدخول كطبيب للوصول إلى هذه الصفحة";
                return RedirectToAction("Login", "Account");
            }

            // الحصول على معلومات الطبيب
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على ملف الطبيب";
                return RedirectToAction("Login", "Account");
            }

            // إحصائيات اليوم
            var today = DateTime.Today;
            ViewBag.TodayAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId && a.AppointmentDate.Date == today)
                .CountAsync();

            ViewBag.TodayCompletedAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId &&
                           a.AppointmentDate.Date == today &&
                           a.Status == "Completed")
                .CountAsync();

            ViewBag.TodayPendingAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId &&
                           a.AppointmentDate.Date == today &&
                           a.Status == "Pending")
                .CountAsync();

            // إحصائيات عامة
            ViewBag.TotalPatients = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId)
                .Select(a => a.PatientId)
                .Distinct()
                .CountAsync();

            ViewBag.TotalAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId)
                .CountAsync();

            ViewBag.CompletedAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId && a.Status == "Completed")
                .CountAsync();

            ViewBag.PendingAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId && a.Status == "Pending")
                .CountAsync();

            // المواعيد القادمة (أقرب 5)
            ViewBag.UpcomingAppointments = await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .Where(a => a.DoctorId == doctor.DoctorId &&
                           a.AppointmentDate >= DateTime.Now &&
                           a.Status == "Pending")
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .ToListAsync();

            // آخر المرضى (أحدث 5)
            ViewBag.RecentPatients = await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .Where(a => a.DoctorId == doctor.DoctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => a.Patient)
                .Distinct()
                .Take(5)
                .ToListAsync();

            // إحصائيات الأسبوع
            var weekAgo = today.AddDays(-7);
            ViewBag.WeekAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.DoctorId &&
                           a.AppointmentDate >= weekAgo)
                .CountAsync();

            // معلومات الطبيب
            ViewBag.DoctorInfo = doctor;

            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // My Patients - مرضاي
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> MyPatients()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Doctor")
            {
                TempData["ErrorMessage"] = "غير مصرح لك بالوصول";
                return RedirectToAction("Login", "Account");
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على ملف الطبيب";
                return RedirectToAction("Login", "Account");
            }

            // الحصول على المرضى الذين لديهم مواعيد مع هذا الطبيب
            var patients = await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .Where(a => a.DoctorId == doctor.DoctorId)
                .Select(a => a.Patient)
                .Distinct()
                .ToListAsync();

            return View(patients);
        }

        // ═══════════════════════════════════════════════════════════
        // Appointments - المواعيد
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> Appointments()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Doctor")
            {
                TempData["ErrorMessage"] = "غير مصرح لك بالوصول";
                return RedirectToAction("Login", "Account");
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على ملف الطبيب";
                return RedirectToAction("Login", "Account");
            }

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .Where(a => a.DoctorId == doctor.DoctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // POST: Doctor/UpdateAppointmentStatus
        [HttpPost]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, string status)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(appointmentId);

                if (appointment == null)
                {
                    return Json(new { success = false, message = "الموعد غير موجود" });
                }

                appointment.Status = status;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم تحديث حالة الموعد" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Doctor/ToggleAvailability
        [HttpPost]
        public async Task<IActionResult> ToggleAvailability()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.UserId == userId);

                if (doctor == null)
                {
                    return Json(new { success = false, message = "لم يتم العثور على ملف الطبيب" });
                }

                doctor.IsAvailable = !doctor.IsAvailable;
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    isAvailable = doctor.IsAvailable,
                    message = doctor.IsAvailable ? "أنت الآن متاح لاستقبال المواعيد" : "تم إيقاف استقبال المواعيد"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Doctor/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Doctor")
            {
                TempData["ErrorMessage"] = "غير مصرح لك بالوصول";
                return RedirectToAction("Login", "Account");
            }

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على ملف الطبيب";
                return RedirectToAction("Login", "Account");
            }

            return View(doctor);
        }
    }
}