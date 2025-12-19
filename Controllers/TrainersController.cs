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

namespace SakaryaFitnessApp.Controllers
{
    // Controller seviyesinde girişi zorunlu kılar (Index hariç)
    [Authorize] 
    [Route("Antrenorler")] 
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Antrenorler (Index) - HERKES GÖREBİLİR
        [AllowAnonymous] 
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trainers.ToListAsync());
        }

        // GET: /Antrenorler/Detay/5 - HERKES GÖREBİLİR
        [AllowAnonymous] 
        [Route("Detay/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var trainer = await _context.Trainers.FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        // GET: /Antrenorler/Yeni - SADECE ADMIN
        [Authorize(Roles = "Admin")] 
        [Route("Yeni")]
        public IActionResult Create() => View();

        // POST: /Antrenorler/Yeni - SADECE ADMIN
        [Authorize(Roles = "Admin")] 
        [HttpPost("Yeni")]
        [ValidateAntiForgeryToken]
        // 👇 AŞAĞIDAKİ SATIRA DİKKAT: "Description" EKLENDİ 👇
        public async Task<IActionResult> Create([Bind("Id,FullName,Expertise,ImageUrl,Description")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        // GET: /Antrenorler/Duzenle/5 - SADECE ADMIN
        [Authorize(Roles = "Admin")]
        [Route("Duzenle/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        // POST: /Antrenorler/Duzenle/5 - SADECE ADMIN
        [Authorize(Roles = "Admin")] 
        [HttpPost("Duzenle/{id?}")]
        [ValidateAntiForgeryToken]
        // 👇 AŞAĞIDAKİ SATIRA DİKKAT: "Description" EKLENDİ 👇
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Expertise,ImageUrl,Description")] Trainer trainer)
        {
            if (id != trainer.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        // GET: /Antrenorler/Sil/5 - SADECE ADMIN
        [Authorize(Roles = "Admin")]
        [Route("Sil/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var trainer = await _context.Trainers.FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        // POST: /Antrenorler/Sil/5 - SADECE ADMIN
        [Authorize(Roles = "Admin")] 
        [HttpPost("Sil/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null) _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id) => _context.Trainers.Any(e => e.Id == id);
    }
}