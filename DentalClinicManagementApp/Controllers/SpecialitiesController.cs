using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentalClinicManagementApp.Data;
using DentalClinicManagementApp.Models;
using DentalClinicManagementApp.Lib;

namespace DentalClinicManagementApp.Controllers
{
    public class SpecialitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecialitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Specialities
        public async Task<IActionResult> Index(string sort, string searchName, int? pageNumber)
        {

            if (_context.Specialities == null)
            {
                Problem("Entity set 'ApplicationDbContext.Specialities'  is null.");
            }


            ViewData["SearchName"] = searchName;
            ViewData["Sort"] = sort;
            ViewData["pageNumber"] = pageNumber;

            var itemsSql = from i in _context.Specialities select i;


            if (!string.IsNullOrEmpty(searchName))
            {
                itemsSql = itemsSql.Where(i => i.SpecialityName.Contains(searchName));
            }


            switch (sort)
            {
                case "speciality_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.SpecialityName);
                    break;
                case "speciality_asc":
                    itemsSql = itemsSql.OrderBy(x => x.SpecialityName);
                    break;
            }

            if (sort == "speciality_desc")
                ViewData["SpecialitySort"] = "speciality_asc";
            else
                ViewData["SpecialitySort"] = "speciality_desc";


            int pageSize = 10;

            var items = await PaginatedList<Speciality>.CreateAsync(itemsSql, pageNumber ?? 1, pageSize);

            return View(items);
        }

        // GET: Specialities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var speciality = await _context.Specialities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (speciality == null)
            {
                return NotFound();
            }

            return View(speciality);
        }

        // GET: Specialities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SpecialityName")] Speciality speciality)
        {
            if (ModelState.IsValid)
            {
                _context.Add(speciality);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(speciality);
        }

        // GET: Specialities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var speciality = await _context.Specialities.FindAsync(id);
            if (speciality == null)
            {
                return NotFound();
            }
            return View(speciality);
        }

        // POST: Specialities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SpecialityName")] Speciality speciality)
        {
            if (id != speciality.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speciality);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialityExists(speciality.ID))
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
            return View(speciality);
        }

        // GET: Specialities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var speciality = await _context.Specialities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (speciality == null)
            {
                return NotFound();
            }

            return View(speciality);
        }

        // POST: Specialities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Specialities == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Specialities'  is null.");
            }
            var speciality = await _context.Specialities.FindAsync(id);
            if (speciality != null)
            {
                _context.Specialities.Remove(speciality);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialityExists(int id)
        {
          return (_context.Specialities?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
