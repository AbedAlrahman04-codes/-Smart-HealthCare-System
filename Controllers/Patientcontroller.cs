using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcareSystem.Data;
using SmartHealthcareSystem.Models;

namespace SmartHealthcareSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ═══════════════════════════════════════════════════════════
        // Dashboard
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                TempData["ErrorMessage"] = "الملف الشخصي غير موجود!";
                return RedirectToAction("Login", "Account");
            }

            var totalAppointments = await _context.Appointments
                .Where(a => a.PatientId == patient.PatientId)
                .CountAsync();

            var upcomingAppointments = await _context.Appointments
                .Where(a => a.PatientId == patient.PatientId &&
                           a.AppointmentDate >= DateTime.Now &&
                           a.Status == "Scheduled")
                .CountAsync();

            var completedAppointments = await _context.Appointments
                .Where(a => a.PatientId == patient.PatientId && a.Status == "Completed")
                .CountAsync();

            var cancelledAppointments = await _context.Appointments
                .Where(a => a.PatientId == patient.PatientId && a.Status == "Cancelled")
                .CountAsync();

            var nextAppointment = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Where(a => a.PatientId == patient.PatientId &&
                           a.AppointmentDate >= DateTime.Now &&
                           a.Status == "Scheduled")
                .OrderBy(a => a.AppointmentDate)
                .FirstOrDefaultAsync();

            var recentAppointments = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Where(a => a.PatientId == patient.PatientId)
                .OrderByDescending(a => a.AppointmentDate)
                .Take(5)
                .ToListAsync();

            var medicalRecordsCount = await _context.MedicalRecords
                .Where(m => m.PatientId == patient.PatientId)
                .CountAsync();

            ViewBag.Patient = patient;
            ViewBag.TotalAppointments = totalAppointments;
            ViewBag.UpcomingAppointments = upcomingAppointments;
            ViewBag.CompletedAppointments = completedAppointments;
            ViewBag.CancelledAppointments = cancelledAppointments;
            ViewBag.NextAppointment = nextAppointment;
            ViewBag.RecentAppointments = recentAppointments;
            ViewBag.MedicalRecordsCount = medicalRecordsCount;

            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // My Appointments
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> MyAppointments(string status = "All")
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Where(a => a.PatientId == patient.PatientId);

            if (status != "All")
            {
                query = query.Where(a => a.Status == status);
            }

            var appointments = await query
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            ViewBag.CurrentStatus = status;
            ViewBag.AllCount = await _context.Appointments.Where(a => a.PatientId == patient.PatientId).CountAsync();
            ViewBag.ScheduledCount = await _context.Appointments.Where(a => a.PatientId == patient.PatientId && a.Status == "Scheduled").CountAsync();
            ViewBag.CompletedCount = await _context.Appointments.Where(a => a.PatientId == patient.PatientId && a.Status == "Completed").CountAsync();
            ViewBag.CancelledCount = await _context.Appointments.Where(a => a.PatientId == patient.PatientId && a.Status == "Cancelled").CountAsync();

            return View(appointments);
        }

        // ═══════════════════════════════════════════════════════════
        // Appointment Details - جديد!
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> AppointmentDetails(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "الموعد غير موجود!";
                return RedirectToAction("MyAppointments");
            }

            // Authorization check
            if (appointment.PatientId != patient.PatientId)
            {
                TempData["ErrorMessage"] = "غير مصرح لك بعرض هذا الموعد!";
                return RedirectToAction("MyAppointments");
            }

            // Get Medical Record if exists
            var medicalRecord = await _context.MedicalRecords
                .FirstOrDefaultAsync(m => m.PatientId == patient.PatientId &&
                                         m.DoctorId == appointment.DoctorId &&
                                         m.VisitDate.Date == appointment.AppointmentDate.Date);

            ViewBag.MedicalRecord = medicalRecord;

            return View(appointment);
        }

        // ═══════════════════════════════════════════════════════════
        // Book Appointment - GET
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> BookAppointment()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Where(d => d.IsAvailable)
                .OrderBy(d => d.User.FullName)
                .ToListAsync();

            ViewBag.Doctors = doctors;
            ViewBag.Patient = patient;

            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // Book Appointment - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(int doctorId, DateTime appointmentDate, string? reason)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (appointmentDate < DateTime.Now)
            {
                TempData["ErrorMessage"] = "لا يمكن حجز موعد في الماضي!";
                return RedirectToAction("BookAppointment");
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId && d.IsAvailable);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "الطبيب غير متاح حالياً!";
                return RedirectToAction("BookAppointment");
            }

            var conflict = await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                              a.AppointmentDate == appointmentDate &&
                              a.Status == "Scheduled");

            if (conflict)
            {
                TempData["ErrorMessage"] = "هذا الموعد محجوز مسبقاً! يرجى اختيار موعد آخر.";
                return RedirectToAction("BookAppointment");
            }

            var appointment = new Appointment
            {
                PatientId = patient.PatientId,
                DoctorId = doctorId,
                AppointmentDate = appointmentDate,
                Status = "Scheduled",
                Reason = reason,
                CreatedAt = DateTime.Now
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم حجز الموعد بنجاح!";
            return RedirectToAction("MyAppointments");
        }

        // ═══════════════════════════════════════════════════════════
        // Reschedule Appointment - GET - جديد!
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> RescheduleAppointment(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == id && a.PatientId == patient.PatientId);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "الموعد غير موجود!";
                return RedirectToAction("MyAppointments");
            }

            if (appointment.Status != "Scheduled")
            {
                TempData["ErrorMessage"] = "لا يمكن إعادة جدولة موعد غير مجدول!";
                return RedirectToAction("AppointmentDetails", new { id });
            }

            if (appointment.AppointmentDate < DateTime.Now)
            {
                TempData["ErrorMessage"] = "لا يمكن إعادة جدولة موعد منتهي!";
                return RedirectToAction("AppointmentDetails", new { id });
            }

            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Where(d => d.IsAvailable)
                .OrderBy(d => d.User.FullName)
                .ToListAsync();

            ViewBag.Doctors = doctors;

            return View(appointment);
        }

        // ═══════════════════════════════════════════════════════════
        // Reschedule Appointment - POST - جديد!
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RescheduleAppointment(int id, DateTime appointmentDate, int? doctorId, string? reason)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id && a.PatientId == patient.PatientId);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "الموعد غير موجود!";
                return RedirectToAction("MyAppointments");
            }

            // Update appointment
            appointment.AppointmentDate = appointmentDate;
            if (doctorId.HasValue)
            {
                appointment.DoctorId = doctorId.Value;
            }
            if (!string.IsNullOrEmpty(reason))
            {
                appointment.Reason = reason;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم إعادة جدولة الموعد بنجاح!";
            return RedirectToAction("AppointmentDetails", new { id });
        }

        // ═══════════════════════════════════════════════════════════
        // Cancel Appointment - GET - جديد!
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == id && a.PatientId == patient.PatientId);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "الموعد غير موجود!";
                return RedirectToAction("MyAppointments");
            }

            if (appointment.Status == "Cancelled")
            {
                TempData["ErrorMessage"] = "هذا الموعد ملغي بالفعل!";
                return RedirectToAction("AppointmentDetails", new { id });
            }

            if (appointment.Status == "Completed")
            {
                TempData["ErrorMessage"] = "لا يمكن إلغاء موعد مكتمل!";
                return RedirectToAction("AppointmentDetails", new { id });
            }

            return View(appointment);
        }

        // ═══════════════════════════════════════════════════════════
        // Cancel Appointment - POST - جديد!
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ActionName("CancelAppointment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointmentConfirmed(int id, string? cancellationReason)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id && a.PatientId == patient.PatientId);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "الموعد غير موجود!";
                return RedirectToAction("MyAppointments");
            }

            appointment.Status = "Cancelled";

            if (!string.IsNullOrEmpty(cancellationReason))
            {
                appointment.Notes = $"سبب الإلغاء: {cancellationReason}";
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم إلغاء الموعد بنجاح!";
            return RedirectToAction("MyAppointments");
        }

        // ═══════════════════════════════════════════════════════════
        // Profile - GET
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(patient);
        }

        // ═══════════════════════════════════════════════════════════
        // Profile - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(Patient model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            patient.User.FullName = model.User.FullName;
            patient.User.Phone = model.User.Phone;
            patient.DateOfBirth = model.DateOfBirth;
            patient.Gender = model.Gender;
            patient.BloodType = model.BloodType;
            patient.Address = model.Address;
            patient.EmergencyContact = model.EmergencyContact;
            patient.EmergencyPhone = model.EmergencyPhone;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم تحديث الملف الشخصي بنجاح!";
            return RedirectToAction("Profile");
        }
    }
}