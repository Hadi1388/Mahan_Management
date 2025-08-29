using installments_Payment.Core.Enums;
using installments_Payment.Core.ViewModels.Request;
using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities.Installments;
using installments_Payment.DataAccessLayer.Entities.Requests;
using installments_Payment.DataAccessLayer.Entities.Treatments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static installments_Payment.Core.Enums.GuaranteeTypes;
using static installments_Payment.Core.Enums.PaymentTypes;
using static installments_Payment.Core.Enums.RequestStatuses;
using static installments_Payment.Core.Enums.TreatmentStatuses;

namespace installments_Payment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequestsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RequestsController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Requests
        public async Task<IActionResult> Index()
        {
            var requests = await _context.Requests
                .Include(r => r.Patient)
                .Include(r => r.Treatment)
                .Include(r => r.TreatmentType)
                .Include(r => r.GuaranteeFiles)
                .Where(r => r.Status == RequestStatuses.Pending)
                .ToListAsync();

            return View(requests);
        }

        // GET: Admin/Requests/CompleteRequest/5
        public async Task<IActionResult> CompleteRequest(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null)
                return NotFound();

            var vm = new RequestCompleteViewModel
            {
                RequestId = request.RequestId,
                FullName = request.Patient?.FullName,
                NationalCode = request.Patient?.NationalCode,
                PrimaryPhoneNumber = request.Patient?.PrimaryPhoneNumber,
                SecondaryPhoneNumber = request.Patient?.SecondaryPhoneNumber,
                Address = request.Patient?.Address,
                NationalCardImagePath = request.Patient.NationalCardImagePath,
                TreatmentTypeId = request.TreatmentTypeId,
                TreatmentId = request.TreatmentId,
                StartDate = request.StartDate == default ? DateTime.Today : request.StartDate,
                PaymentType = request.PaymentType,
                GuaranteeType = request.GuaranteeType,
                DoctorName = request.DoctorName,
                TotalAmount = request.TotalAmount,
                Prepayment = request.Prepayment,
                InstallmentsCount = request.InstallmentsCount,
            };

            ViewData["TreatmentTypes"] = new SelectList(await _context.TreatmentTypes.ToListAsync(), "TreatmentTypeId", "TreatmentTypeName");
            ViewData["Treatments"] = new SelectList(await _context.Treatments.ToListAsync(), "TreatmentId", "TreatmentName");

            return View(vm);
        }

        // POST: Admin/Requests/CompleteRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteRequest(RequestCompleteViewModel vm)
        {
            //ModelState.Remove(nameof(vm.StartDate));
            ModelState.Remove(nameof(vm.GuaranteeFiles));
            ModelState.Remove(nameof(vm.GuaranteeNumbers));
            ModelState.Remove(nameof(vm.InstallmentsCount));
            ModelState.Remove(nameof(vm.GuaranteeType));

            // ولیدیشن شرطی بر اساس نوع پرداخت
            if (vm.PaymentType == PaymentTypes.Cash)
            {
                ModelState.Remove(nameof(vm.GuaranteeFiles));
                ModelState.Remove(nameof(vm.InstallmentsCount));
                ModelState.Remove(nameof(vm.GuaranteeNumbers));
                ModelState.Remove(nameof(vm.GuaranteeType));

                vm.GuaranteeType = GuaranteeTypes.Cash;
            }
            else if (vm.PaymentType == PaymentTypes.Installment)
            {
                bool hasNewFile = vm.GuaranteeFiles != null && vm.GuaranteeFiles.Any(f => f != null && f.Length > 0);
                bool hasOldFile = await _context.RequestGuaranteeFiles
                    .AnyAsync(g => g.RequestId == vm.RequestId);

                if (!hasNewFile && !hasOldFile)
                    ModelState.AddModelError(nameof(vm.GuaranteeFiles), "لطفاً تصویر(های) ضمانت را آپلود کنید.");

                if (vm.InstallmentsCount <= 0 || vm.InstallmentsCount == null)
                    ModelState.AddModelError(nameof(vm.InstallmentsCount), "لطفاً تعداد اقساط را وارد کنید.");
            }

            ModelState.Remove(nameof(vm.GuaranteeFiles));

            if (!ModelState.IsValid)
            {
                var requestF = await _context.Requests
                    .Include(r => r.Patient)
                    .FirstOrDefaultAsync(r => r.RequestId == vm.RequestId);

                if (requestF != null)
                {
                    vm.FullName = requestF.Patient?.FullName;
                    vm.NationalCode = requestF.Patient?.NationalCode;
                    vm.PrimaryPhoneNumber = requestF.Patient?.PrimaryPhoneNumber;
                    vm.SecondaryPhoneNumber = requestF.Patient?.SecondaryPhoneNumber;
                    vm.Address = requestF.Patient?.Address;
                    vm.NationalCardImagePath = requestF.Patient?.NationalCardImagePath;
                }

                ViewData["TreatmentTypes"] = new SelectList(await _context.TreatmentTypes.ToListAsync(), "TreatmentTypeId", "TreatmentTypeName");
                ViewData["Treatments"] = new SelectList(await _context.Treatments.ToListAsync(), "TreatmentId", "TreatmentName");
                return View(vm);
            }

            var request = await _context.Requests
                .Include(r => r.GuaranteeFiles)
                .FirstOrDefaultAsync(r => r.RequestId == vm.RequestId);

            if (request == null)
                return NotFound();

            // آپدیت اطلاعات درخواست
            request.UpdatedAt = DateTime.UtcNow;
            request.TreatmentTypeId = vm.TreatmentTypeId;
            request.TreatmentId = vm.TreatmentId;
            request.StartDate = vm.StartDate;
            request.PaymentType = vm.PaymentType;
            request.GuaranteeType = vm.PaymentType == PaymentTypes.Cash ? GuaranteeTypes.Cash : vm.GuaranteeType;
            request.DoctorName = vm.DoctorName;
            request.TotalAmount = vm.TotalAmount;
            request.Prepayment = vm.Prepayment;
            request.InstallmentsCount = vm.PaymentType == PaymentTypes.Cash
                ? 0
                : (vm.InstallmentsCount ?? 0);
            request.UpdatedAt = DateTime.UtcNow;

            // آپلود فایل ضمانت
            if (vm.GuaranteeFiles != null && vm.GuaranteeFiles.Any(f => f != null && f.Length > 0))
            {
                string guaranteeFolder = Path.Combine(_hostEnvironment.WebRootPath, "img", "guarantees");
                if (!Directory.Exists(guaranteeFolder))
                    Directory.CreateDirectory(guaranteeFolder);

                for (int i = 0; i < vm.GuaranteeFiles.Count; i++)
                {
                    var file = vm.GuaranteeFiles[i];
                    if (file != null && file.Length > 0)
                    {
                        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        string filePath = Path.Combine(guaranteeFolder, uniqueFileName);

                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(fileStream);

                        var guaranteeNumber = (vm.GuaranteeNumbers != null && vm.GuaranteeNumbers.Count > i)
                            ? vm.GuaranteeNumbers[i]
                            : null;

                        _context.RequestGuaranteeFiles.Add(new RequestGuaranteeFile
                        {
                            RequestId = request.RequestId,
                            GuaranteeNumber = guaranteeNumber,
                            FilePath = $"/img/guarantees/{uniqueFileName}"
                        });
                    }
                }
            }

            //// مدیریت اقساط
            //await GenerateInstallments(request);

            await _context.SaveChangesAsync();

            TempData["AlertMessage"] = "درخواست با موفقیت ذخیره شد";
            TempData["AlertType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // متد جدید برای ساخت خودکار اقساط
        private async Task GenerateInstallments(Request request)
        {
            // فقط برای اقساطی‌ها
            if (request.PaymentType != PaymentTypes.Installment || request.InstallmentsCount <= 0)
            {
                // حذف اقساط قبلی
                var oldInstallments = await _context.Installments
                    .Where(i => i.RequestId == request.RequestId)
                    .ToListAsync();
                if (oldInstallments.Any())
                    _context.Installments.RemoveRange(oldInstallments);

                return;
            }

            // حذف اقساط قبلی
            var existingInstallments = await _context.Installments
                .Where(i => i.RequestId == request.RequestId)
                .ToListAsync();
            if (existingInstallments.Any())
                _context.Installments.RemoveRange(existingInstallments);

            int? installments = request.InstallmentsCount;
            int? remain = request.TotalAmount - request.Prepayment;
            int? baseAmount = remain / installments;
            int? remainder = remain % installments;

            DateTime? firstInstallmentDate = request.Status == RequestStatuses.Approved
                ? request.UpdatedAt.AddDays(30)
                : request.StartDate;

            for (int i = 0; i < installments; i++)
            {
                int? amount = baseAmount + (i < remainder ? 1 : 0);
                _context.Installments.Add(new Installment
                {
                    RequestId = request.RequestId,
                    Amount = amount,
                    DueDate = firstInstallmentDate.Value.AddMonths(i),
                    Paid = false
                });
            }
        }

        // GET: Admin/Requests/Approved
        public async Task<IActionResult> ApprovedRequests()
        {
            var approvedRequests = await _context.Requests
                .Include(r => r.Patient)
                .Include(r => r.Treatment)
                .Include(r => r.TreatmentType)
                .Include(r => r.GuaranteeFiles)
                .Where(r => r.Status == RequestStatuses.Approved)
                .ToListAsync();

            return View(approvedRequests);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.Requests
                    .Include(r => r.GuaranteeFiles)
                    .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null)
                return NotFound();

            request.Status = RequestStatuses.Approved;
            request.UpdatedAt = DateTime.UtcNow;

            await GenerateInstallments(request);

            var patientId = await _context.Requests
                    .Where(r => r.RequestId == id)
                    .Select(r => r.PatientId)
                    .FirstOrDefaultAsync();

            var patientName = await _context.Patients
                    .Where(p => p.Id == patientId)
                    .Select(p => p.FullName)
                    .FirstOrDefaultAsync();

            var treatmentProcess = new TreatmentProcess
            {
                RequestId = request.RequestId,
                PatientName = patientName,
                DoctorName = request.DoctorName,
                StartDate = request.StartDate,
                TreatmentStatus = TreatmentStatuses.Examination,
                ProcessStatus =  ProcessStatuses.NotEntered
            };
            _context.TreatmentProcesses.Add(treatmentProcess);

            await _context.SaveChangesAsync();

            TempData["AlertMessage"] = "درخواست با موفقیت تایید شد و درمان به لیست دستیار اضافه شد";
            TempData["AlertType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            //var request = await _context.Requests
            //        .Include(r => r.Installments)
            //        .Include(r => r.TreatmentProcess) // فرض کنیم One-to-One
            //        .FirstOrDefaultAsync(r => r.RequestId == id);

            //if (request == null)
            //    return NotFound();

            //// حذف پروسه درمان
            //_context.TreatmentProcesses.Remove(request.TreatmentProcess);

            //// حذف همه اقساط مربوطه
            //if (request.Installments.Any())
            //    _context.Installments.RemoveRange(request.Installments);

            //_context.Requests.Remove(request);

            //await _context.SaveChangesAsync();

            //TempData["AlertMessage"] = "درخواست با موفقیت رد شد";
            //TempData["AlertType"] = "danger";

            return RedirectToAction(nameof(Index));
        }
    }
}
