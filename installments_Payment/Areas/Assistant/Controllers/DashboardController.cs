using installments_Payment.DataAccessLayer.Context;
using installments_Payment.Filters;
using Microsoft.AspNetCore.Mvc;

namespace installments_Payment.Areas.Assistant.Controllers
{
    [Area("Assistant")]
    [AssistantOnly]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Assistant/MyProfile")]
        public IActionResult Profile()
        {
            // فرض می‌کنیم نام کاربری دستیار در سشن ذخیره شده
            var username = HttpContext.Session.GetString("FullName");

            if (string.IsNullOrEmpty(username))
            {
                // اگر سشن وجود نداشت به صفحه ورود یا خطا redirect شود
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // پیدا کردن دستیار بر اساس نام کاربری
            var assistant = _context.Assistants
                .FirstOrDefault(a => a.FullName == username); // یا فیلد Username اگر داری

            if (assistant == null)
            {
                return NotFound();
            }

            return View(assistant);
        }
    }
}
