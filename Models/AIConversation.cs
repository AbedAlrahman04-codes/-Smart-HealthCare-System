using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthcareSystem.Models
{
    public class AIConversation
    {
        [Key]
        public int ConversationId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        [StringLength(2000)]
        public string UserMessage { get; set; } // الأعراض التي كتبها المستخدم

        [Required]
        [StringLength(2000)]
        public string AIResponse { get; set; } // رد الـ AI

        // ✅ جعلها Optional لأن مو دائماً نحتاجها
        [StringLength(200)]
        public string? DiseaseName { get; set; } // المرض المكتشف

        // ✅ جعلها Optional
        public string? RecommendedMedicines { get; set; } // JSON للأدوية المقترحة

        // ✅ جعلها Optional مع قيمة افتراضية
        [Range(0, 100)]
        public double? Confidence { get; set; } // نسبة الثقة

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}