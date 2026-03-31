using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthcareSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    DiseaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Symptoms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.DiseaseId);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    MedicineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GenericName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActiveIngredient = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DosageForm = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.MedicineId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreatmentRecommendations_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentRecommendations_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "MedicineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AIConversations",
                columns: table => new
                {
                    ConversationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AIResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiseaseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RecommendedMedicines = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Confidence = table.Column<double>(type: "float", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIConversations", x => x.ConversationId);
                    table.ForeignKey(
                        name: "FK_AIConversations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkingHours = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorId);
                    table.ForeignKey(
                        name: "FK_Doctors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BloodType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmergencyContact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmergencyPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Diseases",
                columns: new[] { "DiseaseId", "Description", "Name", "NameArabic", "Severity", "Symptoms" },
                values: new object[,]
                {
                    { 1, "A viral infection of the upper respiratory tract", "Common Cold", "نزلة برد", "Mild", "fever,cough,sore throat,runny nose,headache,حمى,سعال,التهاب حلق,صداع" },
                    { 2, "A contagious respiratory illness caused by influenza viruses", "Influenza", "إنفلونزا", "Moderate", "high fever,body ache,fatigue,headache,cough,حمى شديدة,ألم جسم,تعب,صداع,سعال" },
                    { 3, "Pain in any region of the head", "Headache", "صداع", "Mild", "head pain,migraine,tension,صداع,ألم رأس,شقيقة" },
                    { 4, "Inflammation of the stomach lining", "Gastritis", "التهاب المعدة", "Moderate", "stomach pain,nausea,vomiting,bloating,ألم معدة,غثيان,قيء,انتفاخ" }
                });

            migrationBuilder.InsertData(
                table: "Medicines",
                columns: new[] { "MedicineId", "ActiveIngredient", "Category", "Description", "DosageForm", "GenericName", "ImageUrl", "IsAvailable", "Name", "Price", "Strength" },
                values: new object[,]
                {
                    { 1, "Paracetamol", "Pain Relief", "Effective for pain relief and fever reduction", "Tablets", "Acetaminophen", "/images/medicines/paracetamol.jpg", true, "Paracetamol", 3.50m, "500mg" },
                    { 2, "Ibuprofen", "Pain Relief", "Anti-inflammatory and pain relief", "Tablets", "Ibuprofen", "/images/medicines/ibuprofen.jpg", true, "Ibuprofen", 5.00m, "400mg" },
                    { 3, "Amoxicillin", "Antibiotics", "Antibiotic for bacterial infections", "Capsules", "Amoxicillin", "/images/medicines/amoxicillin.jpg", true, "Amoxicillin", 8.50m, "500mg" },
                    { 4, "Omeprazole", "Digestive Health", "For stomach acid and heartburn", "Capsules", "Omeprazole", "/images/medicines/omeprazole.jpg", true, "Omeprazole", 6.00m, "20mg" },
                    { 5, "Cetirizine", "Allergy Relief", "Antihistamine for allergies", "Tablets", "Cetirizine", "/images/medicines/cetirizine.jpg", true, "Cetirizine", 4.00m, "10mg" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "Phone", "Role", "Username" },
                values: new object[] { 1, new DateTime(2025, 12, 16, 20, 34, 59, 709, DateTimeKind.Local).AddTicks(9713), "admin@healthcare.com", "System Administrator", true, "$2a$11$QlVs2d5ZhjA.BKCSa6q7DuJi9208bR9ue/NZnwb9whowD7L1grJt6", "0790000000", "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "TreatmentRecommendations",
                columns: new[] { "Id", "DiseaseId", "MedicineId", "Priority" },
                values: new object[,]
                {
                    { 1, 1, 1, 9 },
                    { 2, 1, 5, 7 },
                    { 3, 2, 1, 10 },
                    { 4, 2, 2, 8 },
                    { 5, 3, 1, 9 },
                    { 6, 3, 2, 8 },
                    { 7, 4, 4, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIConversations_UserId",
                table: "AIConversations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_LicenseNumber",
                table: "Doctors",
                column: "LicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserId",
                table: "Doctors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                table: "Patients",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecommendations_DiseaseId",
                table: "TreatmentRecommendations",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecommendations_MedicineId",
                table: "TreatmentRecommendations",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIConversations");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "TreatmentRecommendations");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
