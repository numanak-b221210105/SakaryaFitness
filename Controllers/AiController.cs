using Microsoft.AspNetCore.Mvc;

namespace SakaryaFitnessApp.Controllers
{
    [Route("YapayZeka")]
    public class AiController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Olustur")]
        public IActionResult GeneratePlan(int age, int weight, int height, string goal, string gender)
        {
            // Vücut Kitle İndeksi (VKİ) Hesapla
            double heightInMeters = height / 100.0;
            double bmi = weight / (heightInMeters * heightInMeters);
            
            string plan = "";
            string status = "";

            // 1. Durum Analizi
            if (bmi < 18.5) status = "Zayıf";
            else if (bmi < 25) status = "Normal Kilolu";
            else if (bmi < 30) status = "Fazla Kilolu";
            else status = "Obezite";

            // 2. Kişiye Özel Plan Oluşturma (Simüle Edilmiş AI)
            plan += $"<h4>Analiz Sonucu: {status} (VKİ: {bmi:F1})</h4>";
            plan += "<hr/>";

            if (goal == "kilo_ver")
            {
                plan += "<h5>📉 Kilo Verme Odaklı Programın:</h5>";
                plan += "<ul>";
                plan += "<li><strong>Sabah:</strong> Yulaf ezmesi, 2 haşlanmış yumurta, yeşil çay.</li>";
                plan += "<li><strong>Öğle:</strong> Izgara tavuk göğsü, bol yeşillikli salata (yağsız).</li>";
                plan += "<li><strong>Ara Öğün:</strong> 1 adet yeşil elma veya 5 badem.</li>";
                plan += "<li><strong>Akşam:</strong> Zeytinyağlı sebze yemeği, yoğurt.</li>";
                plan += "<li><strong>Egzersiz:</strong> Haftada 4 gün 45 dakika Kardiyo (Koşu/Bisiklet).</li>";
                plan += "</ul>";
            }
            else if (goal == "kas_yap")
            {
                plan += "<h5>💪 Kas Kazanma Odaklı Programın:</h5>";
                plan += "<ul>";
                plan += "<li><strong>Sabah:</strong> 3 yumurta, beyaz peynir, tam buğday ekmeği.</li>";
                plan += "<li><strong>Öğle:</strong> Kırmızı et veya hindi, bulgur pilavı, ayran.</li>";
                plan += "<li><strong>Antrenman Öncesi:</strong> Muz ve fıstık ezmesi.</li>";
                plan += "<li><strong>Akşam:</strong> Somon balığı veya ton balığı, haşlanmış patates.</li>";
                plan += "<li><strong>Egzersiz:</strong> Haftada 5 gün Ağırlık Antrenmanı (Hipertrofi odaklı).</li>";
                plan += "</ul>";
            }
            else // form_koru
            {
                plan += "<h5>⚖️ Form Koruma Programın:</h5>";
                plan += "<ul>";
                plan += "<li><strong>Beslenme:</strong> Dengeli protein ve karbonhidrat alımı. Şekerden uzak dur.</li>";
                plan += "<li><strong>Su Tüketimi:</strong> Günde en az 2.5 litre su içmelisin.</li>";
                plan += "<li><strong>Egzersiz:</strong> Haftada 3 gün tüm vücut (Full Body) antrenmanı.</li>";
                plan += "</ul>";
            }

            // Sonucu ekrana geri gönder
            ViewBag.Plan = plan;
            ViewBag.UserAge = age;
            ViewBag.UserWeight = weight;
            ViewBag.UserHeight = height;
            
            return View("Index");
        }
    }
}