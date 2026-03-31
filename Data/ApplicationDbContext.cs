using Microsoft.EntityFrameworkCore;
using SmartHealthcareSystem.Models;

namespace SmartHealthcareSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; } // ✨ إضافة جديدة
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<TreatmentRecommendation> TreatmentRecommendations { get; set; }
        public DbSet<AIConversation> AIConversations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ═══════════════════════════════════════
            // Configure Relationships
            // ═══════════════════════════════════════

            // User → Doctor (One-to-One)
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Patient (One-to-One)
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment Relationships
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✨ MedicalRecord Relationships (جديد)
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // TreatmentRecommendation Relationships
            modelBuilder.Entity<TreatmentRecommendation>()
                .HasOne(tr => tr.Disease)
                .WithMany(d => d.TreatmentRecommendations)
                .HasForeignKey(tr => tr.DiseaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TreatmentRecommendation>()
                .HasOne(tr => tr.Medicine)
                .WithMany(m => m.TreatmentRecommendations)
                .HasForeignKey(tr => tr.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);

            // ═══════════════════════════════════════
            // Unique Constraints
            // ═══════════════════════════════════════
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.LicenseNumber)
                .IsUnique();

            // ═══════════════════════════════════════
            // Seed Data
            // ═══════════════════════════════════════

            // Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Email = "admin@healthcare.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    FullName = "System Administrator",
                    Phone = "0790000000",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            );

            // Diseases
            modelBuilder.Entity<Disease>().HasData(
                new Disease { DiseaseId = 1, Name = "Common Cold", NameArabic = "نزلة برد", Symptoms = "fever,cough,sore throat,runny nose,headache,sneezing,congestion,حمى,سعال,التهاب حلق,رشح,صداع,عطس,احتقان", Severity = "Mild", Description = "فيروس يصيب الجهاز التنفسي العلوي" },
                new Disease { DiseaseId = 2, Name = "Influenza", NameArabic = "إنفلونزا", Symptoms = "high fever,body ache,fatigue,headache,cough,chills,weakness,حمى شديدة,ألم جسم,تعب,صداع,سعال,قشعريرة,ضعف", Severity = "Moderate", Description = "عدوى فيروسية تصيب الجهاز التنفسي" },
                new Disease { DiseaseId = 3, Name = "Bronchitis", NameArabic = "التهاب الشعب الهوائية", Symptoms = "cough,mucus,chest discomfort,fatigue,shortness of breath,سعال,بلغم,ألم صدر,تعب,ضيق تنفس", Severity = "Moderate", Description = "التهاب في بطانة الشعب الهوائية" },
                new Disease { DiseaseId = 4, Name = "Pneumonia", NameArabic = "التهاب رئوي", Symptoms = "fever,cough,chest pain,shortness of breath,fatigue,chills,حمى,سعال,ألم صدر,ضيق تنفس,تعب,قشعريرة", Severity = "Severe", Description = "عدوى تسبب التهاب في الرئتين" },
                new Disease { DiseaseId = 5, Name = "Asthma", NameArabic = "الربو", Symptoms = "wheezing,shortness of breath,chest tightness,cough,صفير,ضيق تنفس,ضيق صدر,سعال", Severity = "Moderate", Description = "مرض مزمن يصيب الشعب الهوائية" },
                new Disease { DiseaseId = 6, Name = "Gastritis", NameArabic = "التهاب المعدة", Symptoms = "stomach pain,nausea,vomiting,bloating,loss of appetite,ألم معدة,غثيان,قيء,انتفاخ,فقدان شهية", Severity = "Moderate", Description = "التهاب في بطانة المعدة" },
                new Disease { DiseaseId = 7, Name = "Food Poisoning", NameArabic = "تسمم غذائي", Symptoms = "nausea,vomiting,diarrhea,stomach cramps,fever,weakness,غثيان,قيء,إسهال,مغص,حمى,ضعف", Severity = "Moderate", Description = "مرض ناتج عن تناول طعام ملوث" },
                new Disease { DiseaseId = 8, Name = "Gastroenteritis", NameArabic = "التهاب معوي", Symptoms = "diarrhea,vomiting,stomach pain,fever,dehydration,إسهال,قيء,ألم معدة,حمى,جفاف", Severity = "Moderate", Description = "التهاب في المعدة والأمعاء" },
                new Disease { DiseaseId = 9, Name = "Constipation", NameArabic = "إمساك", Symptoms = "difficult bowel movement,abdominal pain,bloating,صعوبة إخراج,ألم بطن,انتفاخ", Severity = "Mild", Description = "صعوبة في حركة الأمعاء" },
                new Disease { DiseaseId = 10, Name = "Acid Reflux", NameArabic = "ارتجاع المريء", Symptoms = "heartburn,chest pain,difficulty swallowing,sour taste,حرقة,ألم صدر,صعوبة بلع,طعم حامض", Severity = "Moderate", Description = "ارتداد حمض المعدة إلى المريء" },
                new Disease { DiseaseId = 11, Name = "Headache", NameArabic = "صداع", Symptoms = "head pain,pressure,throbbing,sensitivity to light,صداع,ضغط رأس,ألم نابض,حساسية ضوء", Severity = "Mild", Description = "ألم في منطقة الرأس" },
                new Disease { DiseaseId = 12, Name = "Migraine", NameArabic = "شقيقة", Symptoms = "severe headache,nausea,vomiting,sensitivity to light,visual disturbances,صداع شديد,غثيان,قيء,حساسية ضوء,اضطراب رؤية", Severity = "Moderate", Description = "صداع شديد ومتكرر" },
                new Disease { DiseaseId = 13, Name = "Back Pain", NameArabic = "ألم الظهر", Symptoms = "back pain,stiffness,muscle ache,limited movement,ألم ظهر,تيبس,ألم عضلي,صعوبة حركة", Severity = "Moderate", Description = "ألم في منطقة الظهر" },
                new Disease { DiseaseId = 14, Name = "Allergic Rhinitis", NameArabic = "حساسية الأنف", Symptoms = "sneezing,runny nose,itchy eyes,congestion,عطس,رشح,حكة عين,احتقان", Severity = "Mild", Description = "التهاب الأنف التحسسي" },
                new Disease { DiseaseId = 15, Name = "Skin Allergy", NameArabic = "حساسية جلدية", Symptoms = "rash,itching,redness,swelling,hives,طفح,حكة,احمرار,تورم,شرى", Severity = "Mild", Description = "رد فعل تحسسي على الجلد" },
                new Disease { DiseaseId = 16, Name = "Urinary Tract Infection", NameArabic = "التهاب مجرى البول", Symptoms = "burning urination,frequent urination,cloudy urine,pelvic pain,حرقة بول,تبول متكرر,بول عكر,ألم حوض", Severity = "Moderate", Description = "عدوى في الجهاز البولي" },
                new Disease { DiseaseId = 17, Name = "Ear Infection", NameArabic = "التهاب الأذن", Symptoms = "ear pain,fever,drainage,hearing difficulty,ألم أذن,حمى,إفرازات,صعوبة سمع", Severity = "Moderate", Description = "التهاب في الأذن الوسطى" },
                new Disease { DiseaseId = 18, Name = "Sinusitis", NameArabic = "التهاب الجيوب الأنفية", Symptoms = "facial pain,nasal congestion,headache,mucus,pressure,ألم وجه,احتقان أنف,صداع,مخاط,ضغط", Severity = "Moderate", Description = "التهاب الجيوب الأنفية" },
                new Disease { DiseaseId = 19, Name = "Eczema", NameArabic = "أكزيما", Symptoms = "dry skin,itching,redness,rash,cracked skin,جلد جاف,حكة,احمرار,طفح,تشقق", Severity = "Mild", Description = "حالة جلدية مزمنة" },
                new Disease { DiseaseId = 20, Name = "Psoriasis", NameArabic = "صدفية", Symptoms = "red patches,scales,itching,dry skin,cracked skin,بقع حمراء,قشور,حكة,جلد جاف,تشقق", Severity = "Moderate", Description = "مرض جلدي مناعي" },
                new Disease { DiseaseId = 21, Name = "Acne", NameArabic = "حب الشباب", Symptoms = "pimples,blackheads,oily skin,scarring,بثور,رؤوس سوداء,جلد دهني,ندوب", Severity = "Mild", Description = "حالة جلدية شائعة" },
                new Disease { DiseaseId = 22, Name = "Hypertension", NameArabic = "ضغط دم مرتفع", Symptoms = "headache,dizziness,chest pain,shortness of breath,nosebleed,صداع,دوخة,ألم صدر,ضيق تنفس,نزيف أنف", Severity = "Severe", Description = "ارتفاع ضغط الدم" },
                new Disease { DiseaseId = 23, Name = "Diabetes Symptoms", NameArabic = "أعراض السكري", Symptoms = "increased thirst,frequent urination,fatigue,blurred vision,slow healing,عطش شديد,تبول متكرر,تعب,زغللة,بطء التئام", Severity = "Severe", Description = "أعراض مرض السكري" },
                new Disease { DiseaseId = 24, Name = "Anemia", NameArabic = "فقر دم", Symptoms = "fatigue,weakness,pale skin,dizziness,shortness of breath,cold hands,تعب,ضعف,شحوب,دوخة,ضيق تنفس,برودة يدين", Severity = "Moderate", Description = "نقص خلايا الدم الحمراء" },
                new Disease { DiseaseId = 25, Name = "Anxiety", NameArabic = "قلق", Symptoms = "worry,nervousness,rapid heartbeat,sweating,trembling,fatigue,قلق,توتر,خفقان,تعرق,رجفة,تعب", Severity = "Moderate", Description = "اضطراب القلق" },
                new Disease { DiseaseId = 26, Name = "Insomnia", NameArabic = "أرق", Symptoms = "difficulty sleeping,waking up,fatigue,irritability,صعوبة نوم,استيقاظ,تعب,عصبية", Severity = "Mild", Description = "اضطراب النوم" },
                new Disease { DiseaseId = 27, Name = "Dehydration", NameArabic = "جفاف", Symptoms = "thirst,dry mouth,fatigue,dizziness,dark urine,عطش,جفاف فم,تعب,دوخة,بول داكن", Severity = "Moderate", Description = "نقص السوائل في الجسم" },
                new Disease { DiseaseId = 28, Name = "Arthritis", NameArabic = "التهاب المفاصل", Symptoms = "joint pain,stiffness,swelling,reduced movement,ألم مفاصل,تيبس,تورم,صعوبة حركة", Severity = "Moderate", Description = "التهاب في المفاصل" },
                new Disease { DiseaseId = 29, Name = "Conjunctivitis", NameArabic = "التهاب الملتحمة", Symptoms = "red eyes,itching,discharge,tearing,burning,احمرار عين,حكة,إفرازات,دموع,حرقة", Severity = "Mild", Description = "التهاب غشاء العين" }
            );

            // Medicines
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine { MedicineId = 1, Name = "Paracetamol", GenericName = "Acetaminophen", ActiveIngredient = "Paracetamol", Category = "Pain Relief", DosageForm = "Tablets", Strength = "500mg", Price = 3.50m, Description = "مسكن للألم وخافض للحرارة", ImageUrl = "/images/medicines/paracetamol.jpg", IsAvailable = true },
                new Medicine { MedicineId = 2, Name = "Ibuprofen", GenericName = "Ibuprofen", ActiveIngredient = "Ibuprofen", Category = "Pain Relief", DosageForm = "Tablets", Strength = "400mg", Price = 5.00m, Description = "مضاد التهاب ومسكن", ImageUrl = "/images/medicines/ibuprofen.jpg", IsAvailable = true },
                new Medicine { MedicineId = 3, Name = "Aspirin", GenericName = "Aspirin", ActiveIngredient = "Aspirin", Category = "Pain Relief", DosageForm = "Tablets", Strength = "100mg", Price = 2.50m, Description = "مسكن ومضاد التهاب", ImageUrl = "/images/medicines/aspirin.jpg", IsAvailable = true },
                new Medicine { MedicineId = 4, Name = "Amoxicillin", GenericName = "Amoxicillin", ActiveIngredient = "Amoxicillin", Category = "Antibiotics", DosageForm = "Capsules", Strength = "500mg", Price = 8.50m, Description = "مضاد حيوي واسع الطيف", ImageUrl = "/images/medicines/amoxicillin.jpg", IsAvailable = true },
                new Medicine { MedicineId = 5, Name = "Azithromycin", GenericName = "Azithromycin", ActiveIngredient = "Azithromycin", Category = "Antibiotics", DosageForm = "Tablets", Strength = "250mg", Price = 12.00m, Description = "مضاد حيوي قوي", ImageUrl = "/images/medicines/azithromycin.jpg", IsAvailable = true },
                new Medicine { MedicineId = 6, Name = "Omeprazole", GenericName = "Omeprazole", ActiveIngredient = "Omeprazole", Category = "Digestive Health", DosageForm = "Capsules", Strength = "20mg", Price = 6.00m, Description = "لعلاج حموضة المعدة", ImageUrl = "/images/medicines/omeprazole.jpg", IsAvailable = true },
                new Medicine { MedicineId = 7, Name = "Antacid", GenericName = "Antacid", ActiveIngredient = "Aluminum Hydroxide", Category = "Digestive Health", DosageForm = "Syrup", Strength = "200ml", Price = 4.50m, Description = "معادل للحموضة", ImageUrl = "/images/medicines/antacid.jpg", IsAvailable = true },
                new Medicine { MedicineId = 8, Name = "Loperamide", GenericName = "Loperamide", ActiveIngredient = "Loperamide", Category = "Digestive Health", DosageForm = "Tablets", Strength = "2mg", Price = 5.50m, Description = "لعلاج الإسهال", ImageUrl = "/images/medicines/loperamide.jpg", IsAvailable = true },
                new Medicine { MedicineId = 9, Name = "Cetirizine", GenericName = "Cetirizine", ActiveIngredient = "Cetirizine", Category = "Allergy Relief", DosageForm = "Tablets", Strength = "10mg", Price = 4.00m, Description = "مضاد للهستامين", ImageUrl = "/images/medicines/cetirizine.jpg", IsAvailable = true },
                new Medicine { MedicineId = 10, Name = "Loratadine", GenericName = "Loratadine", ActiveIngredient = "Loratadine", Category = "Allergy Relief", DosageForm = "Tablets", Strength = "10mg", Price = 4.50m, Description = "مضاد حساسية", ImageUrl = "/images/medicines/loratadine.jpg", IsAvailable = true },
                new Medicine { MedicineId = 11, Name = "Salbutamol", GenericName = "Salbutamol", ActiveIngredient = "Salbutamol", Category = "Respiratory", DosageForm = "Inhaler", Strength = "100mcg", Price = 15.00m, Description = "موسع للشعب الهوائية", ImageUrl = "/images/medicines/salbutamol.jpg", IsAvailable = true },
                new Medicine { MedicineId = 12, Name = "Cough Syrup", GenericName = "Dextromethorphan", ActiveIngredient = "Dextromethorphan", Category = "Respiratory", DosageForm = "Syrup", Strength = "100ml", Price = 6.50m, Description = "شراب للسعال", ImageUrl = "/images/medicines/cough-syrup.jpg", IsAvailable = true },
                new Medicine { MedicineId = 13, Name = "Vitamin C", GenericName = "Ascorbic Acid", ActiveIngredient = "Ascorbic Acid", Category = "Vitamins", DosageForm = "Tablets", Strength = "1000mg", Price = 7.00m, Description = "فيتامين سي", ImageUrl = "/images/medicines/vitaminc.jpg", IsAvailable = true },
                new Medicine { MedicineId = 14, Name = "Vitamin D", GenericName = "Cholecalciferol", ActiveIngredient = "Vitamin D3", Category = "Vitamins", DosageForm = "Capsules", Strength = "5000IU", Price = 8.00m, Description = "فيتامين د", ImageUrl = "/images/medicines/vitamind.jpg", IsAvailable = true },
                new Medicine { MedicineId = 15, Name = "Multivitamin", GenericName = "Multivitamin", ActiveIngredient = "Mixed Vitamins", Category = "Vitamins", DosageForm = "Tablets", Strength = "Daily", Price = 10.00m, Description = "فيتامينات متعددة", ImageUrl = "/images/medicines/multivitamin.jpg", IsAvailable = true }
            );

            // Treatment Recommendations
            modelBuilder.Entity<TreatmentRecommendation>().HasData(
                new TreatmentRecommendation { Id = 1, DiseaseId = 1, MedicineId = 1, Priority = 9 },
                new TreatmentRecommendation { Id = 2, DiseaseId = 1, MedicineId = 9, Priority = 7 },
                new TreatmentRecommendation { Id = 3, DiseaseId = 1, MedicineId = 13, Priority = 6 },
                new TreatmentRecommendation { Id = 4, DiseaseId = 2, MedicineId = 1, Priority = 10 },
                new TreatmentRecommendation { Id = 5, DiseaseId = 2, MedicineId = 2, Priority = 8 },
                new TreatmentRecommendation { Id = 6, DiseaseId = 3, MedicineId = 4, Priority = 10 },
                new TreatmentRecommendation { Id = 7, DiseaseId = 3, MedicineId = 12, Priority = 8 },
                new TreatmentRecommendation { Id = 8, DiseaseId = 4, MedicineId = 5, Priority = 10 },
                new TreatmentRecommendation { Id = 9, DiseaseId = 4, MedicineId = 1, Priority = 7 },
                new TreatmentRecommendation { Id = 10, DiseaseId = 5, MedicineId = 11, Priority = 10 },
                new TreatmentRecommendation { Id = 11, DiseaseId = 6, MedicineId = 6, Priority = 10 },
                new TreatmentRecommendation { Id = 12, DiseaseId = 6, MedicineId = 7, Priority = 7 },
                new TreatmentRecommendation { Id = 13, DiseaseId = 7, MedicineId = 8, Priority = 9 },
                new TreatmentRecommendation { Id = 14, DiseaseId = 7, MedicineId = 6, Priority = 7 },
                new TreatmentRecommendation { Id = 15, DiseaseId = 8, MedicineId = 8, Priority = 9 },
                new TreatmentRecommendation { Id = 16, DiseaseId = 8, MedicineId = 7, Priority = 7 },
                new TreatmentRecommendation { Id = 17, DiseaseId = 10, MedicineId = 6, Priority = 10 },
                new TreatmentRecommendation { Id = 18, DiseaseId = 10, MedicineId = 7, Priority = 8 },
                new TreatmentRecommendation { Id = 19, DiseaseId = 11, MedicineId = 1, Priority = 9 },
                new TreatmentRecommendation { Id = 20, DiseaseId = 11, MedicineId = 2, Priority = 8 },
                new TreatmentRecommendation { Id = 21, DiseaseId = 12, MedicineId = 2, Priority = 10 },
                new TreatmentRecommendation { Id = 22, DiseaseId = 12, MedicineId = 1, Priority = 7 },
                new TreatmentRecommendation { Id = 23, DiseaseId = 13, MedicineId = 2, Priority = 10 },
                new TreatmentRecommendation { Id = 24, DiseaseId = 13, MedicineId = 3, Priority = 7 },
                new TreatmentRecommendation { Id = 25, DiseaseId = 14, MedicineId = 9, Priority = 10 },
                new TreatmentRecommendation { Id = 26, DiseaseId = 14, MedicineId = 10, Priority = 9 },
                new TreatmentRecommendation { Id = 27, DiseaseId = 15, MedicineId = 9, Priority = 10 },
                new TreatmentRecommendation { Id = 28, DiseaseId = 16, MedicineId = 4, Priority = 10 },
                new TreatmentRecommendation { Id = 29, DiseaseId = 17, MedicineId = 4, Priority = 10 },
                new TreatmentRecommendation { Id = 30, DiseaseId = 17, MedicineId = 1, Priority = 7 },
                new TreatmentRecommendation { Id = 31, DiseaseId = 18, MedicineId = 5, Priority = 9 },
                new TreatmentRecommendation { Id = 32, DiseaseId = 18, MedicineId = 1, Priority = 7 },
                new TreatmentRecommendation { Id = 33, DiseaseId = 24, MedicineId = 15, Priority = 9 },
                new TreatmentRecommendation { Id = 34, DiseaseId = 27, MedicineId = 13, Priority = 8 },
                new TreatmentRecommendation { Id = 35, DiseaseId = 28, MedicineId = 2, Priority = 10 },
                new TreatmentRecommendation { Id = 36, DiseaseId = 28, MedicineId = 3, Priority = 8 },
                new TreatmentRecommendation { Id = 37, DiseaseId = 29, MedicineId = 4, Priority = 9 }
            );
        }
    }
}