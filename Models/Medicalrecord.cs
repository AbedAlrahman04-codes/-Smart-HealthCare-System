using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthcareSystem.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        [Required(ErrorMessage = "التشخيص مطلوب")]
        [StringLength(200)]
        public string Diagnosis { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Symptoms { get; set; }

        [StringLength(2000)]
        public string? Prescription { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}