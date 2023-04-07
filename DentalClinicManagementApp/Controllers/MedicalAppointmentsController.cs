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
using Microsoft.AspNetCore.Authorization;

namespace DentalClinicManagementApp.Controllers
{
    [Authorize]
    public class MedicalAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicalAppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicalAppointments
        public async Task<IActionResult> Index(string sort, string searchName, int? pageNumber)
        {
            //var applicationDbContext = _context.MedicalAppointments.Include(m => m.Client).Include(m => m.Professional);
            //return View(await applicationDbContext.ToListAsync());

            if (_context.MedicalAppointments == null)
            {
                Problem("Entity set 'ApplicationDbContext.MedicalAppointments'  is null.");
            }

            ViewData["SearchName"] = searchName;
            ViewData["Sort"] = sort;
            ViewData["pageNumber"] = pageNumber;

            var itemsSql = from i in _context.MedicalAppointments!.Include(m => m.Client).Include(m => m.Professional).OrderBy(x => x.ID) select i;


            if (!string.IsNullOrEmpty(searchName))
            {
                itemsSql = itemsSql.Where(i => i.ID.ToString().Contains(searchName) || 
                                               i.Client!.Name.Contains(searchName) || i.Professional!.Name.Contains(searchName));
            }


            switch (sort)
            {
                case "number_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.ID);
                    break;
                case "number_asc":
                    itemsSql = itemsSql.OrderBy(x => x.ID);
                    break;
                case "data_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.DateOfAppointment);
                    break;
                case "data_asc":
                    itemsSql = itemsSql.OrderBy(x => x.DateOfAppointment);
                    break;
                case "perform_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Performed);
                    break;
                case "perform_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Performed);
                    break;
                case "client_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Client!.Name);
                    break;
                case "client_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Client!.Name);
                    break;
                case "doctor_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Professional!.Name);
                    break;
                case "doctor_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Professional!.Name);
                    break;


            }

            ViewData["NumberSort"] = (sort == "number_desc") ? "number_asc" : "number_desc";
            ViewData["DataSort"] = (sort == "data_desc") ? "data_asc" : "data_desc";
            ViewData["PerformSort"] = (sort == "perform_desc") ? "perform_asc" : "perform_desc";
            ViewData["ClientSort"] = (sort == "client_desc") ? "client_asc" : "client_desc";
            ViewData["DoctorSort"] = (sort == "doctor_desc") ? "doctor_asc" : "doctor_desc";

            int pageSize = 10;

            var items = await PaginatedList<MedicalAppointment>.CreateAsync(itemsSql, pageNumber ?? 1, pageSize);

            return View(items);

        }

        // GET: MedicalAppointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MedicalAppointments == null)
            {
                return NotFound();
            }

            var medicalAppointment = await _context.MedicalAppointments
                .Include(m => m.Client)
                .Include(m => m.Professional)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medicalAppointment == null)
            {
                return NotFound();
            }

            return View(medicalAppointment);
        }

        // GET: MedicalAppointments/Create
        public IActionResult Create()
        {
            IEnumerable<Professional> aux = _context.Professionals.Where(c => c.ProfessionalRole.RoleName == "Médico").ToList();
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Name");
            ViewData["ProfessionalID"] = new SelectList(aux, "ID", "Name");
            return View();
        }

        // POST: MedicalAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DateOfAppointment,Observations,Performed,ClientID,ProfessionalID")] MedicalAppointment medicalAppointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalAppointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Professional> aux = _context.Professionals.Where(c => c.ProfessionalRole.RoleName == "Médico").ToList();
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Name", medicalAppointment.ClientID);
            ViewData["ProfessionalID"] = new SelectList(aux, "ID", "Name", medicalAppointment.ProfessionalID);
            return View(medicalAppointment);
        }

        // GET: MedicalAppointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MedicalAppointments == null)
            {
                return NotFound();
            }

            var medicalAppointment = await _context.MedicalAppointments.FindAsync(id);
            if (medicalAppointment == null)
            {
                return NotFound();
            }
            IEnumerable<Professional> aux = _context.Professionals.Where(c => c.ProfessionalRole.RoleName == "Médico").ToList();
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Name", medicalAppointment.ClientID);
            ViewData["ProfessionalID"] = new SelectList(aux, "ID", "Name", medicalAppointment.ProfessionalID);
            return View(medicalAppointment);
        }

        // POST: MedicalAppointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DateOfAppointment,Observations,Performed,ClientID,ProfessionalID")] MedicalAppointment medicalAppointment)
        {
            if (id != medicalAppointment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalAppointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalAppointmentExists(medicalAppointment.ID))
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
            IEnumerable<Professional> aux = _context.Professionals.Where(c => c.ProfessionalRole.RoleName == "Médico").ToList();
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Name", medicalAppointment.ClientID);
            ViewData["ProfessionalID"] = new SelectList(aux, "ID", "Name", medicalAppointment.ProfessionalID);
            return View(medicalAppointment);
        }

        // GET: MedicalAppointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MedicalAppointments == null)
            {
                return NotFound();
            }

            var medicalAppointment = await _context.MedicalAppointments
                .Include(m => m.Client)
                .Include(m => m.Professional)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medicalAppointment == null)
            {
                return NotFound();
            }

            return View(medicalAppointment);
        }

        // POST: MedicalAppointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MedicalAppointments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MedicalAppointments'  is null.");
            }
            var medicalAppointment = await _context.MedicalAppointments.FindAsync(id);
            if (medicalAppointment != null)
            {
                _context.MedicalAppointments.Remove(medicalAppointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalAppointmentExists(int id)
        {
          return (_context.MedicalAppointments?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
