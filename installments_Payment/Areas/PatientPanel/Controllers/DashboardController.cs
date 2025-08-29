using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using installments_Payment.Core.ViewModels;
using installments_Payment.DataAccessLayer.Context;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using installments_Payment.Core.ViewModels.Patient;
using installments_Payment.Core.Enums;
using installments_Payment.DataAccessLayer.Entities.Requests;
using Microsoft.EntityFrameworkCore;

namespace installments_Payment.Areas.PatientPanel.Controllers
{
    [Area("PatientPanel")]
    [PatientOnly]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DashboardController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var patient = await _context.Patients.FindAsync(userId);

            if (patient == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            // بررسی شرایط غیرفعال شدن دکمه
            bool disableRequestButton = !patient.IsPhoneVerified
                || string.IsNullOrWhiteSpace(patient.Address)
                || string.IsNullOrWhiteSpace(patient.SecondaryPhoneNumber)
                || string.IsNullOrWhiteSpace(patient.NationalCode)
                || string.IsNullOrWhiteSpace(patient.NationalCardImagePath);

            var model = new PatientProfileViewModel
            {
                FullName = patient.FullName,
                PrimaryPhoneNumber = patient.PrimaryPhoneNumber,
                IsPhoneVerified = patient.IsPhoneVerified,
                DisableRequestButton = !patient.IsPhoneVerified
                    || string.IsNullOrWhiteSpace(patient.Address)
                    || string.IsNullOrWhiteSpace(patient.SecondaryPhoneNumber)
                    || string.IsNullOrWhiteSpace(patient.NationalCode)
                    || string.IsNullOrWhiteSpace(patient.NationalCardImagePath),

                // مقداردهی فیلدهای تکمیل اطلاعات
                SecondaryPhoneNumber = patient.SecondaryPhoneNumber,
                NationalCode = patient.NationalCode,
                Address = patient.Address,
                NationalCardImagePath = patient.NationalCardImagePath,
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CompleteInfo()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            var patient = await _context.Patients.FindAsync(userId);

            if (patient == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            //if (!patient.IsPhoneVerified)
            //{
            //    // شماره تلفن تایید نشده، ریدایرکت یا نمایش پیام مناسب
            //    return RedirectToAction("CompleteInfo", "Dashboard", new { area = "PatientPanel" });
            //}

            var isRequestCompleted = !string.IsNullOrWhiteSpace(patient.SecondaryPhoneNumber)
                         && !string.IsNullOrWhiteSpace(patient.NationalCode)
                         && !string.IsNullOrWhiteSpace(patient.Address)
                         && !string.IsNullOrWhiteSpace(patient.NationalCardImagePath);

            // مقداردهی اولیه مدل فقط برای فیلدهای تکمیل اطلاعات
            var model = new PatientEditInfoViewModel
            {
                SecondaryPhoneNumber = string.IsNullOrWhiteSpace(patient.SecondaryPhoneNumber) ? "" : patient.SecondaryPhoneNumber,
                NationalCode = string.IsNullOrWhiteSpace(patient.NationalCode) ? "" : patient.NationalCode,
                NationalCardImagePath = string.IsNullOrWhiteSpace(patient.NationalCardImagePath) ? "" : patient.NationalCardImagePath,
                Address = string.IsNullOrWhiteSpace(patient.Address) ? "" : patient.Address,
                isRequestCompleted = isRequestCompleted
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteInfo(PatientEditInfoViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var patient = await _context.Patients.FindAsync(userId);

            if (patient == null)
                return RedirectToAction("Login", "Account");

            // فقط فیلدهایی که توی ویو مدل داریم آپدیت میشن
            patient.SecondaryPhoneNumber = model.SecondaryPhoneNumber;
            patient.NationalCode = model.NationalCode;
            patient.Address = model.Address;

            // بررسی آپلود فایل
            if (model.NationalCardImageFile != null)
            {
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img/national-card");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = $"{Guid.NewGuid()}_{model.NationalCardImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.NationalCardImageFile.CopyToAsync(stream);
                }
                patient.NationalCardImagePath = $"/img/national-card/{uniqueFileName}";
            }

            // اگر تصویر آپلود نشده و تصویر قبلی هم وجود نداره
            if (string.IsNullOrEmpty(patient.NationalCardImagePath))
            {
                ModelState.AddModelError("NationalCardImageFile", "تصویر کارت ملی خود را آپلود کنید");
            }

            if (!ModelState.IsValid)
            {
                // برای پیش‌نمایش تصویر انتخاب شده
                model.NationalCardImagePath = patient.NationalCardImagePath;
                return View(model); // بدون Redirect
            }

            patient.UpdatedAt = DateTime.UtcNow;
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "اطلاعات شما با موفقیت ثبت شد";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRequest()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var patient = await _context.Patients.FindAsync(userId);

            var newRequest = new Request
            {
                PatientId = patient.Id,
                DoctorName = "",           // فیلدهای الزامی ولی خالی
                TotalAmount = null,
                Prepayment = null,
                InstallmentsCount = null,
                TreatmentId = null,
                TreatmentTypeId = null,
                StartDate = null,
                PaymentType = null,
                GuaranteeType = null,
                Status = RequestStatuses.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Requests.Add(newRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "درخواست شما با موفقیت ثبت شد و در وضعیت انتظار قرار گرفت.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            // گرفتن همه درخواست‌های در انتظار تایید کاربر
            var pendingRequests = await _context.Requests
                .Include(r => r.Treatment)
                .Where(r => r.PatientId == userId && r.Status == RequestStatuses.Pending)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();

            return View(pendingRequests);
        }
    }
}
