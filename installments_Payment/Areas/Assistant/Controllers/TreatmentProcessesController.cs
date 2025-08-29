using installments_Payment.Core.ViewModels.Treatment;
using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities.Treatments;
using installments_Payment.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace installments_Payment.Areas.Assistant.Controllers
{
    [Area("Assistant")]
    [AssistantOnly]
    public class TreatmentProcessesController : Controller
    {
        private readonly AppDbContext _context;
        public TreatmentProcessesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(string gregorianDate, string shamsiDate)
        {
            var selectedDate = !string.IsNullOrEmpty(gregorianDate)
                ? DateTime.Parse(gregorianDate)
                : DateTime.Today; // پیش‌فرض: امروز

            var processes = await _context.TreatmentProcesses
                .Include(tp => tp.Request)
                    .ThenInclude(r => r.Patient)
                .Where(tp => tp.StartDate >= selectedDate && tp.StartDate < selectedDate.AddDays(1))
                .OrderBy(tp => tp.StartDate)
                .ToListAsync();

            foreach (var tp in processes)
            {
                tp.PatientName = tp.Request?.Patient?.FullName ?? "نامشخص";
            }

            ViewBag.SelectedDate = selectedDate;

            // اگر شمسـی خالی بود، خودش رو از میلادی تولید کن
            if (string.IsNullOrEmpty(shamsiDate))
            {
                // تبدیل میلادی به شمسی با System.Globalization.PersianCalendar
                var pc = new System.Globalization.PersianCalendar();
                var d = selectedDate;
                shamsiDate = $"{pc.GetYear(d):0000}/{pc.GetMonth(d):00}/{pc.GetDayOfMonth(d):00}";
            }
            ViewBag.ShamsiDate = shamsiDate;

            return View(processes);
        }

        // GET تغییر
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var process = await _context.TreatmentProcesses.FindAsync(id);
            if (process == null) return NotFound();

            var vm = new TreatmentProcessUpdateViewModel
            {
                TreatmentProcessId = process.TreatmentProcessId,
                TreatmentStatus = process.TreatmentStatus,
                ProcessStatus = process.ProcessStatus
            };
            return View(vm);
        }

        // POST تغییر
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(TreatmentProcessUpdateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var process = await _context.TreatmentProcesses.FindAsync(vm.TreatmentProcessId);
            if (process == null) return NotFound();

            process.TreatmentStatus = vm.TreatmentStatus;
            process.ProcessStatus = vm.ProcessStatus;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
    }
}
