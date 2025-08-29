using installments_Payment.Core.Enums;
using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities.Installments;
using installments_Payment.DataAccessLayer.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace installments_Payment.Areas.PatientPanel.Controllers
{
    [Area("PatientPanel")]
    [PatientOnly]
    public class TreatmentsController : Controller
    {
        private readonly AppDbContext _context;

        public TreatmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            // گرفتن درخواست‌های تایید شده کاربر
            var requests = await _context.Requests
                .Include(r => r.Treatment)
                .Include(r => r.Patient)
                .Where(r => r.PatientId == userId && r.Status == RequestStatuses.Approved)
                .ToListAsync();

            return View(requests);
        }

        [HttpGet]
        public async Task<IActionResult> Installments(int requestId)
        {
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            // بررسی اینکه این درخواست متعلق به کاربر است و تایید شده
            var request = await _context.Requests
                .Include(r => r.Treatment)
                .FirstOrDefaultAsync(r => r.RequestId == requestId && r.PatientId == userId && r.Status == RequestStatuses.Approved);

            if (request == null)
                return NotFound();

            // گرفتن اقساط مرتبط با این درخواست
            var installments = await _context.Installments
                .Where(i => i.RequestId == requestId)
                .OrderBy(i => i.DueDate) // مرتب سازی بر اساس تاریخ سررسید
                .ToListAsync();

            var model = new TreatmentInstallmentsViewModel
            {
                Request = request,
                Installments = installments
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentHistory()
        {
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            // گرفتن همه اقساط پرداخت شده مربوط به درخواست‌های کاربر
            var paidInstallments = await _context.Installments
                .Include(i => i.Request)
                .ThenInclude(r => r.Treatment)
                .Where(i => i.Request.PatientId == userId && i.Paid)
                .OrderByDescending(i => i.PaymentDate)
                .ToListAsync();

            return View(paidInstallments);
        }

        public class TreatmentInstallmentsViewModel
        {
            public Request Request { get; set; }
            public List<Installment> Installments { get; set; }
        }
    }
}
