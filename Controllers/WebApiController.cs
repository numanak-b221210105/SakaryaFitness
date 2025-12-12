using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakaryaFitnessApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SakaryaFitnessApp.Controllers
{
    // Bu kontrolcüye sadece veri istemek için ulaşılır
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WebApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. TÜM ANTRENÖRLERİ GETİR (LINQ Kullanımı)
        // Adres: /api/WebApi/Antrenorler
        [HttpGet("Antrenorler")]
        public async Task<IActionResult> GetTrainers()
        {
            // LINQ ile sadece ihtiyacımız olan verileri seçiyoruz (Projection)
            var trainers = await _context.Trainers
                .Select(t => new 
                {
                    Id = t.Id,
                    AdSoyad = t.FullName,
                    Uzmanlik = t.Expertise,
                    Resim = t.ImageUrl
                })
                .ToListAsync();

            return Ok(trainers);
        }

        // 2. TÜM HİZMETLERİ GETİR
        // Adres: /api/WebApi/Hizmetler
        [HttpGet("Hizmetler")]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.Services
                .Select(s => new 
                {
                    Id = s.Id,
                    HizmetAdi = s.Name,
                    Fiyat = s.Price,
                    Sure = s.DurationMinutes + " Dakika"
                })
                .ToListAsync();

            return Ok(services);
        }

        // 3. DOLU RANDEVULARI GETİR (Raporlama Örneği)
        // Adres: /api/WebApi/DoluSaatler
        [HttpGet("DoluSaatler")]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Select(a => new 
                {
                    Tarih = a.Date.ToString("dd.MM.yyyy HH:mm"),
                    Antrenor = a.Trainer.FullName,
                    Durum = a.Status
                })
                .ToListAsync();

            return Ok(appointments);
        }
    }
}