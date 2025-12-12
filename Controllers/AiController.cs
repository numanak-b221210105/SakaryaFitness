using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels; 
using OpenAI.Interfaces;
using System.Linq; 
using System.Collections.Generic;

namespace SakaryaFitnessApp.Controllers
{
    [Route("YapayZeka")]
    public class AiController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly OpenAIService? _openAIService;

        public AiController(IConfiguration configuration)
        {
            _configuration = configuration;
            var apiKey = _configuration["OpenAI:ApiKey"];

            if (!string.IsNullOrEmpty(apiKey) && !apiKey.Contains("PLACEHOLDER") && !apiKey.Contains("BURAYA"))
            {
                _openAIService = new OpenAIService(new OpenAI.OpenAiOptions() { ApiKey = apiKey });
            }
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("GeneratePlan")] 
        public async Task<IActionResult> GeneratePlan(int age, int weight, int height, string goal, string gender)
        {
            // VKİ Hesapla
            double heightInMeters = height / 100.0;
            double bmi = weight / (heightInMeters * heightInMeters);
            string status = bmi < 18.5 ? "Zayıf" : (bmi < 25 ? "Normal" : (bmi < 30 ? "Fazla Kilolu" : "Obezite"));

            string planContent = "";
            string generatedImageUrl = "";
            bool apiBasarili = false;

            // 1. GERÇEK YAPAY ZEKA DENEMESİ
            if (_openAIService != null)
            {
                try
                {
                    var goalText = goal == "kilo_ver" ? "Kilo vermek" : (goal == "kas_yap" ? "Kas yapmak" : "Form korumak");
                    
                    var prompt = $"Sen bir fitness uzmanısın. Yaş: {age}, Cinsiyet: {gender}, Kilo: {weight}, Hedef: {goalText}. Bana 4 haftalık HTML plan hazırla.";
                    var completionResult = await _openAIService.ChatCompletion.CreateCompletion(
                        new ChatCompletionCreateRequest
                        {
                            Messages = new List<ChatMessage> { ChatMessage.FromSystem("Uzmansın."), ChatMessage.FromUser(prompt) },
                            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo, 
                            MaxTokens = 800,
                        });

                    if (completionResult.Successful)
                    {
                        planContent = completionResult.Choices.First().Message.Content;
                        
                        // Resim Oluşturma (DALL-E)
                        var imagePrompt = $"A realistic photo of a fit {gender} working out in a gym, healthy body transformation, cinematic lighting, {goalText}";
                        var imageResult = await _openAIService.Image.CreateImage(new ImageCreateRequest
                        {
                            Prompt = imagePrompt,
                            N = 1,
                            Size = StaticValues.ImageStatics.Size.Size512,
                            ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url
                        });

                        if (imageResult.Successful)
                        {
                            generatedImageUrl = imageResult.Results.First().Url;
                        }
                        
                        apiBasarili = true;
                    }
                }
                catch
                {
                    apiBasarili = false;
                }
            }

            // 2. SİMÜLASYON MODU (GÜNCELLENMİŞ - GERÇEKÇİ FOTOĞRAFLAR)
            if (!apiBasarili)
            {
                planContent = GenerateMockPlan(age, weight, height, goal, status, bmi);
                
                // Unsplash'tan gerçekçi stok fotoğraflar kullanıyoruz
                if (gender == "Erkek")
                {
                    if (goal == "kas_yap") 
                        generatedImageUrl = "https://images.unsplash.com/photo-1583454110551-21f2fa2afe61?w=600&h=400&fit=crop"; // Kaslı Erkek
                    else if (goal == "kilo_ver") 
                        generatedImageUrl = "https://images.unsplash.com/photo-1571019614242-c5c5dee9f50b?w=600&h=400&fit=crop"; // Koşan/Zayıf Erkek
                    else 
                        generatedImageUrl = "https://images.unsplash.com/photo-1483721310020-03333e577078?w=600&h=400&fit=crop"; // Fit Erkek
                }
                else // Kadın
                {
                    if (goal == "kas_yap") 
                        generatedImageUrl = "https://images.unsplash.com/photo-1522898467493-49726bf28798?w=600&h=400&fit=crop"; // Fit Kadın Sporcu
                    else if (goal == "kilo_ver") 
                        generatedImageUrl = "https://images.unsplash.com/photo-1518611012118-696072aa579a?w=600&h=400&fit=crop"; // Zayıf/Koşan Kadın
                    else 
                        generatedImageUrl = "https://images.unsplash.com/photo-1574680096145-d05b474e2155?w=600&h=400&fit=crop"; // Yoga/Fit Kadın
                }
            }

            ViewBag.Plan = planContent.Replace("\n", "<br>");
            ViewBag.ImageUrl = generatedImageUrl;
            
            ViewBag.UserAge = age;
            ViewBag.UserHeight = height;
            ViewBag.UserWeight = weight;
            
            return View("Index");
        }

        // --- YEDEK PLAN ÜRETİCİSİ ---
        private string GenerateMockPlan(int age, int weight, int height, string goal, string status, double bmi)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"<h3>📋 Kişiselleştirilmiş Fitness Programı</h3>");
            sb.Append($"<p><strong>Analiz:</strong> Vücut kitle indeksiniz <strong>{bmi:F1}</strong> ({status}).</p><hr>");
            
            if (goal == "kilo_ver") {
                sb.Append("<h4>🔥 Hedef: Yağ Yakımı</h4><ul><li>Haftada 4 gün 45dk Kardiyo</li><li>Şekersiz Beslenme</li><li>Bol Su Tüketimi</li></ul>");
            } else if (goal == "kas_yap") {
                sb.Append("<h4>💪 Hedef: Kas İnşası</h4><ul><li>Haftada 5 gün Ağırlık Antrenmanı</li><li>Yüksek Proteinli Beslenme</li><li>Düzenli Uyku</li></ul>");
            } else {
                sb.Append("<h4>⚖️ Hedef: Form Koruma</h4><ul><li>Haftada 3 gün Full Body</li><li>Dengeli Karbonhidrat</li></ul>");
            }
            sb.Append("<br><div class='alert alert-light border'><em>*Bu plan ve aşağıdaki görsel, sistem tarafından simüle edilmiştir.</em></div>");
            return sb.ToString();
        }
    }
}