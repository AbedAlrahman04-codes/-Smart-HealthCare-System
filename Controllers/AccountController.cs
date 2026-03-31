using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcareSystem.Data;
using SmartHealthcareSystem.Models;
using SmartHealthcareSystem.ViewModels;

namespace SmartHealthcareSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ═══════════════════════════════════════════════════════════
        // GET: /Account/Login
        // ═══════════════════════════════════════════════════════════
        public IActionResult Login(string returnUrl = null)
        {
            // إذا كان مسجل دخول بالفعل
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // POST: /Account/Login
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // البحث عن المستخدم
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.IsActive);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة");
                return View(model);
            }

            // التحقق من كلمة المرور
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة");
                return View(model);
            }

            // حفظ بيانات المستخدم في Session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Role", user.Role);

            // التوجيه حسب الدور
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return user.Role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Doctor" => RedirectToAction("Dashboard", "Doctor"),
                "Patient" => RedirectToAction("Dashboard", "Patient"), // ✨ معدّل - يروح للـ Dashboard!
                _ => RedirectToAction("Index", "Home")
            };
        }

        // ═══════════════════════════════════════════════════════════
        // GET: /Account/Register
        // ═══════════════════════════════════════════════════════════
        public IActionResult Register()
        {
            // إذا كان مسجل دخول بالفعل
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // POST: /Account/Register ✨ معدّل - Auto Login!
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // التحقق من عدم تكرار اسم المستخدم
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "اسم المستخدم موجود بالفعل");
                return View(model);
            }

            // التحقق من عدم تكرار البريد
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني مسجل بالفعل");
                return View(model);
            }

            // إنشاء المستخدم
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                Phone = model.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "Patient",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // إنشاء ملف المريض
            var patient = new Patient
            {
                UserId = user.UserId,
                DateOfBirth = model.DateOfBirth ?? DateTime.Now.AddYears(-25),
                Gender = model.Gender ?? "Male",
                BloodType = model.BloodType ?? "Unknown",
                Address = model.Address ?? "",
                EmergencyContact = "",
                EmergencyPhone = ""
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // ✨✨✨ Auto Login - تسجيل دخول تلقائي ✨✨✨
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Role", "Patient");

            TempData["SuccessMessage"] = "تم التسجيل بنجاح! مرحباً بك في النظام";

            // ✨ التوجيه المباشر لـ Patient Dashboard!
            return RedirectToAction("Dashboard", "Patient");
        }

        // ═══════════════════════════════════════════════════════════
        // GET: /Account/Logout
        // ═══════════════════════════════════════════════════════════
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "تم تسجيل الخروج بنجاح";
            return RedirectToAction("Index", "Home");
        }

        // ═══════════════════════════════════════════════════════════
        // GET: /Account/Profile
        // ═══════════════════════════════════════════════════════════
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // ═══════════════════════════════════════════════════════════
        // Helper: التحقق من تسجيل الدخول
        // ═══════════════════════════════════════════════════════════
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }
    }
}