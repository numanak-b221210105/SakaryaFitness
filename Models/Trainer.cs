using System.ComponentModel.DataAnnotations;

namespace SakaryaFitnessApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Antrenör Adı")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Uzmanlık Alanı")]
        public string Expertise { get; set; } = string.Empty;

        [Display(Name = "Fotoğraf")]
        public string? ImageUrl { get; set; }

        // --- YENİ EKLENEN ALAN ---
        [Display(Name = "Hakkında / Biyografi")]
        public string? Description { get; set; } // Antrenör hakkında detaylı bilgi
        // -------------------------

        // İlişkiler
        public ICollection<Appointment>? Appointments { get; set; }
    }
}