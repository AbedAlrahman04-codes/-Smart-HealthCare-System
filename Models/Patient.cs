using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthcareSystem.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "الجنس مطلوب")]
        [StringLength(10)]
        public string Gender { get; set; } // Male, Female

        [StringLength(5)]
        public string? BloodType { get; set; } // A+, O-, etc.

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? EmergencyContact { get; set; }

        [StringLength(20)]
        [Phone]
        public string? EmergencyPhone { get; set; }

        // Navigation Properties
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; } // إضافة جديدة
    }
}