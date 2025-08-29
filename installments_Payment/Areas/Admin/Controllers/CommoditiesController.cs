using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities.Inventories;

namespace installments_Payment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommoditiesController : Controller
    {
        private readonly AppDbContext _context;

        public CommoditiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Commodities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Commodities.ToListAsync());
        }

        // GET: Admin/Commodities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commodity = await _context.Commodities
                .FirstOrDefaultAsync(m => m.CommodityId == id);
            if (commodity == null)
            {
                return NotFound();
            }

            return View(commodity);
        }

        // GET: Admin/Commodities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Commodities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommodityId,Name")] Commodity commodity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commodity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commodity);
        }

        // GET: Admin/Commodities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commodity = await _context.Commodities.FindAsync(id);
            if (commodity == null)
            {
                return NotFound();
            }
            return View(commodity);
        }

        // POST: Admin/Commodities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommodityId,Name")] Commodity commodity)
        {
            if (id != commodity.CommodityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commodity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommodityExists(commodity.CommodityId))
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
            return View(commodity);
        }

        // GET: Admin/Commodities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commodity = await _context.Commodities
                .FirstOrDefaultAsync(m => m.CommodityId == id);
            if (commodity == null)
            {
                return NotFound();
            }

            return View(commodity);
        }

        // POST: Admin/Commodities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commodity = await _context.Commodities.FindAsync(id);
            if (commodity != null)
            {
                _context.Commodities.Remove(commodity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommodityExists(int id)
        {
            return _context.Commodities.Any(e => e.CommodityId == id);
        }
    }
}
