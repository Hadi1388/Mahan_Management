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
    public class AssistantsController : Controller
    {
        private readonly AppDbContext _context;

        public AssistantsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Assistants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Assistants.ToListAsync());
        }

        // GET: Admin/Assistants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assistant == null)
            {
                return NotFound();
            }

            return View(assistant);
        }

        // GET: Admin/Assistants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Assistants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DataAccessLayer.Entities.Users.Assistant assistant, IFormFile ProfileImageFile)
        {
            ModelState.Remove(nameof(ProfileImageFile));
            if (ModelState.IsValid)
            {
                // هش پسورد
                var hasher = new PasswordHasher<DataAccessLayer.Entities.Users.Assistant>();
                assistant.Password = hasher.HashPassword(assistant, assistant.Password);
                // ذخیره عکس پروفایل
                if (ProfileImageFile != null && ProfileImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImageFile.FileName);
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/assistant-profile", fileName);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await ProfileImageFile.CopyToAsync(stream);
                    }

                    assistant.ProfileImage = "/img/assistant-profile/" + fileName;
                }

                else
                {
                    // اگر فایل آپلود نشد، تصویر پیش‌فرض قرار بده
                    assistant.ProfileImage = "/img/assistant-profile/no-image.jpg"; // مسیر تصویر پیش‌فرض
                }

                _context.Add(assistant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assistant);
        }

        // GET: Admin/Assistants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistants.FindAsync(id);
            if (assistant == null)
            {
                return NotFound();
            }
            return View(assistant);
        }

        // POST: Admin/Assistants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DataAccessLayer.Entities.Users.Assistant assistant, IFormFile ProfileImageFile)
        {
            if (id != assistant.Id)
                return NotFound();

            ModelState.Remove(nameof(ProfileImageFile));
            ModelState.Remove(nameof(assistant.Password));

            if (ModelState.IsValid)
            {
                try
                {
                    // پسورد تغییر نکند
                    assistant.Password = _context.Assistants
                        .AsNoTracking()
                        .First(a => a.Id == assistant.Id).Password;

                    // اگر عکس جدید آپلود شد
                    if (ProfileImageFile != null && ProfileImageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImageFile.FileName);
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/assistant-profile", fileName);

                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await ProfileImageFile.CopyToAsync(stream);
                        }

                        assistant.ProfileImage = "/img/assistant-profile/" + fileName;
                    }
                    else
                    {
                        // نگه داشتن عکس قبلی
                        assistant.ProfileImage = _context.Assistants
                            .AsNoTracking()
                            .First(a => a.Id == assistant.Id).ProfileImage;
                    }

                    _context.Update(assistant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssistantExists(assistant.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(assistant);
        }

        // GET: Admin/Assistants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assistant = await _context.Assistants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assistant == null)
            {
                return NotFound();
            }

            return View(assistant);
        }

        // POST: Admin/Assistants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assistant = await _context.Assistants.FindAsync(id);
            if (assistant != null)
            {
                _context.Assistants.Remove(assistant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssistantExists(int id)
        {
            return _context.Assistants.Any(e => e.Id == id);
        }
    }
}
