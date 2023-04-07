using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentalClinicManagementApp.Data;
using DentalClinicManagementApp.Models;
using Microsoft.AspNetCore.Mvc.Localization;
using NToastNotify;
using DentalClinicManagementApp.Lib;
using Microsoft.AspNetCore.Authorization;

namespace DentalClinicManagementApp.Controllers
{
    [Authorize(Policy = AppConstants.APP_ADMIN_POLICY)]
    public class PostalCodesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IHtmlLocalizer<SharedResource> _sharedLocalizer;

        public PostalCodesController(ApplicationDbContext context, IToastNotification toastNotification,
            IHtmlLocalizer<SharedResource> sharedLocalizer)
        {
            _context = context;
            _toastNotification = toastNotification;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: PostalCodes
        public async Task<IActionResult> Index(string sort, string searchName, int? pageNumber)
        {

            if (_context.PostalCodes == null)
            {
                Problem("Entity set 'ApplicationDbContext.PostalCodes'  is null.");
            }


            ViewData["SearchName"] = searchName;
            ViewData["Sort"] = sort;
            ViewData["pageNumber"] = pageNumber;

            var itemsSql = from i in _context.PostalCodes select i;


            if (!string.IsNullOrEmpty(searchName))
            {
                itemsSql = itemsSql.Where(i => i.Location.Contains(searchName) || i.ZipCode.Contains(searchName));
            }


            switch (sort)
            {
                case "zipcode_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.ZipCode);
                    break;
                case "zipcode_asc":
                    itemsSql = itemsSql.OrderBy(x => x.ZipCode);
                    break;

                case "location_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Location);
                    break;
                case "location_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Location);
                    break;
            }

            if (sort == "zipcode_desc")
                ViewData["ZipCodeSort"] = "zipcode_asc";
            else
                ViewData["ZipCodeSort"] = "zipcode_desc";


            if (sort == "location_desc")
                ViewData["LocationSort"] = "location_asc";
            else
                ViewData["LocationSort"] = "location_desc";


            int pageSize = 10;

            var items = await PaginatedList<PostalCode>.CreateAsync(itemsSql, pageNumber ?? 1, pageSize);

            return View(items);
        }

        // GET: PostalCodes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.PostalCodes == null)
            {
                return NotFound();
            }

            var postalCode = await _context.PostalCodes
                .FirstOrDefaultAsync(m => m.ZipCode == id);
            if (postalCode == null)
            {
                return NotFound();
            }

            return View(postalCode);
        }

        // GET: PostalCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostalCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZipCode,Location")] PostalCode postalCode)
        {
            if (ModelState.IsValid)
            {
                var _postalCode = await _context.PostalCodes.FindAsync(postalCode.ZipCode);

                if (_postalCode == null)
                {
                    _context.Add(postalCode);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage(
                                   string.Format(_sharedLocalizer["Postcode added!"].Value, postalCode.ZipCode),
                                   new ToastrOptions { Title = _sharedLocalizer["New Zip Code"].Value });


                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _toastNotification.AddErrorToastMessage(
                                   string.Format(_sharedLocalizer["Postcode already exists!"].Value, postalCode.ZipCode),
                                   new ToastrOptions { Title = _sharedLocalizer["New Zip Code"].Value });

                    return View(postalCode);
                }

            }
            return View(postalCode);
        }

        // GET: PostalCodes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.PostalCodes == null)
            {
                return NotFound();
            }

            var postalCode = await _context.PostalCodes.FindAsync(id);
            if (postalCode == null)
            {
                return NotFound();
            }
            return View(postalCode);
        }

        // POST: PostalCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ZipCode,Location")] PostalCode postalCode)
        {
            if (id != postalCode.ZipCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postalCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostalCodeExists(postalCode.ZipCode))
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
            return View(postalCode);
        }

        // GET: PostalCodes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.PostalCodes == null)
            {
                return NotFound();
            }

            var postalCode = await _context.PostalCodes
                .FirstOrDefaultAsync(m => m.ZipCode == id);
            if (postalCode == null)
            {
                return NotFound();
            }

            return View(postalCode);
        }

        // POST: PostalCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.PostalCodes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PostalCodes'  is null.");
            }
            var postalCode = await _context.PostalCodes.FindAsync(id);
            if (postalCode != null)
            {
                _context.PostalCodes.Remove(postalCode);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostalCodeExists(string id)
        {
          return (_context.PostalCodes?.Any(e => e.ZipCode == id)).GetValueOrDefault();
        }
    }
}
