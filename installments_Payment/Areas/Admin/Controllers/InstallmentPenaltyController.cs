using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities.Installments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace installments_Payment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InstallmentPenaltyController : Controller
    {
        private readonly AppDbContext _context;

        public InstallmentPenaltyController(AppDbContext context)
        {
            _context = context;
        }

        // نمایش آخرین تنظیم
        public async Task<IActionResult> Index()
        {
            // آخرین تنظیم
            var latestSetting = await _context.InstallmentPenalties
                .OrderByDescending(s => s.LastUpdated)
                .FirstOrDefaultAsync();

            // تاریخچه همه تنظیمات
            var history = await _context.InstallmentPenalties
                .OrderByDescending(s => s.LastUpdated)
                .ToListAsync();

            ViewBag.Latest = latestSetting;
            ViewBag.History = history;

            return View();
        }

        // تغییر یا ساخت تنظیم جدید
        [HttpPost]
        public async Task<IActionResult> Update(double dailyPenaltyPercent)
        {
            var setting = new InstallmentPenalty
            {
                DailyPenaltyPercent = dailyPenaltyPercent,
                LastUpdated = DateTime.Now
            };

            _context.InstallmentPenalties.Add(setting);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
