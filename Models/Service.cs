using System.ComponentModel.DataAnnotations;

namespace SakaryaFitnessApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Süre zorunludur.")]
        [Display(Name = "Süre (Dakika)")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Display(Name = "Fiyat (TL)")]
        public decimal Price { get; set; }

        // Yeni eklediğimiz alan:
        [Display(Name = "Hizmet Resmi")]
        public string? ImageUrl { get; set; }
    }
}