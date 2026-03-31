using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthcareSystem.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        [Required(ErrorMessage = "تاريخ الموعد مطلوب")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled

        // إضافة جديدة: سبب الزيارة
        [StringLength(500)]
        public string? Reason { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}