using SmartHealthcareSystem.Data;
using SmartHealthcareSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace SmartHealthcareSystem.Services
{
    public interface IMedicalAIService
    {
        Task<AIAnalysisResult> AnalyzeSymptomsAsync(string symptoms, int userId);
        Task<List<Medicine>> GetAlternativeMedicinesAsync(string activeIngredient);
    }

    public class MedicalAIService : IMedicalAIService
    {
        private readonly ApplicationDbContext _context;

        public MedicalAIService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ═══════════════════════════════════════════════════════════
        // الوظيفة الرئيسية: تحليل الأعراض واقتراح العلاج
        // ═══════════════════════════════════════════════════════════
        public async Task<AIAnalysisResult> AnalyzeSymptomsAsync(string symptoms, int userId)
        {
            try
            {
                // 1. تنظيف النص
                var cleanedSymptoms = CleanText(symptoms);

                // 2. التحقق من الأسئلة العامة
                if (IsGeneralQuestion(cleanedSymptoms))
                {
                    return new AIAnalysisResult
                    {
                        Success = true,
                        Message = GetWelcomeMessage(),
                        Confidence = 100
                    };
                }

                // 3. استخراج الكلمات المفتاحية من الأعراض
                var keywords = ExtractKeywords(cleanedSymptoms);

                if (keywords.Count == 0)
                {
                    return new AIAnalysisResult
                    {
                        Success = false,
                        Message = @"⚠️ لم أتمكن من فهم الأعراض. يرجى وصف الأعراض بشكل أوضح.

📝 **أمثلة صحيحة:**
• عندي صداع شديد وحمى
• ألم في المعدة وغثيان
• سعال والتهاب في الحلق
• body ache and fever",
                        Confidence = 0
                    };
                }

                // 4. البحث عن الأمراض المطابقة
                var diseases = await _context.Diseases
                    .Include(d => d.TreatmentRecommendations)
                    .ThenInclude(tr => tr.Medicine)
                    .ToListAsync();

                var matchedDiseases = FindMatchingDiseases(diseases, keywords);

                if (!matchedDiseases.Any())
                {
                    return new AIAnalysisResult
                    {
                        Success = false,
                        Message = $@"⚠️ الأعراض المذكورة غير موجودة في قاعدة البيانات الحالية.

**الأعراض المكتشفة:** {string.Join(", ", keywords)}

🏥 **يُنصح بـ:**
• استشارة طبيب مختص
• زيارة أقرب مركز صحي

💡 **نصيحة:** حاول وصف الأعراض بشكل أوضح أو استخدم كلمات أخرى.",
                        ExtractedSymptoms = keywords,
                        Confidence = 0
                    };
                }

                // 5. اختيار المرض الأكثر احتمالاً
                var bestMatch = matchedDiseases.OrderByDescending(m => m.MatchScore).First();

                // 6. بناء الرد
                var result = await BuildAnalysisResult(bestMatch, keywords, userId);

                return result;
            }
            catch (Exception ex)
            {
                return new AIAnalysisResult
                {
                    Success = false,
                    Message = $"❌ حدث خطأ في التحليل: {ex.Message}",
                    Confidence = 0
                };
            }
        }

        // ═══════════════════════════════════════════════════════════
        // تنظيف النص
        // ═══════════════════════════════════════════════════════════
        private string CleanText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text.ToLower()
                      .Replace(".", "")
                      .Replace("،", ",")
                      .Replace("؟", "")
                      .Replace("!", "")
                      .Trim();
        }

        // ═══════════════════════════════════════════════════════════
        // استخراج الكلمات المفتاحية
        // ═══════════════════════════════════════════════════════════
        private List<string> ExtractKeywords(string text)
        {
            var keywords = new List<string>();

            // كلمات شائعة نتجاهلها
            var stopWords = new[] { "عندي", "عند", "لدي", "أشعر", "أعاني", "من", "في", "و", "أو", "the", "a", "an", "i", "have", "feel", "my" };

            // تقسيم النص
            var words = text.Split(new[] { ' ', ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                var cleanWord = word.Trim();
                if (cleanWord.Length > 2 && !stopWords.Contains(cleanWord))
                {
                    keywords.Add(cleanWord);
                }
            }

            return keywords.Distinct().ToList();
        }

        // ═══════════════════════════════════════════════════════════
        // البحث عن الأمراض المطابقة
        // ═══════════════════════════════════════════════════════════
        private List<DiseaseMatch> FindMatchingDiseases(List<Disease> diseases, List<string> keywords)
        {
            var matches = new List<DiseaseMatch>();

            foreach (var disease in diseases)
            {
                // تقسيم أعراض المرض
                var diseaseSymptoms = disease.Symptoms.ToLower()
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                // حساب التطابق
                int matchCount = 0;
                var matchedSymptoms = new List<string>();

                foreach (var keyword in keywords)
                {
                    foreach (var symptom in diseaseSymptoms)
                    {
                        if (symptom.Contains(keyword) || keyword.Contains(symptom))
                        {
                            matchCount++;
                            matchedSymptoms.Add(symptom);
                            break;
                        }
                    }
                }

                if (matchCount > 0)
                {
                    double score = ((double)matchCount / keywords.Count) * 100;
                    matches.Add(new DiseaseMatch
                    {
                        Disease = disease,
                        MatchScore = score,
                        MatchedSymptoms = matchedSymptoms
                    });
                }
            }

            return matches;
        }

        // ═══════════════════════════════════════════════════════════
        // بناء نتيجة التحليل
        // ═══════════════════════════════════════════════════════════
        private async Task<AIAnalysisResult> BuildAnalysisResult(DiseaseMatch match, List<string> keywords, int userId)
        {
            var disease = match.Disease;

            // جلب الأدوية الموصى بها
            var recommendedMedicines = disease.TreatmentRecommendations
                .Where(tr => tr.Medicine.IsAvailable)
                .OrderByDescending(tr => tr.Priority)
                .Select(tr => new MedicineRecommendation
                {
                    MedicineId = tr.Medicine.MedicineId,
                    Name = tr.Medicine.Name,
                    GenericName = tr.Medicine.GenericName,
                    ActiveIngredient = tr.Medicine.ActiveIngredient,
                    Category = tr.Medicine.Category,
                    DosageForm = tr.Medicine.DosageForm,
                    Strength = tr.Medicine.Strength,
                    Price = tr.Medicine.Price,
                    ImageUrl = tr.Medicine.ImageUrl,
                    Priority = tr.Priority,
                    Reason = $"موصى به لعلاج {disease.NameArabic}"
                })
                .Take(5)
                .ToList();

            // بناء رسالة AI
            var message = BuildAIMessage(disease, match.MatchScore, recommendedMedicines);

            // حفظ المحادثة
            await SaveConversationAsync(userId, string.Join(", ", keywords), message, disease.Name, match.MatchScore, recommendedMedicines);

            return new AIAnalysisResult
            {
                Success = true,
                Message = message,
                DiseaseName = disease.Name,
                DiseaseNameArabic = disease.NameArabic,
                Severity = disease.Severity,
                Confidence = match.MatchScore,
                ExtractedSymptoms = keywords,
                RecommendedMedicines = recommendedMedicines
            };
        }

        // ═══════════════════════════════════════════════════════════
        // بناء رسالة AI
        // ═══════════════════════════════════════════════════════════
        private string BuildAIMessage(Disease disease, double confidence, List<MedicineRecommendation> medicines)
        {
            var severityArabic = disease.Severity switch
            {
                "Mild" => "خفيفة",
                "Moderate" => "متوسطة",
                "Severe" => "شديدة",
                _ => "غير محددة"
            };

            var message = $@"
🔍 **تحليل الأعراض:**

📊 **التشخيص المحتمل:**
{disease.NameArabic} ({disease.Name})

**مستوى الثقة:** {confidence:F1}%
**درجة الخطورة:** {severityArabic}

📝 **الوصف:**
{disease.Description}

💊 **الأدوية الموصى بها:**
";

            foreach (var med in medicines)
            {
                message += $"\n• **{med.Name}** ({med.GenericName})";
                message += $"\n  الشكل: {med.DosageForm} | التركيز: {med.Strength}";
                message += $"\n  السعر: {med.Price:F2} دينار";
                message += $"\n";
            }

            message += "\n⚠️ **ملاحظة مهمة:**";
            message += "\nهذا التحليل استرشادي فقط ولا يغني عن استشارة الطبيب المختص.";

            return message;
        }

        // ═══════════════════════════════════════════════════════════
        // حفظ المحادثة في قاعدة البيانات
        // ═══════════════════════════════════════════════════════════
        private async Task SaveConversationAsync(int userId, string userMessage, string aiResponse,
            string diseaseName, double confidence, List<MedicineRecommendation> medicines)
        {
            var conversation = new AIConversation
            {
                UserId = userId,
                UserMessage = userMessage,
                AIResponse = aiResponse,
                DiseaseName = diseaseName,
                RecommendedMedicines = JsonSerializer.Serialize(medicines),
                Confidence = confidence,
                Timestamp = DateTime.Now
            };

            _context.AIConversations.Add(conversation);
            await _context.SaveChangesAsync();
        }

        // ═══════════════════════════════════════════════════════════
        // إيجاد بدائل للدواء
        // ═══════════════════════════════════════════════════════════
        public async Task<List<Medicine>> GetAlternativeMedicinesAsync(string activeIngredient)
        {
            return await _context.Medicines
                .Where(m => m.ActiveIngredient == activeIngredient && m.IsAvailable)
                .OrderBy(m => m.Price)
                .ToListAsync();
        }

        // ═══════════════════════════════════════════════════════════
        // التحقق من الأسئلة العامة (دالة جديدة)
        // ═══════════════════════════════════════════════════════════
        private bool IsGeneralQuestion(string text)
        {
            var generalPhrases = new[]
            {
                "مرحبا", "مساء", "صباح", "السلام", "هلا", "اهلا",
                "كيف حالك", "شلونك", "كيفك", "ما هي",
                "what are", "what is", "who are", "hello", "hi",
                "help", "مساعدة", "وظائف", "مهام", "tasks",
                "can you", "هل تستطيع", "هل يمكنك"
            };

            return generalPhrases.Any(phrase => text.Contains(phrase));
        }

        // ═══════════════════════════════════════════════════════════
        // رسالة الترحيب (دالة جديدة)
        // ═══════════════════════════════════════════════════════════
        private string GetWelcomeMessage()
        {
            return @"👋 **مرحباً بك! أنا مساعدك الطبي الذكي**

🎯 **ماذا أستطيع أن أفعل؟**

✅ **تحليل الأعراض**
   أقوم بتحليل الأعراض التي تصفها وأحدد المرض المحتمل

✅ **اقتراح العلاج**
   أوصي بالأدوية المناسبة من قاعدة البيانات الطبية

✅ **عرض البدائل**
   أعرض لك بدائل دوائية بنفس المادة الفعالة وأسعار مختلفة

✅ **دعم اللغتين**
   أفهم العربية والإنجليزية

---

📝 **كيف تستخدمني؟**

اكتب الأعراض التي تعاني منها مباشرة، مثل:

💬 **أمثلة:**
• 'عندي صداع شديد وحمى'
• 'ألم في المعدة وغثيان وقيء'
• 'سعال وصعوبة في التنفس'
• 'fever and body ache'
• 'chest pain and cough'

---

⚠️ **ملاحظة مهمة:**

أنا أداة مساعدة لدعم القرارات الطبية ولا أغني عن:
• استشارة الطبيب المختص
• الفحوصات الطبية اللازمة
• التشخيص الدقيق من قبل محترف

🏥 **في الحالات الطارئة، توجه فوراً لأقرب مستشفى!**

---

✨ **جرّبني الآن! اكتب أعراضك وسأساعدك...**";
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Models للنتائج
    // ═══════════════════════════════════════════════════════════
    public class AIAnalysisResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string DiseaseName { get; set; }
        public string DiseaseNameArabic { get; set; }
        public string Severity { get; set; }
        public double Confidence { get; set; }
        public List<string> ExtractedSymptoms { get; set; } = new List<string>();
        public List<MedicineRecommendation> RecommendedMedicines { get; set; } = new List<MedicineRecommendation>();
    }

    public class MedicineRecommendation
    {
        public int MedicineId { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string ActiveIngredient { get; set; }
        public string Category { get; set; }
        public string DosageForm { get; set; }
        public string Strength { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Priority { get; set; }
        public string Reason { get; set; }
    }

    public class DiseaseMatch
    {
        public Disease Disease { get; set; }
        public double MatchScore { get; set; }
        public List<string> MatchedSymptoms { get; set; }
    }
}