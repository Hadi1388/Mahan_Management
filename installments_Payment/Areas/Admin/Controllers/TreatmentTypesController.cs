using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities.Treatments;
using installments_Payment.Core.ViewModels.Treatment;

namespace installments_Payment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TreatmentTypesController : Controller
    {
        private readonly AppDbContext _context;

        public TreatmentTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TreatmentTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TreatmentTypes.ToListAsync());
        }

        // GET: Admin/TreatmentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentType = await _context.TreatmentTypes
                .FirstOrDefaultAsync(m => m.TreatmentTypeId == id);
            if (treatmentType == null)
            {
                return NotFound();
            }

            return View(treatmentType);
        }

        // GET: Admin/TreatmentTypes/Create
        public IActionResult Create()
        {
            return View(new TreatmentTypeCreateEditViewModel());
        }

        // POST: Admin/TreatmentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TreatmentTypeCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var treatmenttype = new TreatmentType
            {
                TreatmentTypeName = model.TreatmentTypeName,
            };

            _context.TreatmentTypes.Add(treatmenttype);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Admin/TreatmentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var treatmenttype = await _context.TreatmentTypes.FindAsync(id);
            if (treatmenttype == null)
                return NotFound();

            var model = new TreatmentTypeCreateEditViewModel
            {
                TreatmentTypeId = treatmenttype.TreatmentTypeId,
                TreatmentTypeName = treatmenttype.TreatmentTypeName,
            };

            return View(model);
        }

        // POST: Admin/TreatmentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TreatmentTypeCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var treatmenttype = await _context.TreatmentTypes.FindAsync(model.TreatmentTypeId);
            if (treatmenttype == null)
                return NotFound();

            treatmenttype.TreatmentTypeName = model.TreatmentTypeName;

            _context.Update(treatmenttype);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/TreatmentTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentType = await _context.TreatmentTypes
                .FirstOrDefaultAsync(m => m.TreatmentTypeId == id);
            if (treatmentType == null)
            {
                return NotFound();
            }

            return View(treatmentType);
        }

        // POST: Admin/TreatmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatmentType = await _context.TreatmentTypes.FindAsync(id);
            if (treatmentType != null)
            {
                _context.TreatmentTypes.Remove(treatmentType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentTypeExists(int id)
        {
            return _context.TreatmentTypes.Any(e => e.TreatmentTypeId == id);
        }
    }
}
