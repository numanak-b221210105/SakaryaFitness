using System.ComponentModel.DataAnnotations;

namespace SakaryaFitnessApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tarih ve Saat zorunludur.")]
        [Display(Name = "Tarih ve Saat")]
        public DateTime Date { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Onay Bekliyor"; 

        // Trainer İlişkisi
        [Required]
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; } // <-- Uyarı giderildi

        // Service İlişkisi
        [Required]
        public int ServiceId { get; set; }
        public Service? Service { get; set; } // <-- Uyarı giderildi

        // Member İlişkisi
        public string MemberId { get; set; } = string.Empty; // <-- Uyarı giderildi
    }
}