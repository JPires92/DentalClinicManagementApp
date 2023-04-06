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
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("HitoryAppointmentsClient")]
        public async Task<IActionResult> HitoryAppointmentsClient(int id, int? pageNumber)
        {
            ViewData["pageNumber"] = pageNumber;
            ViewData["ID"] = id;

            var itemsSql = from i in _context.MedicalAppointments!.Include(m => m.Client).Include(m => m.Professional).OrderBy(x => x.DateOfAppointment) select i;
            itemsSql = itemsSql.Where(i => i.ClientID == id && i.DateOfAppointment < DateTime.Now);

            int pageSize = 10;
            var items = await PaginatedList<MedicalAppointment>.CreateAsync(itemsSql, pageNumber ?? 1, pageSize);

            return View(items);
        }

        [HttpGet("FutureAppointmentsClient")]
        public async Task<IActionResult> FutureAppointmentsClient(int id, int? pageNumber)
        {
            ViewData["pageNumber"] = pageNumber;
            ViewData["ID"] = id;

            var itemsSql = from i in _context.MedicalAppointments!.Include(m => m.Client).Include(m => m.Professional).OrderBy(x => x.DateOfAppointment) select i;
            itemsSql = itemsSql.Where(i => i.ClientID == id && i.DateOfAppointment >= DateTime.Now);


            int pageSize = 10;
            var items = await PaginatedList<MedicalAppointment>.CreateAsync(itemsSql, pageNumber ?? 1, pageSize);

            return View(items);
        }


        // GET: Clients
        public async Task<IActionResult> Index(string sort, string searchName, int? pageNumber)
        {
            //var applicationDbContext = _context.Clients.Include(c => c.PostalCode);
            //return View(await applicationDbContext.ToListAsync());


            if (_context.Clients == null)
            {
                Problem("Entity set 'ApplicationDbContext.Clients'  is null.");
            }

            ViewData["SearchName"] = searchName;
            ViewData["Sort"] = sort;
            ViewData["pageNumber"] = pageNumber;

            var itemsSql = from i in _context.Clients!.Include(p => p.PostalCode).OrderBy(x => x.Name) select i;


            if (!string.IsNullOrEmpty(searchName))
            {
                itemsSql = itemsSql.Where(i => i.Name.Contains(searchName) || i.NIF.ToString().Contains(searchName));
            }


            switch (sort)
            {
                case "name_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Name);
                    break;
                case "name_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Name);
                    break;
                case "birth_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.DateOfBirth);
                    break;
                case "birth_asc":
                    itemsSql = itemsSql.OrderBy(x => x.DateOfBirth);
                    break;
                case "nif_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.NIF);
                    break;
                case "nif_asc":
                    itemsSql = itemsSql.OrderBy(x => x.NIF);
                    break;
                case "health_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.HealthInsuranceCompany);
                    break;
                case "health_asc":
                    itemsSql = itemsSql.OrderBy(x => x.HealthInsuranceCompany);
                    break;


            }

            ViewData["NameSort"] = (sort == "name_desc") ? "name_asc" : "name_desc";
            ViewData["BirthSort"] = (sort == "birth_desc") ? "birth_asc" : "birth_desc";
            ViewData["NIFSort"] = (sort == "nif_desc") ? "nif_asc" : "nif_desc";
            ViewData["HealthSort"] = (sort == "health_desc") ? "health_asc" : "health_desc";


            int pageSize = 10;

            var items = await PaginatedList<Client>.CreateAsync(itemsSql, pageNumber ?? 1, pageSize);

            return View(items);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.PostalCode)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["PostalCodeID"] = new SelectList(_context.PostalCodes.Select(p => new { Id = p.ZipCode, Name = $"{p.ZipCode} {p.Location}" }), "Id", "Name");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,DateOfBirth,NIF,HealthInsuranceCompany,Address,PostalCodeID")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostalCodeID"] = new SelectList(_context.PostalCodes.Select(p => new { Id = p.ZipCode, Name = $"{p.ZipCode} {p.Location}" }), "Id", "Name", client.PostalCodeID);
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["PostalCodeID"] = new SelectList(_context.PostalCodes.Select(p => new { Id = p.ZipCode, Name = $"{p.ZipCode} {p.Location}" }), "Id", "Name", client.PostalCodeID);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,DateOfBirth,NIF,HealthInsuranceCompany,Address,PostalCodeID")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
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
            ViewData["PostalCodeID"] = new SelectList(_context.PostalCodes.Select(p => new { Id = p.ZipCode, Name = $"{p.ZipCode} {p.Location}" }), "Id", "Name", client.PostalCodeID);
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.PostalCode)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Clients?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
