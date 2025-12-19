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
    // Controller seviyesinde sadece Admin yetkisi zorunludur.
    [Authorize(Roles = "Admin")] 
    [Route("Hizmetler")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Hizmetler (Index) - HERKES GÖREBİLİR
        [AllowAnonymous] 
        [Route("")]
        public async Task<IActionResult> Index()
        {
            // Admin her şeyi görür, normal kullanıcı sadece "Aktif" olanları görür
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Services.ToListAsync());
            }
            else
            {
                // IsActive == true olanları getir
                return View(await _context.Services.Where(s => s.IsActive).ToListAsync());
            }
        }
        
        // GET: /Hizmetler/Detay/5 - HERKES GÖREBİLİR
        [AllowAnonymous] 
        [Route("Detay/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            var service = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);
            
            if (service == null) return NotFound();

            return View(service);
        }

        // GET: /Hizmetler/Yeni (Create) - SADECE ADMIN
        [Route("Yeni")]
        public IActionResult Create() => View();

        // POST: /Hizmetler/Yeni (Create) - SADECE ADMIN
        [HttpPost("Yeni")]
        [ValidateAntiForgeryToken]
        // DİKKAT: [Bind] içine Description ve IsActive eklendi!
        public async Task<IActionResult> Create([Bind("Id,Name,Description,DurationMinutes,Price,ImageUrl,IsActive")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: /Hizmetler/Duzenle/5 (Edit) - SADECE ADMIN
        [Route("Duzenle/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();
            return View(service);
        }

        // POST: /Hizmetler/Duzenle/5 (Edit) - SADECE ADMIN
        [HttpPost("Duzenle/{id?}")]
        [ValidateAntiForgeryToken]
        // DİKKAT: [Bind] içine Description ve IsActive eklendi!
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DurationMinutes,Price,ImageUrl,IsActive")] Service service)
        {
            if (id != service.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: /Hizmetler/Sil/5 (Delete) - SADECE ADMIN
        [Route("Sil/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var service = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);
            if (service == null) return NotFound();
            return View(service);
        }

        // POST: /Hizmetler/Sil/5 (DeleteConfirmed) - SADECE ADMIN
        [HttpPost("Sil/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null) _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id) => _context.Services.Any(e => e.Id == id);
    }
}