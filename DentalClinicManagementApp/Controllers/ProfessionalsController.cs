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
    public class ProfessionalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessionalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Professionals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Professionals.Include(p => p.PostalCode).Include(p => p.ProfessionalRole).Include(p => p.Speciality);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Professionals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Professionals == null)
            {
                return NotFound();
            }

            var professional = await _context.Professionals
                .Include(p => p.PostalCode)
                .Include(p => p.ProfessionalRole)
                .Include(p => p.Speciality)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (professional == null)
            {
                return NotFound();
            }

            return View(professional);
        }

        // GET: Professionals/Create
        public IActionResult Create()
        {
            ViewData["PostalCodeID"] = new SelectList(_context.PostalCodes.Select(p => new { Id = p.ZipCode, Name = $"{p.ZipCode} {p.Location}"}), "Id", "Name");
            ViewData["ProfessionalRoleID"] = new SelectList(_context.ProfessionalRoles, "ID", "RoleName");
            
            var specialities = _context.Specialities.ToList();
            specialities.Insert(0, new Speciality { ID = 0, SpecialityName = "-- Select a speciality --" });
            ViewData["SpecialityID"] = new SelectList(specialities, "ID", "SpecialityName");


            return View();
        }

        // POST: Professionals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,DateOfBirth,NIF,Address,PostalCodeID,ProfessionalRoleID,SpecialityID")] Professional professional)
        {
            if (ModelState.IsValid)
            {
                if(professional.SpecialityID == 0)
                {
                    professional.SpecialityID = null;
                }

                ProfessionalRole aux = _context.ProfessionalRoles.Find(professional.ProfessionalRoleID)!;
                string _professionalRole = aux.RoleName.ToUpper();
                if (!(_professionalRole.Equals("MEDICO") || _professionalRole.Equals("MÉDICO")))
                {
                    //Especialidade deve ser null
                    professional.SpecialityID = null;
                }

                _context.Add(professional);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PostalCodeID"] = new SelectList(_context.PostalCodes.Select(p => new { Id = p.ZipCode, Name = $"{p.ZipCode} {p.Location}" }), "Id", "Name");
            ViewData["ProfessionalRoleID"] = new SelectList(_context.ProfessionalRoles, "ID", "RoleName", professional.ProfessionalRoleID);

            var specialities = _context.Specialities.ToList();
            specialities.Insert(0, new Speciality { ID = 0, SpecialityName = "-- Select a speciality --" });
            ViewData["SpecialityID"] = new SelectList(specialities, "ID", "SpecialityName", professional.SpecialityID);


            return View(professional);
        }

        // GET: Professionals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Professionals == null)
            {
                return NotFound();
            }

            var professional = await _context.Professionals.FindAsync(id);
            if (professional == null)
            {
                return NotFound();
            }

            ViewData["PostalCodeID"] = new SelectList(_context.PostalCodes.Select(p => new { Id = p.ZipCode, Name = $"{p.ZipCode} {p.Location}" }), "Id", "Name", professional.PostalCodeID);
            ViewData["ProfessionalRoleID"] = new SelectList(_context.ProfessionalRoles, "ID", "RoleName", professional.ProfessionalRoleID);
            
            var specialities = _context.Specialities.ToList();
            specialities.Insert(0, new Speciality { ID = 0, SpecialityName = "-- Select a speciality --" });
            ViewData["SpecialityID"] = new SelectList(specialities, "ID", "SpecialityName", professional.SpecialityID is null? 0 : professional.SpecialityID);
            
            return View(professional);
        }

        // POST: Professionals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,DateOfBirth,NIF,Address,PostalCodeID,ProfessionalRoleID,SpecialityID")] Professional professional)
        {
            if (id != professional.ID)
            {
                return NotFound();
            }

            if (professional.SpecialityID == 0)
            {
                professional.SpecialityID = null;
            }


            if (ModelState.IsValid)
            {
                try
                {
                    ProfessionalRole aux = _context.ProfessionalRoles.Find(professional.ProfessionalRoleID)!;
                    string _professionalRole = aux.RoleName.ToUpper();
                    if (!(_professionalRole.Equals("MEDICO") || _professionalRole.Equals("MÉDICO")))
                    {
                        //Especialidade deve ser null
                        professional.SpecialityID = null;
                    }

                    _context.Update(professional);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessionalExists(professional.ID))
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

            ViewData["PostalCodeID"] = new SelectList(_context.PostalCodes.Select(p => new { Id = p.ZipCode, Name = $"{p.ZipCode} {p.Location}" }), "Id", "Name", professional.PostalCodeID);
            ViewData["ProfessionalRoleID"] = new SelectList(_context.ProfessionalRoles, "ID", "RoleName", professional.ProfessionalRoleID);
            
            var specialities = _context.Specialities.ToList();
            specialities.Insert(0, new Speciality { ID = 0, SpecialityName = "-- Select a speciality --" });
            ViewData["SpecialityID"] = new SelectList(specialities, "ID", "SpecialityName", professional.SpecialityID);
            
            return View(professional);
        }

        // GET: Professionals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Professionals == null)
            {
                return NotFound();
            }

            var professional = await _context.Professionals
                .Include(p => p.PostalCode)
                .Include(p => p.ProfessionalRole)
                .Include(p => p.Speciality)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (professional == null)
            {
                return NotFound();
            }

            return View(professional);
        }

        // POST: Professionals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Professionals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Professionals'  is null.");
            }
            var professional = await _context.Professionals.FindAsync(id);
            if (professional != null)
            {
                _context.Professionals.Remove(professional);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessionalExists(int id)
        {
          return (_context.Professionals?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
