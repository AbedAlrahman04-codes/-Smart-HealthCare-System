using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthcareSystem.Data;
using SmartHealthcareSystem.Models;

namespace SmartHealthcareSystem.Controllers
{
    public class MedicalRecordController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ══════════════════════════════════════════════════════════
        // GET: MedicalRecord/MyRecords - للمريض
        // ══════════════════════════════════════════════════════════
        public async Task<IActionResult> MyRecords()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Patient")
            {
                TempData["ErrorMessage"] = "Access denied";
                return RedirectToAction("Login", "Account");
            }

            // جلب PatientId
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                TempData["ErrorMessage"] = "Patient profile not found";
                return RedirectToAction("Dashboard", "Patient");
            }

            // جلب السجلات الطبية
            var records = await _context.MedicalRecords
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.User)
                .Where(m => m.PatientId == patient.PatientId)
                .OrderByDescending(m => m.VisitDate)
                .ToListAsync();

            ViewBag.PatientName = HttpContext.Session.GetString("FullName");

            return View(records);
        }

        // ══════════════════════════════════════════════════════════
        // GET: MedicalRecord/Details/5
        // ══════════════════════════════════════════════════════════
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            var record = await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.User)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(m => m.RecordId == id);

            if (record == null)
            {
                return NotFound();
            }

            // التحقق من الصلاحيات
            if (role == "Patient")
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (patient == null || record.PatientId != patient.PatientId)
                {
                    TempData["ErrorMessage"] = "Access denied";
                    return RedirectToAction("MyRecords");
                }
            }
            else if (role == "Doctor")
            {
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.UserId == userId);

                if (doctor == null || record.DoctorId != doctor.DoctorId)
                {
                    TempData["ErrorMessage"] = "Access denied";
                    return RedirectToAction("Index", "Doctor");
                }
            }
            else if (role != "Admin")
            {
                TempData["ErrorMessage"] = "Access denied";
                return RedirectToAction("Login", "Account");
            }

            return View(record);
        }

        // ══════════════════════════════════════════════════════════
        // GET: MedicalRecord/Create - للطبيب
        // ══════════════════════════════════════════════════════════
        public async Task<IActionResult> Create(int? patientId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || (role != "Doctor" && role != "Admin"))
            {
                TempData["ErrorMessage"] = "Access denied - Only doctors can create medical records";
                return RedirectToAction("Login", "Account");
            }

            if (patientId == null)
            {
                TempData["ErrorMessage"] = "Patient not specified";
                return RedirectToAction("Index", "Doctor");
            }

            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PatientId == patientId);

            if (patient == null)
            {
                TempData["ErrorMessage"] = "Patient not found";
                return RedirectToAction("Index", "Doctor");
            }

            ViewBag.PatientName = patient.User.FullName;
            ViewBag.PatientId = patientId;

            return View();
        }

        // ══════════════════════════════════════════════════════════
        // POST: MedicalRecord/Create
        // ══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRecord record)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || (role != "Doctor" && role != "Admin"))
            {
                TempData["ErrorMessage"] = "Access denied";
                return RedirectToAction("Login", "Account");
            }

            // جلب DoctorId
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "Doctor profile not found";
                return RedirectToAction("Index", "Doctor");
            }

            record.DoctorId = doctor.DoctorId;
            record.CreatedAt = DateTime.Now;

            // Remove validation for fields we're setting programmatically
            ModelState.Remove("Doctor");
            ModelState.Remove("Patient");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(record);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Medical record created successfully";
                    return RedirectToAction("Details", new { id = record.RecordId });
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error creating record: {ex.Message}";
                }
            }

            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PatientId == record.PatientId);

            ViewBag.PatientName = patient?.User.FullName;
            ViewBag.PatientId = record.PatientId;

            return View(record);
        }

        // ══════════════════════════════════════════════════════════
        // GET: MedicalRecord/Edit/5 - للطبيب
        // ══════════════════════════════════════════════════════════
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || (role != "Doctor" && role != "Admin"))
            {
                TempData["ErrorMessage"] = "Access denied";
                return RedirectToAction("Login", "Account");
            }

            var record = await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(m => m.RecordId == id);

            if (record == null)
            {
                return NotFound();
            }

            // التحقق من أن الطبيب هو من أنشأ السجل
            if (role == "Doctor")
            {
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.UserId == userId);

                if (doctor == null || record.DoctorId != doctor.DoctorId)
                {
                    TempData["ErrorMessage"] = "You can only edit your own records";
                    return RedirectToAction("Index", "Doctor");
                }
            }

            ViewBag.PatientName = record.Patient.User.FullName;

            return View(record);
        }

        // ══════════════════════════════════════════════════════════
        // POST: MedicalRecord/Edit/5
        // ══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicalRecord record)
        {
            if (id != record.RecordId)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || (role != "Doctor" && role != "Admin"))
            {
                TempData["ErrorMessage"] = "Access denied";
                return RedirectToAction("Login", "Account");
            }

            // Remove validation for navigation properties
            ModelState.Remove("Doctor");
            ModelState.Remove("Patient");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(record);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Medical record updated successfully";
                    return RedirectToAction("Details", new { id = record.RecordId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRecordExists(record.RecordId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error updating record: {ex.Message}";
                }
            }

            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PatientId == record.PatientId);

            ViewBag.PatientName = patient?.User.FullName;

            return View(record);
        }

        // ══════════════════════════════════════════════════════════
        // GET: MedicalRecord/Delete/5
        // ══════════════════════════════════════════════════════════
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Admin")
            {
                TempData["ErrorMessage"] = "Only administrators can delete records";
                return RedirectToAction("Index", "Home");
            }

            var record = await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.User)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(m => m.RecordId == id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // ══════════════════════════════════════════════════════════
        // POST: MedicalRecord/Delete/5
        // ══════════════════════════════════════════════════════════
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Admin")
            {
                TempData["ErrorMessage"] = "Only administrators can delete records";
                return RedirectToAction("Index", "Home");
            }

            var record = await _context.MedicalRecords.FindAsync(id);

            if (record != null)
            {
                _context.MedicalRecords.Remove(record);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Medical record deleted successfully";
            }

            return RedirectToAction("Index", "Admin");
        }

        // ══════════════════════════════════════════════════════════
        // Helper Methods
        // ══════════════════════════════════════════════════════════
        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.RecordId == id);
        }
    }
}