using System.ComponentModel.DataAnnotations;

namespace SakaryaFitnessApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Antrenör Adı")]
        public string FullName { get; set; } = string.Empty; // <-- Uyarı giderildi

        [Display(Name = "Uzmanlık Alanı")]
        public string Expertise { get; set; } = string.Empty; // <-- Uyarı giderildi

        [Display(Name = "Fotoğraf")]
        public string? ImageUrl { get; set; } 

        // İlişkiler
        public ICollection<Appointment>? Appointments { get; set; } // <-- Null atanabilir
    }
}