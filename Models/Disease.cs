using System.ComponentModel.DataAnnotations;

namespace SmartHealthcareSystem.Models
{
    public class Disease
    {
        [Key]
        public int DiseaseId { get; set; }

        [Required(ErrorMessage = "اسم المرض مطلوب")]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string NameArabic { get; set; }

        [Required(ErrorMessage = "الأعراض مطلوبة")]
        public string Symptoms { get; set; } // نص يحتوي الأعراض مفصولة بفواصل

        [Required]
        [StringLength(20)]
        public string Severity { get; set; } = "Moderate"; // Mild, Moderate, Severe

        public string Description { get; set; }

        // Navigation Properties
        public ICollection<TreatmentRecommendation> TreatmentRecommendations { get; set; }
    }
}