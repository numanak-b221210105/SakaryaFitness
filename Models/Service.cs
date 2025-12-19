using System.ComponentModel.DataAnnotations;

namespace SakaryaFitnessApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Display(Name = "Hizmet Adı")]
        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Hizmet adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        // YENİ: Detay sayfasında ve kartlarda görünecek açıklama
        [Display(Name = "Açıklama / Detaylar")]
        [Required(ErrorMessage = "Hizmet açıklaması zorunludur.")]
        public string Description { get; set; } = string.Empty; 

        [Display(Name = "Süre (dk)")]
        [Range(1, 1440, ErrorMessage = "Süre en az 1 dakika olmalıdır.")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Fiyat (TL)")]
        [Range(0, 100000, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        public decimal Price { get; set; }

        [Display(Name = "Resim URL")]
        public string? ImageUrl { get; set; }

        // YENİ: Hizmeti silmeden gizleyebilmek için
        [Display(Name = "Aktif Hizmet mi?")]
        public bool IsActive { get; set; } = true;

        // İlişkiler
        public ICollection<Appointment>? Appointments { get; set; } 
    }
}