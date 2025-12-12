using System.ComponentModel.DataAnnotations;

namespace SakaryaFitnessApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } = string.Empty; // <-- Uyarı giderildi

        [Display(Name = "Süre (dk)")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Fiyat (TL)")]
        public decimal Price { get; set; }

        [Display(Name = "Resim URL")]
        public string? ImageUrl { get; set; }

        // İlişkiler
        public ICollection<Appointment>? Appointments { get; set; } // <-- Null atanabilir
    }
}