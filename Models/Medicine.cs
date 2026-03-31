using System.ComponentModel.DataAnnotations;

namespace SmartHealthcareSystem.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "اسم الدواء مطلوب")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string GenericName { get; set; }

        [Required(ErrorMessage = "المادة الفعالة مطلوبة")]
        [StringLength(100)]
        public string ActiveIngredient { get; set; }

        [Required(ErrorMessage = "الفئة مطلوبة")]
        [StringLength(50)]
        public string Category { get; set; } // Pain Relief, Antibiotics, etc.

        [Required]
        [StringLength(30)]
        public string DosageForm { get; set; } // Tablets, Syrup, Capsules

        [StringLength(20)]
        public string Strength { get; set; } // 500mg, 250mg

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; } = "/images/default-medicine.jpg";

        public bool IsAvailable { get; set; } = true;

        // Navigation Properties
        public ICollection<TreatmentRecommendation> TreatmentRecommendations { get; set; }
    }
}