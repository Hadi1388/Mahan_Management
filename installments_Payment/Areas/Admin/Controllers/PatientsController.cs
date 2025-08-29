using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace installments_Payment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PatientsController : Controller
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Admin/Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Admin/Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,PrimaryPhoneNumber,SecondaryPhoneNumber,NationalCode,CheckOrPromissoryNoteImagePath,NationalCardImagePath,Address,Password,IsPhoneVerified")] Patient patient, IFormFile NationalCardImageFile)
        {
            // اگر فایل آپلود شده وجود دارد، ذخیره کن
            if (NationalCardImageFile != null && NationalCardImageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(NationalCardImageFile.FileName);

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "national-card");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await NationalCardImageFile.CopyToAsync(fileStream);
                }

                patient.NationalCardImagePath = uniqueFileName;
            }
            else
            {
                // اگر فایل ضروری نیست می‌توانید خط زیر را فعال کنید و ولیدیشن را دور بزنید
                ModelState.Remove(nameof(patient.NationalCardImagePath));
                patient.NationalCardImagePath = "/img/national-card/no-image.jpg";
            }



            if (!string.IsNullOrEmpty(patient.Password))
            {
                var hasher = new PasswordHasher<Patient>();
                patient.Password = hasher.HashPassword(patient, patient.Password);
            }

            patient.IsPhoneVerified = false;
            patient.CreatedAt = DateTime.Now;
            patient.UpdatedAt = DateTime.Now;
            patient.PhoneVerificationCode = "00000";
            patient.PhoneVerificationExpiry = null;
            ModelState.Remove(nameof(patient.PhoneVerificationCode));
            ModelState.Remove(nameof(patient.NationalCardImagePath));
            ModelState.Remove(nameof(patient.PhoneVerificationExpiry));
            ModelState.Remove(nameof(NationalCardImageFile));
            ModelState.Remove(nameof(patient.UpdatedAt));
            ModelState.Remove(nameof(patient.LastLoginAt));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "خطا در ثبت بیمار: " + errorMessage);
                }
            }

            return View(patient);
        }

        // POST: Admin/Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound();

            // پسورد رو خالی کنیم که توی ویو خالی نمایش داده بشه (امنیت)
            patient.Password = "";

            return View(patient);
        }

        // POST: Admin/Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient, IFormFile NationalCardImageFile)
        {
            if (id != patient.Id)
                return NotFound();

            ModelState.Remove(nameof(NationalCardImageFile));
            ModelState.Remove(nameof(patient.Password));
            ModelState.Remove(nameof(patient.PhoneVerificationCode));

            if (!ModelState.IsValid)
                return View(patient);

            // دریافت مدل اصلی از دیتابیس
            var patientInDb = await _context.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (patientInDb == null)
                return NotFound();

            // مسیر تصویر قبلی را ذخیره می‌کنیم
            var oldImagePath = patientInDb.NationalCardImagePath;
            ModelState.Remove(nameof(patient.NationalCardImagePath));

            // آپدیت مسیر تصویر فقط در صورت آپلود فایل جدید
            if (NationalCardImageFile != null && NationalCardImageFile.Length > 0)
            {
                // آپلود فایل جدید
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(NationalCardImageFile.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "national-card");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await NationalCardImageFile.CopyToAsync(fileStream);
                }

                patient.NationalCardImagePath = uniqueFileName;
            }
            else
            {
                // اگر فایل جدید آپلود نشد، مسیر تصویر قبلی حفظ شود
                patient.NationalCardImagePath = oldImagePath;
            }

            // مدیریت پسورد:
            // اگر کاربر پسورد جدید وارد نکرده، مقدار پسورد در دیتابیس حفظ شود
            if (string.IsNullOrWhiteSpace(patient.Password))
            {
                patient.Password = patientInDb.Password;
            }
            else
            {
                // هَش کردن پسورد جدید
                var hasher = new PasswordHasher<Patient>();
                patient.Password = hasher.HashPassword(patient, patient.Password);
            }

            // حفظ فیلدهای سیستمی (تاریخ‌ها و غیره) از دیتابیس
            patient.CreatedAt = patientInDb.CreatedAt;
            patient.UpdatedAt = DateTime.UtcNow;
            patient.LastLoginAt = patientInDb.LastLoginAt;
            patient.PhoneVerificationCode = patientInDb.PhoneVerificationCode;
            patient.PhoneVerificationExpiry = patientInDb.PhoneVerificationExpiry;

            try
            {
                _context.Update(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patient.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Admin/Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
