using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SmartHealthcareSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(20)]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string Phone { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } // Admin, Doctor, Patient
         
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public ICollection<AIConversation> AIConversations { get; set; }
    }
}