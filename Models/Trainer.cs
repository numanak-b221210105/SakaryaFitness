using System.ComponentModel.DataAnnotations;

namespace SakaryaFitnessApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Antrenör Adı")]
        public string FullName { get; set; }

        [Display(Name = "Uzmanlık Alanı")]
        public string Expertise { get; set; } // Örn: Yoga, Fitness, Pilates

        [Display(Name = "Fotoğraf")]
        public string? ImageUrl { get; set; } // Antrenör resmi için

        // İlişkiler (Daha sonra detaylandırılabilir)
        public ICollection<Appointment>? Appointments { get; set; }
    }
}