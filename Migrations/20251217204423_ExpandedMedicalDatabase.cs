using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthcareSystem.Migrations
{
    /// <inheritdoc />
    public partial class ExpandedMedicalDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 1,
                columns: new[] { "Description", "Symptoms" },
                values: new object[] { "فيروس يصيب الجهاز التنفسي العلوي", "fever,cough,sore throat,runny nose,headache,sneezing,congestion,حمى,سعال,التهاب حلق,رشح,صداع,عطس,احتقان" });

            migrationBuilder.UpdateData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 2,
                columns: new[] { "Description", "Symptoms" },
                values: new object[] { "عدوى فيروسية تصيب الجهاز التنفسي", "high fever,body ache,fatigue,headache,cough,chills,weakness,حمى شديدة,ألم جسم,تعب,صداع,سعال,قشعريرة,ضعف" });

            migrationBuilder.UpdateData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 3,
                columns: new[] { "Description", "Name", "NameArabic", "Severity", "Symptoms" },
                values: new object[] { "التهاب في بطانة الشعب الهوائية", "Bronchitis", "التهاب الشعب الهوائية", "Moderate", "cough,mucus,chest discomfort,fatigue,shortness of breath,سعال,بلغم,ألم صدر,تعب,ضيق تنفس" });

            migrationBuilder.UpdateData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 4,
                columns: new[] { "Description", "Name", "NameArabic", "Severity", "Symptoms" },
                values: new object[] { "عدوى تسبب التهاب في الرئتين", "Pneumonia", "التهاب رئوي", "Severe", "fever,cough,chest pain,shortness of breath,fatigue,chills,حمى,سعال,ألم صدر,ضيق تنفس,تعب,قشعريرة" });

            migrationBuilder.InsertData(
                table: "Diseases",
                columns: new[] { "DiseaseId", "Description", "Name", "NameArabic", "Severity", "Symptoms" },
                values: new object[,]
                {
                    { 5, "مرض مزمن يصيب الشعب الهوائية", "Asthma", "الربو", "Moderate", "wheezing,shortness of breath,chest tightness,cough,صفير,ضيق تنفس,ضيق صدر,سعال" },
                    { 6, "التهاب في بطانة المعدة", "Gastritis", "التهاب المعدة", "Moderate", "stomach pain,nausea,vomiting,bloating,loss of appetite,ألم معدة,غثيان,قيء,انتفاخ,فقدان شهية" },
                    { 7, "مرض ناتج عن تناول طعام ملوث", "Food Poisoning", "تسمم غذائي", "Moderate", "nausea,vomiting,diarrhea,stomach cramps,fever,weakness,غثيان,قيء,إسهال,مغص,حمى,ضعف" },
                    { 8, "التهاب في المعدة والأمعاء", "Gastroenteritis", "التهاب معوي", "Moderate", "diarrhea,vomiting,stomach pain,fever,dehydration,إسهال,قيء,ألم معدة,حمى,جفاف" },
                    { 9, "صعوبة في حركة الأمعاء", "Constipation", "إمساك", "Mild", "difficult bowel movement,abdominal pain,bloating,صعوبة إخراج,ألم بطن,انتفاخ" },
                    { 10, "ارتداد حمض المعدة إلى المريء", "Acid Reflux", "ارتجاع المريء", "Moderate", "heartburn,chest pain,difficulty swallowing,sour taste,حرقة,ألم صدر,صعوبة بلع,طعم حامض" },
                    { 11, "ألم في منطقة الرأس", "Headache", "صداع", "Mild", "head pain,pressure,throbbing,sensitivity to light,صداع,ضغط رأس,ألم نابض,حساسية ضوء" },
                    { 12, "صداع شديد ومتكرر", "Migraine", "شقيقة", "Moderate", "severe headache,nausea,vomiting,sensitivity to light,visual disturbances,صداع شديد,غثيان,قيء,حساسية ضوء,اضطراب رؤية" },
                    { 13, "ألم في منطقة الظهر", "Back Pain", "ألم الظهر", "Moderate", "back pain,stiffness,muscle ache,limited movement,ألم ظهر,تيبس,ألم عضلي,صعوبة حركة" },
                    { 14, "التهاب الأنف التحسسي", "Allergic Rhinitis", "حساسية الأنف", "Mild", "sneezing,runny nose,itchy eyes,congestion,عطس,رشح,حكة عين,احتقان" },
                    { 15, "رد فعل تحسسي على الجلد", "Skin Allergy", "حساسية جلدية", "Mild", "rash,itching,redness,swelling,hives,طفح,حكة,احمرار,تورم,شرى" },
                    { 16, "عدوى في الجهاز البولي", "Urinary Tract Infection", "التهاب مجرى البول", "Moderate", "burning urination,frequent urination,cloudy urine,pelvic pain,حرقة بول,تبول متكرر,بول عكر,ألم حوض" },
                    { 17, "التهاب في الأذن الوسطى", "Ear Infection", "التهاب الأذن", "Moderate", "ear pain,fever,drainage,hearing difficulty,ألم أذن,حمى,إفرازات,صعوبة سمع" },
                    { 18, "التهاب الجيوب الأنفية", "Sinusitis", "التهاب الجيوب الأنفية", "Moderate", "facial pain,nasal congestion,headache,mucus,pressure,ألم وجه,احتقان أنف,صداع,مخاط,ضغط" },
                    { 19, "حالة جلدية مزمنة", "Eczema", "أكزيما", "Mild", "dry skin,itching,redness,rash,cracked skin,جلد جاف,حكة,احمرار,طفح,تشقق" },
                    { 20, "مرض جلدي مناعي", "Psoriasis", "صدفية", "Moderate", "red patches,scales,itching,dry skin,cracked skin,بقع حمراء,قشور,حكة,جلد جاف,تشقق" },
                    { 21, "حالة جلدية شائعة", "Acne", "حب الشباب", "Mild", "pimples,blackheads,oily skin,scarring,بثور,رؤوس سوداء,جلد دهني,ندوب" },
                    { 22, "ارتفاع ضغط الدم", "Hypertension", "ضغط دم مرتفع", "Severe", "headache,dizziness,chest pain,shortness of breath,nosebleed,صداع,دوخة,ألم صدر,ضيق تنفس,نزيف أنف" },
                    { 23, "أعراض مرض السكري", "Diabetes Symptoms", "أعراض السكري", "Severe", "increased thirst,frequent urination,fatigue,blurred vision,slow healing,عطش شديد,تبول متكرر,تعب,زغللة,بطء التئام" },
                    { 24, "نقص خلايا الدم الحمراء", "Anemia", "فقر دم", "Moderate", "fatigue,weakness,pale skin,dizziness,shortness of breath,cold hands,تعب,ضعف,شحوب,دوخة,ضيق تنفس,برودة يدين" },
                    { 25, "اضطراب القلق", "Anxiety", "قلق", "Moderate", "worry,nervousness,rapid heartbeat,sweating,trembling,fatigue,قلق,توتر,خفقان,تعرق,رجفة,تعب" },
                    { 26, "اضطراب النوم", "Insomnia", "أرق", "Mild", "difficulty sleeping,waking up,fatigue,irritability,صعوبة نوم,استيقاظ,تعب,عصبية" },
                    { 27, "نقص السوائل في الجسم", "Dehydration", "جفاف", "Moderate", "thirst,dry mouth,fatigue,dizziness,dark urine,عطش,جفاف فم,تعب,دوخة,بول داكن" },
                    { 28, "التهاب في المفاصل", "Arthritis", "التهاب المفاصل", "Moderate", "joint pain,stiffness,swelling,reduced movement,ألم مفاصل,تيبس,تورم,صعوبة حركة" },
                    { 29, "التهاب غشاء العين", "Conjunctivitis", "التهاب الملتحمة", "Mild", "red eyes,itching,discharge,tearing,burning,احمرار عين,حكة,إفرازات,دموع,حرقة" }
                });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 1,
                column: "Description",
                value: "مسكن للألم وخافض للحرارة");

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 2,
                column: "Description",
                value: "مضاد التهاب ومسكن");

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 3,
                columns: new[] { "ActiveIngredient", "Category", "Description", "DosageForm", "GenericName", "ImageUrl", "Name", "Price", "Strength" },
                values: new object[] { "Aspirin", "Pain Relief", "مسكن ومضاد التهاب", "Tablets", "Aspirin", "/images/medicines/aspirin.jpg", "Aspirin", 2.50m, "100mg" });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 4,
                columns: new[] { "ActiveIngredient", "Category", "Description", "GenericName", "ImageUrl", "Name", "Price", "Strength" },
                values: new object[] { "Amoxicillin", "Antibiotics", "مضاد حيوي واسع الطيف", "Amoxicillin", "/images/medicines/amoxicillin.jpg", "Amoxicillin", 8.50m, "500mg" });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 5,
                columns: new[] { "ActiveIngredient", "Category", "Description", "GenericName", "ImageUrl", "Name", "Price", "Strength" },
                values: new object[] { "Azithromycin", "Antibiotics", "مضاد حيوي قوي", "Azithromycin", "/images/medicines/azithromycin.jpg", "Azithromycin", 12.00m, "250mg" });

            migrationBuilder.InsertData(
                table: "Medicines",
                columns: new[] { "MedicineId", "ActiveIngredient", "Category", "Description", "DosageForm", "GenericName", "ImageUrl", "IsAvailable", "Name", "Price", "Strength" },
                values: new object[,]
                {
                    { 6, "Omeprazole", "Digestive Health", "لعلاج حموضة المعدة", "Capsules", "Omeprazole", "/images/medicines/omeprazole.jpg", true, "Omeprazole", 6.00m, "20mg" },
                    { 7, "Aluminum Hydroxide", "Digestive Health", "معادل للحموضة", "Syrup", "Antacid", "/images/medicines/antacid.jpg", true, "Antacid", 4.50m, "200ml" },
                    { 8, "Loperamide", "Digestive Health", "لعلاج الإسهال", "Tablets", "Loperamide", "/images/medicines/loperamide.jpg", true, "Loperamide", 5.50m, "2mg" },
                    { 9, "Cetirizine", "Allergy Relief", "مضاد للهستامين", "Tablets", "Cetirizine", "/images/medicines/cetirizine.jpg", true, "Cetirizine", 4.00m, "10mg" },
                    { 10, "Loratadine", "Allergy Relief", "مضاد حساسية", "Tablets", "Loratadine", "/images/medicines/loratadine.jpg", true, "Loratadine", 4.50m, "10mg" },
                    { 11, "Salbutamol", "Respiratory", "موسع للشعب الهوائية", "Inhaler", "Salbutamol", "/images/medicines/salbutamol.jpg", true, "Salbutamol", 15.00m, "100mcg" },
                    { 12, "Dextromethorphan", "Respiratory", "شراب للسعال", "Syrup", "Dextromethorphan", "/images/medicines/cough-syrup.jpg", true, "Cough Syrup", 6.50m, "100ml" },
                    { 13, "Ascorbic Acid", "Vitamins", "فيتامين سي", "Tablets", "Ascorbic Acid", "/images/medicines/vitaminc.jpg", true, "Vitamin C", 7.00m, "1000mg" },
                    { 14, "Vitamin D3", "Vitamins", "فيتامين د", "Capsules", "Cholecalciferol", "/images/medicines/vitamind.jpg", true, "Vitamin D", 8.00m, "5000IU" },
                    { 15, "Mixed Vitamins", "Vitamins", "فيتامينات متعددة", "Tablets", "Multivitamin", "/images/medicines/multivitamin.jpg", true, "Multivitamin", 10.00m, "Daily" }
                });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 2,
                column: "MedicineId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DiseaseId", "MedicineId", "Priority" },
                values: new object[] { 1, 13, 6 });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "MedicineId", "Priority" },
                values: new object[] { 1, 10 });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DiseaseId", "MedicineId", "Priority" },
                values: new object[] { 2, 2, 8 });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "MedicineId", "Priority" },
                values: new object[] { 4, 10 });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DiseaseId", "MedicineId", "Priority" },
                values: new object[] { 3, 12, 8 });

            migrationBuilder.InsertData(
                table: "TreatmentRecommendations",
                columns: new[] { "Id", "DiseaseId", "MedicineId", "Priority" },
                values: new object[,]
                {
                    { 8, 4, 5, 10 },
                    { 9, 4, 1, 7 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 17, 23, 44, 22, 724, DateTimeKind.Local).AddTicks(5836), "$2a$11$xJhEISHZSTzfXstXxIlqk.GP3i4aDiQNXlXmY8oW/zkFexiUYDQAK" });

            migrationBuilder.InsertData(
                table: "TreatmentRecommendations",
                columns: new[] { "Id", "DiseaseId", "MedicineId", "Priority" },
                values: new object[,]
                {
                    { 10, 5, 11, 10 },
                    { 11, 6, 6, 10 },
                    { 12, 6, 7, 7 },
                    { 13, 7, 8, 9 },
                    { 14, 7, 6, 7 },
                    { 15, 8, 8, 9 },
                    { 16, 8, 7, 7 },
                    { 17, 10, 6, 10 },
                    { 18, 10, 7, 8 },
                    { 19, 11, 1, 9 },
                    { 20, 11, 2, 8 },
                    { 21, 12, 2, 10 },
                    { 22, 12, 1, 7 },
                    { 23, 13, 2, 10 },
                    { 24, 13, 3, 7 },
                    { 25, 14, 9, 10 },
                    { 26, 14, 10, 9 },
                    { 27, 15, 9, 10 },
                    { 28, 16, 4, 10 },
                    { 29, 17, 4, 10 },
                    { 30, 17, 1, 7 },
                    { 31, 18, 5, 9 },
                    { 32, 18, 1, 7 },
                    { 33, 24, 15, 9 },
                    { 34, 27, 13, 8 },
                    { 35, 28, 2, 10 },
                    { 36, 28, 3, 8 },
                    { 37, 29, 4, 9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 1,
                columns: new[] { "Description", "Symptoms" },
                values: new object[] { "A viral infection of the upper respiratory tract", "fever,cough,sore throat,runny nose,headache,حمى,سعال,التهاب حلق,صداع" });

            migrationBuilder.UpdateData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 2,
                columns: new[] { "Description", "Symptoms" },
                values: new object[] { "A contagious respiratory illness caused by influenza viruses", "high fever,body ache,fatigue,headache,cough,حمى شديدة,ألم جسم,تعب,صداع,سعال" });

            migrationBuilder.UpdateData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 3,
                columns: new[] { "Description", "Name", "NameArabic", "Severity", "Symptoms" },
                values: new object[] { "Pain in any region of the head", "Headache", "صداع", "Mild", "head pain,migraine,tension,صداع,ألم رأس,شقيقة" });

            migrationBuilder.UpdateData(
                table: "Diseases",
                keyColumn: "DiseaseId",
                keyValue: 4,
                columns: new[] { "Description", "Name", "NameArabic", "Severity", "Symptoms" },
                values: new object[] { "Inflammation of the stomach lining", "Gastritis", "التهاب المعدة", "Moderate", "stomach pain,nausea,vomiting,bloating,ألم معدة,غثيان,قيء,انتفاخ" });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 1,
                column: "Description",
                value: "Effective for pain relief and fever reduction");

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 2,
                column: "Description",
                value: "Anti-inflammatory and pain relief");

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 3,
                columns: new[] { "ActiveIngredient", "Category", "Description", "DosageForm", "GenericName", "ImageUrl", "Name", "Price", "Strength" },
                values: new object[] { "Amoxicillin", "Antibiotics", "Antibiotic for bacterial infections", "Capsules", "Amoxicillin", "/images/medicines/amoxicillin.jpg", "Amoxicillin", 8.50m, "500mg" });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 4,
                columns: new[] { "ActiveIngredient", "Category", "Description", "GenericName", "ImageUrl", "Name", "Price", "Strength" },
                values: new object[] { "Omeprazole", "Digestive Health", "For stomach acid and heartburn", "Omeprazole", "/images/medicines/omeprazole.jpg", "Omeprazole", 6.00m, "20mg" });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 5,
                columns: new[] { "ActiveIngredient", "Category", "Description", "GenericName", "ImageUrl", "Name", "Price", "Strength" },
                values: new object[] { "Cetirizine", "Allergy Relief", "Antihistamine for allergies", "Cetirizine", "/images/medicines/cetirizine.jpg", "Cetirizine", 4.00m, "10mg" });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 2,
                column: "MedicineId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DiseaseId", "MedicineId", "Priority" },
                values: new object[] { 2, 1, 10 });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "MedicineId", "Priority" },
                values: new object[] { 2, 8 });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DiseaseId", "MedicineId", "Priority" },
                values: new object[] { 3, 1, 9 });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "MedicineId", "Priority" },
                values: new object[] { 2, 8 });

            migrationBuilder.UpdateData(
                table: "TreatmentRecommendations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DiseaseId", "MedicineId", "Priority" },
                values: new object[] { 4, 4, 10 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 16, 20, 34, 59, 709, DateTimeKind.Local).AddTicks(9713), "$2a$11$QlVs2d5ZhjA.BKCSa6q7DuJi9208bR9ue/NZnwb9whowD7L1grJt6" });
        }
    }
}
