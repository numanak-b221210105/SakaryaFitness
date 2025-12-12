using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SakaryaFitnessApp.Data;
using SakaryaFitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SakaryaFitnessApp.Controllers
{
    [Authorize]
    [Route("Randevular")]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LİSTELEME
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var randevular = _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .AsQueryable();

            // SADECE ÜYE İSE, KENDİSİNİN ALDIĞI RANDEVULARI GÖSTER
            if (!isAdmin)
            {
                randevular = randevular.Where(a => a.MemberId == userId);
            }

            return View(await randevular.OrderBy(a => a.Date).ToListAsync());
        }

        // DETAY
        [Route("Detay/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            // Yetki Kontrolü: Admin değilse ve kendi randevusu değilse yasakla
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && appointment.MemberId != userId) return Forbid();

            return View(appointment);
        }

        // YENİ EKLEME (GET) - SADECE ÜYELER İÇİN
        [Authorize(Roles = "Member")] // <<< SADECE ÜYELERİN YENİ OLUŞTURMASINI SAĞLAR
        [Route("Yeni")]
        public IActionResult Create()
        {
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // YENİ EKLEME (POST) - SADECE ÜYELER İÇİN
        [Authorize(Roles = "Member")] // <<< SADECE ÜYELERİN YENİ OLUŞTURMASINI SAĞLAR
        [HttpPost("Yeni")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,TrainerId,ServiceId")] Appointment appointment)
        {
            appointment.Date = DateTime.SpecifyKind(appointment.Date, DateTimeKind.Utc);
            
            appointment.MemberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            appointment.Status = "Onay Bekliyor"; // Randevuyu "Onay Bekliyor" olarak başlatır

            // Çakışma Kontrolü
            bool cakisma = _context.Appointments.Any(a => 
                a.TrainerId == appointment.TrainerId && 
                a.Date == appointment.Date);

            if (cakisma) ModelState.AddModelError("", "Seçilen antrenör bu saatte dolu.");

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            return View(appointment);
        }

        // DÜZENLEME (GET)
        [Route("Duzenle/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            // Yetki Kontrolü: Admin değilse ve kendi randevusu değilse yasakla
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && appointment.MemberId != userId) return Forbid();

            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            return View(appointment);
        }

        // DÜZENLEME (POST)
        [HttpPost("Duzenle/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,TrainerId,ServiceId,MemberId,Status")] Appointment appointment)
        {
            if (id != appointment.Id) return NotFound();

            appointment.Date = DateTime.SpecifyKind(appointment.Date, DateTimeKind.Utc);

            // Çakışma Kontrolü
            bool cakisma = _context.Appointments.Any(a => 
                a.TrainerId == appointment.TrainerId && 
                a.Date == appointment.Date &&
                a.Id != id);

            if (cakisma) ModelState.AddModelError("", "Seçilen antrenör bu saatte dolu.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            return View(appointment);
        }

        // SİLME (GET)
        [Route("Sil/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            // Yetki Kontrolü: Admin değilse ve kendi randevusu değilse yasakla
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && appointment.MemberId != userId) return Forbid();

            return View(appointment);
        }

        // SİLME (POST)
        [HttpPost("Sil/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null) _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id) => _context.Appointments.Any(e => e.Id == id);
    }
}