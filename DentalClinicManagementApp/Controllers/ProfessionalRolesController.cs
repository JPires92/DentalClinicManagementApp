using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentalClinicManagementApp.Data;
using DentalClinicManagementApp.Models;

namespace DentalClinicManagementApp.Controllers
{
    public class ProfessionalRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessionalRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProfessionalRoles
        public async Task<IActionResult> Index()
        {
              return _context.ProfessionalRoles != null ? 
                          View(await _context.ProfessionalRoles.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ProfessionalRoles'  is null.");
        }

        // GET: ProfessionalRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProfessionalRoles == null)
            {
                return NotFound();
            }

            var professionalRole = await _context.ProfessionalRoles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (professionalRole == null)
            {
                return NotFound();
            }

            return View(professionalRole);
        }

        // GET: ProfessionalRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProfessionalRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RoleName")] ProfessionalRole professionalRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(professionalRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(professionalRole);
        }

        // GET: ProfessionalRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProfessionalRoles == null)
            {
                return NotFound();
            }

            var professionalRole = await _context.ProfessionalRoles.FindAsync(id);
            if (professionalRole == null)
            {
                return NotFound();
            }
            return View(professionalRole);
        }

        // POST: ProfessionalRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,RoleName")] ProfessionalRole professionalRole)
        {
            if (id != professionalRole.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professionalRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessionalRoleExists(professionalRole.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(professionalRole);
        }

        // GET: ProfessionalRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProfessionalRoles == null)
            {
                return NotFound();
            }

            var professionalRole = await _context.ProfessionalRoles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (professionalRole == null)
            {
                return NotFound();
            }

            return View(professionalRole);
        }

        // POST: ProfessionalRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProfessionalRoles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProfessionalRoles'  is null.");
            }
            var professionalRole = await _context.ProfessionalRoles.FindAsync(id);
            if (professionalRole != null)
            {
                _context.ProfessionalRoles.Remove(professionalRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessionalRoleExists(int id)
        {
          return (_context.ProfessionalRoles?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
