using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using installments_Payment.DataAccessLayer.Entities.Users;
using installments_Payment.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Kavenegar;
using installments_Payment.Core.ViewModels.Authentication;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<Patient> _passwordHasher;
    private readonly string _kavenegarApiKey = "<Your_Kavenegar_API_Key>"; // کلید API کاوه نگار

    public AccountController(AppDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<Patient>();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var existingPatient = await _context.Patients
            .FirstOrDefaultAsync(p => p.PrimaryPhoneNumber == model.PrimaryPhoneNumber);

        if (existingPatient != null)
        {
            ModelState.AddModelError(nameof(model.PrimaryPhoneNumber), "این شماره تماس قبلاً ثبت شده است.");
            return View(model);
        }

        var patient = new Patient
        {
            FullName = model.FullName,
            PrimaryPhoneNumber = model.PrimaryPhoneNumber,
            NationalCardImagePath = "",
            Address = "",
            NationalCode = "",
            PhoneVerificationCode = "00000",
            SecondaryPhoneNumber = "",
            PhoneVerificationExpiry = DateTime.Now,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsPhoneVerified = false // چون تایید شماره حذف شد، فرض می‌کنیم true است
        };

        patient.Password = _passwordHasher.HashPassword(patient, model.Password);

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        // بعد از ثبت نام مستقیم به صفحه لاگین هدایت کن
        // پس از ثبت موفق بیمار:
        TempData["RegisterSuccessMessage"] = "ثبت نام موفقیت‌آمیز بود، وارد حساب کاربری خود شوید";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // پیدا کردن کاربر در هر دو جدول
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.PrimaryPhoneNumber == model.PrimaryPhoneNumber);

        var assistant = await _context.Assistants
            .FirstOrDefaultAsync(a => a.PhoneNumber == model.PrimaryPhoneNumber);

        // هیچکدوم نبود
        if (patient == null && assistant == null)
        {
            ModelState.AddModelError("Password", "شماره تلفن یا رمز عبور اشتباه است.");
            return View(model);
        }

        // فقط بیمار بود
        if (patient != null && assistant == null)
        {
            var result = _passwordHasher.VerifyHashedPassword(patient, patient.Password, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Password", "شماره تلفن یا رمز عبور اشتباه است.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", patient.Id);
            HttpContext.Session.SetString("FullName", patient.FullName);
            HttpContext.Session.SetString("UserRole", "Patient");

            return RedirectToAction("Index", "Home");
        }

        // فقط دستیار بود
        if (assistant != null && patient == null)
        {
            var assistantHasher = new PasswordHasher<Assistant>();
            var result = assistantHasher.VerifyHashedPassword(assistant, assistant.Password, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Password", "شماره تلفن یا رمز عبور اشتباه است.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", assistant.Id);
            HttpContext.Session.SetString("FullName", assistant.FullName);
            HttpContext.Session.SetString("UserRole", "Assistant");

            return RedirectToAction("Index", "Dashboard", new { area = "Assistant" });
        }

        // توی هر دو جدول وجود داشت → بفرست به صفحه انتخاب نقش
        return RedirectToAction("ChooseRole", "Account", new { phone = model.PrimaryPhoneNumber });
    }

    [HttpGet]
    public IActionResult VerifyPhone(string phone)
    {
        ViewBag.Phone = phone;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> VerifyPhone(VerifyPhoneViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PrimaryPhoneNumber == model.Phone);

        if (patient == null)
        {
            ModelState.AddModelError("", "شماره تماس معتبر نیست.");
            return View(model);
        }

        if (patient.PhoneVerificationCode == model.Code && patient.PhoneVerificationExpiry > DateTime.UtcNow)
        {
            patient.IsPhoneVerified = true;
            patient.PhoneVerificationCode = null;
            patient.PhoneVerificationExpiry = null;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "کد تأیید اشتباه یا منقضی شده است.");
            return View(model);
        }
    }
}
