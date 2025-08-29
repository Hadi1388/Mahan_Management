using installments_Payment.Core.ViewModels.Installment;
using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities.Installments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace installments_Payment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Installments")]
    public class InstallmentsController : Controller
    {
        private readonly AppDbContext _context;

        public InstallmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: نمایش اقساط یک درخواست
        [HttpGet("Index/{requestId}")]
        public async Task<IActionResult> Index(int requestId)
        {
            var installments = await _context.Installments
                .Where(i => i.RequestId == requestId)
                .OrderBy(i => i.DueDate)
                .ToListAsync();

            var penaltySetting = await _context.InstallmentPenalties
                .OrderByDescending(x => x.LastUpdated)
                .FirstOrDefaultAsync();


            var request = await _context.Requests
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(r => r.RequestId == requestId);

            var patientName = request.Patient.FullName;

            var vmList = new List<InstallmentViewModel>();


            foreach (var inst in installments)
            {
                var vm = new InstallmentViewModel
                {
                    RequestId = inst.RequestId,
                    Amount = inst.Amount,
                    DueDate = inst.DueDate,
                    IsPaid = inst.Paid,
                    AmountWithPenalty = inst.Amount
                };

                if (!inst.Paid && penaltySetting != null && inst.DueDate < DateTime.Now)
                {
                    var daysLate = (DateTime.Now - inst.DueDate.Value).Days;
                    if (daysLate > 0)
                    {
                        var penalty = inst.Amount * (decimal)(penaltySetting.DailyPenaltyPercent / 100) * daysLate;
                        vm.AmountWithPenalty = inst.Amount + penalty;
                    }
                }

                vmList.Add(vm);
            }

            ViewBag.RequestId = requestId;
            ViewBag.PatientName = patientName;
            return View(vmList);
        }

        // GET: ایجاد قسط جدید
        [HttpGet("Create/{requestId}")]
        public IActionResult Create(int requestId)
        {
            var vm = new InstallmentViewModel { RequestId = requestId };
            return View(vm);
        }

        // POST: ذخیره قسط
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstallmentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var installment = new Installment
            {
                RequestId = vm.RequestId,
                Amount = vm.Amount,
                DueDate = vm.DueDate,
                Paid = false
            };

            _context.Installments.Add(installment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { requestId = vm.RequestId });
        }
    }
}
