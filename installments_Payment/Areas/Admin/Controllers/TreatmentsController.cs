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
    public class TreatmentsController : Controller
    {
        private readonly AppDbContext _context;

        public TreatmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Treatments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Treatments.ToListAsync());
        }

        // GET: Admin/Treatments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments
                .FirstOrDefaultAsync(m => m.TreatmentId == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // GET: Admin/Treatments/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TreatmentCreateEditViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TreatmentCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var treatment = new Treatment
            {
                TreatmentName = model.TreatmentName,
            };

            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Admin/Treatments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            if (treatment == null)
                return NotFound();

            var model = new TreatmentCreateEditViewModel
            {
                TreatmentId = treatment.TreatmentId,
                TreatmentName = treatment.TreatmentName,
            };

            return View(model);
        }

        // POST: Admin/Treatments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TreatmentCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var treatment = await _context.Treatments.FindAsync(model.TreatmentId);
            if (treatment == null)
                return NotFound();

            treatment.TreatmentName = model.TreatmentName;

            _context.Update(treatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Treatments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments
                .FirstOrDefaultAsync(m => m.TreatmentId == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // POST: Admin/Treatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            if (treatment != null)
            {
                _context.Treatments.Remove(treatment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatments.Any(e => e.TreatmentId == id);
        }
    }
}
