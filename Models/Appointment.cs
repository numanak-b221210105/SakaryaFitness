using System.ComponentModel.DataAnnotations;

namespace SakaryaFitnessApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Display(Name = "Tarih ve Saat")]
        public DateTime Date { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Bekliyor"; // Onaylandı, İptal, Bekliyor

        // İlişkiler
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        // Üye bilgisi Identity kütüphanesinden gelecek, şimdilik string tutalım
        public string MemberId { get; set; } 
    }
}