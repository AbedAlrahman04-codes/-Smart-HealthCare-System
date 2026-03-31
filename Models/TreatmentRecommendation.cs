using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthcareSystem.Models
{
    public class TreatmentRecommendation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DiseaseId { get; set; }

        [ForeignKey("DiseaseId")]
        public Disease Disease { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [ForeignKey("MedicineId")]
        public Medicine Medicine { get; set; }

        [Range(1, 10)]
        public int Priority { get; set; } = 5; // 1=قليل الأهمية, 10=أولوية عالية
    }
}