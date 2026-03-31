using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcareSystem.Data;
using SmartHealthcareSystem.Models;

namespace SmartHealthcareSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ═══════════════════════════════════════════════════════════
        // Dashboard - الصفحة الرئيسية
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> Dashboard()
        {
            // التحقق من تسجيل الدخول والصلاحيات
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Admin")
            {
                TempData["ErrorMessage"] = "يجب تسجيل الدخول كمدير للوصول إلى هذه الصفحة";
                return RedirectToAction("Login", "Account");
            }

            // إحصائيات النظام
            ViewBag.TotalDoctors = await _context.Doctors.CountAsync();
            ViewBag.TotalPatients = await _context.Patients.CountAsync();
            ViewBag.TotalAppointments = await _context.Appointments.CountAsync();
            ViewBag.TotalConversations = await _context.AIConversations.CountAsync();

            // الأطباء المتاحين
            ViewBag.AvailableDoctors = await _context.Doctors
                .Where(d => d.IsAvailable)
                .CountAsync();

            // المواعيد المعلقة
            ViewBag.PendingAppointments = await _context.Appointments
                .Where(a => a.Status == "Pending")
                .CountAsync();

            // آخر 5 أطباء
            ViewBag.RecentDoctors = await _context.Doctors
                .Include(d => d.User)
                .OrderByDescending(d => d.DoctorId)
                .Take(5)
                .ToListAsync();

            // آخر 5 مرضى
            ViewBag.RecentPatients = await _context.Patients
                .Include(p => p.User)
                .OrderByDescending(p => p.PatientId)
                .Take(5)
                .ToListAsync();

            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // Doctors Management - إدارة الأطباء
        // ═══════════════════════════════════════════════════════════

        // GET: Admin/ManageDoctors
        public async Task<IActionResult> ManageDoctors()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Admin")
            {
                TempData["ErrorMessage"] = "غير مصرح لك بالوصول";
                return RedirectToAction("Login", "Account");
            }

            var doctors = await _context.Doctors
                .Include(d => d.User)
                .OrderByDescending(d => d.DoctorId)
                .ToListAsync();

            return View(doctors);
        }

        // GET: Admin/AddDoctor
        public IActionResult AddDoctor()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Admin")
            {
                TempData["ErrorMessage"] = "غير مصرح لك بالوصول";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Admin/AddDoctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(
            string fullName,
            string username,
            string email,
            string phone,
            string password,
            string specialization,
            string licenseNumber,
            decimal consultationFee,
            string workingHours)
        {
            try
            {
                // التحقق من وجود اسم المستخدم
                if (await _context.Users.AnyAsync(u => u.Username == username))
                {
                    TempData["ErrorMessage"] = "اسم المستخدم موجود بالفعل";
                    return View();
                }

                // التحقق من وجود البريد الإلكتروني
                if (await _context.Users.AnyAsync(u => u.Email == email))
                {
                    TempData["ErrorMessage"] = "البريد الإلكتروني مسجل بالفعل";
                    return View();
                }

                // إنشاء المستخدم
                var user = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    FullName = fullName,
                    Phone = phone,
                    Role = "Doctor",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // إنشاء ملف الطبيب
                var doctor = new Doctor
                {
                    UserId = user.UserId,
                    Specialization = specialization,
                    LicenseNumber = licenseNumber,
                    ConsultationFee = consultationFee,
                    WorkingHours = workingHours,
                    IsAvailable = true
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إضافة الطبيب بنجاح";
                return RedirectToAction("ManageDoctors");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة الطبيب: " + ex.Message;
                return View();
            }
        }

        // POST: Admin/ToggleDoctorAvailability
        [HttpPost]
        public async Task<IActionResult> ToggleDoctorAvailability(int doctorId)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(doctorId);
                if (doctor == null)
                {
                    return Json(new { success = false, message = "الطبيب غير موجود" });
                }

                doctor.IsAvailable = !doctor.IsAvailable;
                await _context.SaveChangesAsync();

                return Json(new { success = true, isAvailable = doctor.IsAvailable });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/DeleteDoctor
        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            try
            {
                var doctor = await _context.Doctors
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.DoctorId == doctorId);

                if (doctor == null)
                {
                    return Json(new { success = false, message = "الطبيب غير موجود" });
                }

                // حذف المواعيد المرتبطة
                var appointments = await _context.Appointments
                    .Where(a => a.DoctorId == doctorId)
                    .ToListAsync();
                _context.Appointments.RemoveRange(appointments);

                // حذف الطبيب والمستخدم
                _context.Doctors.Remove(doctor);
                _context.Users.Remove(doctor.User);

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم حذف الطبيب بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ═══════════════════════════════════════════════════════════
        // Patients Management - إدارة المرضى
        // ═══════════════════════════════════════════════════════════

        // GET: Admin/ManagePatients
        public async Task<IActionResult> ManagePatients()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Admin")
            {
                TempData["ErrorMessage"] = "غير مصرح لك بالوصول";
                return RedirectToAction("Login", "Account");
            }

            var patients = await _context.Patients
                .Include(p => p.User)
                .OrderByDescending(p => p.PatientId)
                .ToListAsync();

            return View(patients);
        }

        // POST: Admin/DeletePatient
        [HttpPost]
        public async Task<IActionResult> DeletePatient(int patientId)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.PatientId == patientId);

                if (patient == null)
                {
                    return Json(new { success = false, message = "المريض غير موجود" });
                }

                // حذف المواعيد المرتبطة
                var appointments = await _context.Appointments
                    .Where(a => a.PatientId == patientId)
                    .ToListAsync();
                _context.Appointments.RemoveRange(appointments);

                // حذف المحادثات المرتبطة
                var conversations = await _context.AIConversations
                    .Where(c => c.UserId == patient.UserId)
                    .ToListAsync();
                _context.AIConversations.RemoveRange(conversations);

                // حذف المريض والمستخدم
                _context.Patients.Remove(patient);
                _context.Users.Remove(patient.User);

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم حذف المريض بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ═══════════════════════════════════════════════════════════
        // Statistics - الإحصائيات التفصيلية
        // ═══════════════════════════════════════════════════════════

        public async Task<IActionResult> Statistics()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Admin")
            {
                TempData["ErrorMessage"] = "غير مصرح لك بالوصول";
                return RedirectToAction("Login", "Account");
            }

            // إحصائيات مفصلة
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.ActiveUsers = await _context.Users.Where(u => u.IsActive).CountAsync();
            ViewBag.TotalDiseases = await _context.Diseases.CountAsync();
            ViewBag.TotalMedicines = await _context.Medicines.CountAsync();

            // إحصائيات المحادثات
            ViewBag.TodayConversations = await _context.AIConversations
                .Where(c => c.Timestamp.Date == DateTime.Today)
                .CountAsync();

            ViewBag.ThisWeekConversations = await _context.AIConversations
                .Where(c => c.Timestamp >= DateTime.Today.AddDays(-7))
                .CountAsync();

            // الأمراض الأكثر شيوعاً
            var topDiseases = await _context.AIConversations
                .GroupBy(c => c.DiseaseName)
                .Select(g => new { Disease = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            ViewBag.TopDiseases = topDiseases;

            return View();
        }
    }
}