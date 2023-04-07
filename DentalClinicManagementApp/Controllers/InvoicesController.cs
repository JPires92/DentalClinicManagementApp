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
    [Authorize(Policy = AppConstants.APP_ADMIN_POLICY)]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> InvoiceAmount(int? ano)
        {
             

            ViewBag.Ano = ano;

            //Lista de Anos
            var anos = _context.Invoices.Where(f => f.PaymentDate != null)
                      .Select(f => f.PaymentDate.Value.Year)
                      .Distinct()
                      .ToList();
            ano = ano.HasValue ? ano : anos[0];
            ViewData["ListaAnos"]= anos;

            var valoresMensais = new Dictionary<int, decimal>();
      
            for (int mes = 1; mes <= 12; mes++)
            {
                var faturasPagas = _context.Invoices
                    .Where(f => f.PaymentDate != null && f.PaymentDate.Value.Month == mes && f.PaymentDate.Value.Year == ano)
                    .ToList();

                decimal valorMensal = faturasPagas.Sum(f => f.FinalValue);

                valoresMensais.Add(mes, decimal.Round(valorMensal,2));
            }

            return View("InvoiceAmount", valoresMensais);

        }

        // GET: Invoices
        public async Task<IActionResult> Index(string sort, string searchName, int? pageNumber)
        {
            //var applicationDbContext = _context.Invoices.Include(i => i.Client).Include(i => i.MedicalAppointment);
            //return View(await applicationDbContext.ToListAsync());

            if (_context.Invoices == null)
            {
                Problem("Entity set 'ApplicationDbContext.Invoices'  is null.");
            }

            ViewData["SearchName"] = searchName;
            ViewData["Sort"] = sort;
            ViewData["pageNumber"] = pageNumber;

            var itemsSql = from i in _context.Invoices!.Include(i => i.Client).Include(m => m.MedicalAppointment).OrderBy(x => x.InvoiceNumber) select i;


            if (!string.IsNullOrEmpty(searchName))
            {
                itemsSql = itemsSql.Where(i => i.InvoiceNumber.Contains(searchName) || i.Client!.Name.Contains(searchName) 
                || i.PaymentDate.ToString().Contains(searchName));
            }


            switch (sort)
            {
                case "number_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.InvoiceNumber);
                    break;
                case "number_asc":
                    itemsSql = itemsSql.OrderBy(x => x.InvoiceNumber);
                    break;
                case "client_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Client!.Name);
                    break;
                case "client_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Client!.Name);
                    break;
                case "state_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.State);
                    break;
                case "state_asc":
                    itemsSql = itemsSql.OrderBy(x => x.State);
                    break;
                case "data_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.PaymentDate);
                    break;
                case "data_asc":
                    itemsSql = itemsSql.OrderBy(x => x.PaymentDate);
                    break;


            }
            Decimal valorTotalFaturasNaoPagas = _context.Invoices
                                            .Where(f => f.State == false)
                                            .Sum(f => f.FinalValue);

            ViewData["ValorReceber"] = Decimal.Round(valorTotalFaturasNaoPagas,2);

            ViewData["NumberSort"] = (sort == "number_desc") ? "number_asc" : "number_desc";
            ViewData["ClientSort"] = (sort == "client_desc") ? "client_asc" : "client_desc";
            ViewData["StateSort"] = (sort == "state_desc") ? "state_asc" : "state_desc";
            ViewData["DataSort"] = (sort == "data_desc") ? "data_asc" : "data_desc";

            int pageSize = 10;

            var items = await PaginatedList<Invoice>.CreateAsync(itemsSql, pageNumber ?? 1, pageSize);

            return View(items);

        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.MedicalAppointment)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
           
            var consultasNaoPagas = _context.MedicalAppointments
                .GroupJoin(_context.Invoices, a => a.ID, f => f.MedicalAppointmentID, (a, f) => new { Appointment = a, Invoice = f.FirstOrDefault() })
                .Where(c => !c.Invoice!.State && c.Appointment.Performed)
                .Select(c => new {ID = c.Appointment.ID, Name = $"Nº: {c.Appointment.ID}, Date: {c.Appointment.DateOfAppointment}" })
                .ToList();

            //ViewData["MedicalAppointmentID"] = new SelectList(consultasNaoPagas, "ID", "Name");
            ViewData["MedicalAppointmentID"] = consultasNaoPagas.Count != 0 ? new SelectList(consultasNaoPagas, "ID", "Name") : new SelectList(Enumerable.Empty<SelectListItem>());

            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Description,FinalValue,State,PaymentDate,MedicalAppointmentID")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                MedicalAppointment aux = _context.MedicalAppointments.Find(invoice.MedicalAppointmentID)!;
                invoice.ClientID = aux.ClientID;

                int maxID = _context.Invoices.Any() ? _context.Invoices.Max(e => e.ID) : 1;
                invoice.InvoiceNumber = "FT00000"+ (maxID+1).ToString();
                
                invoice.State = invoice.PaymentDate.HasValue? true: false;

                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         
            var consultasNaoPagas = _context.MedicalAppointments
              .GroupJoin(_context.Invoices, a => a.ID, f => f.MedicalAppointmentID, (a, f) => new { Appointment = a, Invoice = f.FirstOrDefault() })
              .Where(c => !c.Invoice!.State && c.Appointment.Performed)
              .Select(c => new { ID = c.Appointment.ID, Name = $"Nº: {c.Appointment.ID}, Date: {c.Appointment.DateOfAppointment}" })
              .ToList();

            ViewData["MedicalAppointmentID"] = consultasNaoPagas.Count != 0 ? new SelectList(consultasNaoPagas, "ID", "Name", invoice.MedicalAppointmentID) : new SelectList(Enumerable.Empty<SelectListItem>());


            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var consultasNaoPagas = _context.MedicalAppointments
                .GroupJoin(_context.Invoices, a => a.ID, f => f.MedicalAppointmentID, (a, f) => new { Appointment = a, Invoice = f.FirstOrDefault() })
                .Where(c => c.Appointment.Performed)
                .Select(c => new { ID = c.Appointment.ID, Name = $"Nº: {c.Appointment.ID}, Date: {c.Appointment.DateOfAppointment}" })
                .ToList();
            //ViewData["MedicalAppointmentID"] = new SelectList(consultasNaoPagas, "ID", "Name", invoice.MedicalAppointmentID);
            ViewData["MedicalAppointmentID"] = consultasNaoPagas.Count != 0 ? new SelectList(consultasNaoPagas, "ID", "Name", invoice.MedicalAppointmentID) : new SelectList(Enumerable.Empty<SelectListItem>());

            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,InvoiceNumber,Description,FinalValue,State,PaymentDate,MedicalAppointmentID")] Invoice invoice)
        {
            if (id != invoice.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MedicalAppointment aux = _context.MedicalAppointments.Find(invoice.MedicalAppointmentID)!;
                    invoice.ClientID = aux.ClientID;

                    invoice.State = invoice.PaymentDate.HasValue ? true : false;

                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.ID))
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
            
            var consultasNaoPagas = _context.MedicalAppointments
                .GroupJoin(_context.Invoices, a => a.ID, f => f.MedicalAppointmentID, (a, f) => new { Appointment = a, Invoice = f.FirstOrDefault() })
                .Where(c => c.Appointment.Performed)
                .Select(c => new { ID = c.Appointment.ID, Name = $"Nº: {c.Appointment.ID}, Date: {c.Appointment.DateOfAppointment}" })
                .ToList();

            //ViewData["MedicalAppointmentID"] = new SelectList(consultasNaoPagas, "ID", "Name", invoice.MedicalAppointmentID);


            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.MedicalAppointment)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
          return (_context.Invoices?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
