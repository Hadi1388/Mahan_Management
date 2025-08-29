using Azure.Core;
using installments_Payment.Core.ViewModels.Request;
using installments_Payment.DataAccessLayer.Context;
using installments_Payment.DataAccessLayer.Entities;
using installments_Payment.DataAccessLayer.Entities.Requests;
using installments_Payment.DataAccessLayer.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
public class RequestGuaranteeFilesController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public RequestGuaranteeFilesController(AppDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    // GET: نمایش لیست وثیقه‌های یک درخواست
    public async Task<IActionResult> Index(int requestId)
    {
        var request = await _context.Requests
            .Include(r => r.Patient)
            .Include(r => r.GuaranteeFiles)
            .FirstOrDefaultAsync(r => r.RequestId == requestId);

        if (request == null)
            return NotFound();

        ViewBag.RequestId = requestId;
        ViewBag.PatientName = request.Patient.FullName;
        return View(request.GuaranteeFiles);
    }

    // GET: فرم آپلود وثیقه جدید
    public IActionResult Create(int requestId)
    {
        var request = _context.Requests.Include(r => r.Patient).FirstOrDefault(r => r.RequestId == requestId);
        if (request == null)
            return NotFound();

        var vm = new UploadMultipleGuaranteeFilesViewModel
        {
            RequestId = requestId,
            PatientFullName = request.Patient.FullName
        };

        ViewBag.RequestId = requestId;
        return View(vm);
    }

    // POST: ذخیره وثیقه جدید
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UploadMultipleGuaranteeFilesViewModel vm)
    {
        ModelState.Remove("PatientFullName");
        if (!ModelState.IsValid)
            return View(vm);

        var request = await _context.Requests.FindAsync(vm.RequestId);
        if (request == null)
            return NotFound();

        // آپلود فایل
        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img", "guarantees");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.File.FileName)}";
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await vm.File.CopyToAsync(fileStream);
        }

        // ذخیره مسیر فایل برای دسترسی از وب (مسیر نسبی به wwwroot)
        var guaranteeFile = new RequestGuaranteeFile
        {
            RequestId = vm.RequestId,
            GuaranteeNumber = vm.GuaranteeNumber,
            FilePath = $"/img/guarantees/{uniqueFileName}"
        };


        _context.RequestGuaranteeFiles.Add(guaranteeFile);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new { requestId = vm.RequestId });
    }

    // POST: حذف وثیقه
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var guaranteeFile = await _context.RequestGuaranteeFiles.FindAsync(id);
        if (guaranteeFile == null)
            return NotFound();

        _context.RequestGuaranteeFiles.Remove(guaranteeFile);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new { requestId = guaranteeFile.RequestId });
    }
}
