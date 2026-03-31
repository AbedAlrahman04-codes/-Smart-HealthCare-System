using Microsoft.AspNetCore.Mvc;
using SmartHealthcareSystem.Data;
using SmartHealthcareSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace SmartHealthcareSystem.Controllers
{
    public class AIAssistantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string PYTHON_API_URL = "http://127.0.0.1:8000";

        public AIAssistantController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: /AIAssistant
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = userId ?? 0;
            ViewBag.UserName = HttpContext.Session.GetString("FullName") ?? "ضيف";
            return View();
        }

        // POST: /AIAssistant/Chat
        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return Json(new
                {
                    success = false,
                    message = "⚠️ يرجى كتابة رسالة أولاً"
                });
            }

            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                // إرسال الرسالة إلى Python API
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                var pythonRequest = new
                {
                    message = request.Message
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(pythonRequest),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.PostAsync(
                    $"{PYTHON_API_URL}/chat_chat_post",
                    content
                );

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var aiResponse = JsonSerializer.Deserialize<PythonChatResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    var aiMessage = aiResponse?.Message ?? "لا يوجد رد";

                    // ✅ حفظ المحادثة في قاعدة البيانات (فقط للمستخدمين المسجلين)
                    if (userId.HasValue && userId.Value > 0)
                    {
                        try
                        {
                            // ✅ تحقق من وجود المستخدم
                            var userExists = await _context.Users
                                .AnyAsync(u => u.UserId == userId.Value);

                            if (userExists)
                            {
                                var conversation = new AIConversation
                                {
                                    UserId = userId.Value,
                                    UserMessage = request.Message,
                                    AIResponse = aiMessage,
                                    DiseaseName = null, // ✅ Optional - يمكن أن يكون null
                                    RecommendedMedicines = null, // ✅ Optional
                                    Confidence = null, // ✅ Optional
                                    Timestamp = DateTime.Now
                                };

                                _context.AIConversations.Add(conversation);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                // المستخدم مو موجود - لكن نكمل ونرجع الرد
                                Console.WriteLine($"User {userId.Value} not found in database");
                            }
                        }
                        catch (Exception dbEx)
                        {
                            // ✅ خطأ في حفظ قاعدة البيانات - لكن نرجع الرد
                            Console.WriteLine($"Database save error: {dbEx.InnerException?.Message ?? dbEx.Message}");
                            // لا نوقف العملية - المستخدم يحصل على رد AI حتى لو ما انحفظ
                        }
                    }

                    return Json(new
                    {
                        success = true,
                        message = aiMessage
                    });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new
                    {
                        success = false,
                        message = $"❌ خطأ من Python API: {response.StatusCode}\n{errorContent}"
                    });
                }
            }
            catch (HttpRequestException ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"❌ لا يمكن الاتصال بـ Python API. تأكد أنه يعمل على المنفذ 8000.\nالخطأ: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"❌ حدث خطأ: {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }

        // GET: /AIAssistant/History
        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue || userId.Value == 0)
            {
                TempData["ErrorMessage"] = "يجب تسجيل الدخول لعرض السجل";
                return RedirectToAction("Login", "Account");
            }

            var conversations = await _context.AIConversations
                .Where(c => c.UserId == userId.Value)
                .OrderByDescending(c => c.Timestamp)
                .Take(50)
                .ToListAsync();

            return View(conversations);
        }

        // POST: /AIAssistant/ClearHistory
        [HttpPost]
        public async Task<IActionResult> ClearHistory()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue || userId.Value == 0)
            {
                return Json(new { success = false, message = "غير مصرح" });
            }

            try
            {
                var conversations = await _context.AIConversations
                    .Where(c => c.UserId == userId.Value)
                    .ToListAsync();

                _context.AIConversations.RemoveRange(conversations);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم مسح السجل" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }

    // Models
    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }

    public class PythonChatResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}