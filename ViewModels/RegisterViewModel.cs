using System.ComponentModel.DataAnnotations;

namespace SmartHealthcareSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "اسم المستخدم يجب أن يكون بين 3 و 50 حرف")]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الكامل لا يمكن أن يتجاوز 100 حرف")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; }

        // للمرضى فقط
        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "الجنس")]
        public string Gender { get; set; }

        [Display(Name = "فصيلة الدم")]
        public string BloodType { get; set; }

        [Display(Name = "العنوان")]
        [StringLength(200)]
        public string Address { get; set; }
    }
}