using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthcareSystem.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required(ErrorMessage = "التخصص مطلوب")]
        [StringLength(100)]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "رقم الترخيص مطلوب")]
        [StringLength(50)]
        public string LicenseNumber { get; set; }

        [Range(0, 10000, ErrorMessage = "الرسوم يجب أن تكون بين 0 و 10000")]
        public decimal ConsultationFee { get; set; }

        [StringLength(200)]
        public string? WorkingHours { get; set; } // مثلاً: "9:00 AM - 5:00 PM"

        public bool IsAvailable { get; set; } = true;

        // Navigation Properties
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; } // إضافة جديدة
    }
}