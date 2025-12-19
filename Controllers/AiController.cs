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
        private readonly string _aiModel;

        public AiController(IConfiguration configuration)
        {
            _configuration = configuration;
            
            var apiKey = _configuration["OpenAI:ApiKey"];
            var baseDomain = _configuration["OpenAI:BaseDomain"]; 
            _aiModel = _configuration["OpenAI:Model"] ?? "llama-3.3-70b-versatile"; // En zeki modeli kullanıyoruz

            // API Key kontrolü
            if (!string.IsNullOrEmpty(apiKey) && !apiKey.Contains("BURAYA"))
            {
                _openAIService = new OpenAIService(new OpenAI.OpenAiOptions() 
                { 
                    ApiKey = apiKey,
                    BaseDomain = baseDomain 
                });
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
            // 1. Matematiksel Hesaplamalar (Yapay Zekaya Yardımcı Olmak İçin)
            double heightInMeters = height / 100.0;
            double bmi = weight / (heightInMeters * heightInMeters);
            string status = bmi < 18.5 ? "Zayıf" : (bmi < 25 ? "Normal Kilolu" : (bmi < 30 ? "Fazla Kilolu" : "Obezite Sınırında"));

            string planContent = "";
            string generatedImageUrl = "";
            bool apiBasarili = false;

            // 2. GELİŞMİŞ PROMPT MÜHENDİSLİĞİ
            if (_openAIService != null)
            {
                try
                {
                    var goalText = goal == "kilo_ver" ? "Kilo Vermek ve Yağ Yakmak" : (goal == "kas_yap" ? "Kas Kütlesini Artırmak (Hipertrofi)" : "Mevcut Formu Korumak ve Sıkılaşmak");
                    
                    // BURASI ÇOK ÖNEMLİ: Yapay zekaya detaylı talimat veriyoruz
                    var prompt = $@"
                    Sen dünyanın en iyi spor ve beslenme koçusun. Aşağıdaki danışanın TÜM fiziksel özelliklerini analiz ederek ona özel bir plan yaz.
                    
                    DANIŞAN PROFİLİ:
                    - Yaş: {age} (DİKKAT: Antrenman şiddetini ve dinlenme sürelerini bu yaşa göre ayarla.)
                    - Cinsiyet: {gender} (Biyolojik faktörleri göz önünde bulundur.)
                    - Boy: {height} cm
                    - Kilo: {weight} kg
                    - Vücut Kitle İndeksi (VKİ): {bmi:F1} (Durumu: {status})
                    - Hedef: {goalText}

                    GÖREVİN:
                    Bu kişi için 4 haftalık, nokta atışı bir program hazırla.
                    1. Eğer kişi yaşlıysa eklemleri yormayan, gençse daha dinamik hareketler seç.
                    2. Eğer VKİ yüksekse kardiyo ağırlıklı, düşükse beslenme ağırlıklı tavsiyeler ver.
                    3. {gender} metabolizmasına uygun beslenme tüyoları ekle.

                    ÇIKTI FORMATI (HTML):
                    Sadece HTML etiketleri kullan (h4, h5, ul, li, strong, p). 
                    Asla 'Merhaba', 'İşte planınız' gibi giriş cümleleri yazma. Direkt başlıklarla konuya gir.
                    İçeriği şu başlıklarla ayır: 'Vücut Analizi', 'Antrenman Programı', 'Beslenme Planı'.
                    ";
                    
                    var completionResult = await _openAIService.ChatCompletion.CreateCompletion(
                        new ChatCompletionCreateRequest
                        {
                            Messages = new List<ChatMessage> 
                            { 
                                ChatMessage.FromSystem("Sen çok tecrübeli, bilimsel çalışan ve motive edici bir fitness koçusun."), 
                                ChatMessage.FromUser(prompt) 
                            },
                            Model = _aiModel, 
                            MaxTokens = 1500, // Daha uzun ve detaylı cevap için limiti artırdık
                            Temperature = 0.7f // Yaratıcılık ayarı
                        });

                    if (completionResult.Successful)
                    {
                        planContent = completionResult.Choices.First().Message.Content;
                        apiBasarili = true;
                    }
                    else
                    {
                        var err = completionResult.Error;
                        ViewBag.Error = $"API Hatası: {err?.Message}";
                    }
                }
                catch (System.Exception ex)
                {
                    ViewBag.Error = $"Sistem Hatası: {ex.Message}";
                }
            }

            // 3. RESİM SEÇİMİ (Unsplash - Hedefe Göre Dinamik)
            if (gender == "Erkek")
            {
                if (goal == "kas_yap") generatedImageUrl = "https://images.unsplash.com/photo-1583454110551-21f2fa2afe61?w=600&h=400&fit=crop"; 
                else if (goal == "kilo_ver") generatedImageUrl = "https://images.unsplash.com/photo-1571019614242-c5c5dee9f50b?w=600&h=400&fit=crop"; 
                else generatedImageUrl = "https://images.unsplash.com/photo-1483721310020-03333e577078?w=600&h=400&fit=crop"; 
            }
            else // Kadın
            {
                if (goal == "kas_yap") generatedImageUrl = "https://images.unsplash.com/photo-1522898467493-49726bf28798?w=600&h=400&fit=crop"; 
                else if (goal == "kilo_ver") generatedImageUrl = "https://images.unsplash.com/photo-1518611012118-696072aa579a?w=600&h=400&fit=crop"; 
                else generatedImageUrl = "https://images.unsplash.com/photo-1574680096145-d05b474e2155?w=600&h=400&fit=crop"; 
            }

            // Fallback (Simülasyon)
            if (!apiBasarili)
            {
                planContent = GenerateMockPlan(age, weight, height, goal, status, bmi);
            }

            ViewBag.Plan = planContent.Replace("\n", "<br>");
            ViewBag.ImageUrl = generatedImageUrl;
            
            ViewBag.UserAge = age;
            ViewBag.UserHeight = height;
            ViewBag.UserWeight = weight;
            
            return View("Index");
        }

        private string GenerateMockPlan(int age, int weight, int height, string goal, string status, double bmi)
        {
            return $@"
            <h4 class='text-danger'>Bağlantı Sorunu</h4>
            <p>Üzgünüz, yapay zeka şu an yanıt veremiyor. Ancak senin için temel bir analiz yaptık:</p>
            <ul>
                <li><strong>Durum:</strong> {status} (VKİ: {bmi:F1})</li>
                <li><strong>Öneri:</strong> {goal} hedefine uygun olarak haftada 3 gün spor yapmalısın.</li>
            </ul>";
        }
    }
}