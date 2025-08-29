using installments_Payment.Core.ViewModels.Inventory;
using installments_Payment.DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using installments_Payment.Core.Extensions;
using installments_Payment.DataAccessLayer.Entities.Inventories;
using installments_Payment.Filters;

namespace installments_Payment.Areas.Assistant.Controllers
{
    [Area("Assistant")]
    //[AssistantOnly]
    public class RequirementsController : Controller
    {
        private readonly AppDbContext _context;
        public RequirementsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }

        // GET: Requirements/OrderRequest
        [HttpGet]
        public IActionResult OrderRequest()
        {
            var commodities = _context.Commodities
                .Select(c => new RequirementsItemViewModel
                {
                    CommodityId = c.CommodityId,
                    CommodityName = c.Name,
                    Quantity = 0,
                    IsSelected = false
                }).ToList();

            var viewModel = new RequirementsViewModel
            {
                RequirementsItems = commodities
            };

            return View(viewModel);
        }

        // POST: Requirements/OrderRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderRequest(RequirementsViewModel model)
        {
            // پر کردن نام کالاها
            foreach (var item in model.RequirementsItems)
            {
                item.CommodityName = _context.Commodities
                    .Where(c => c.CommodityId == item.CommodityId)
                    .Select(c => c.Name)
                    .FirstOrDefault() ?? "";
            }

            // بررسی حداقل یک انتخاب
            bool anySelected = model.RequirementsItems.Any(i => i.IsSelected && i.Quantity > 0);
            if (!anySelected)
                ModelState.AddModelError("", "هیچ کالایی انتخاب نشده است.");

            // ولیدیشن تعداد و انتخاب
            for (int i = 0; i < model.RequirementsItems.Count; i++)
            {
                var item = model.RequirementsItems[i];

                if (item.IsSelected && item.Quantity <= 0)
                    ModelState.AddModelError($"RequirementsItems[{i}].Quantity", "تعداد را وارد کنید.");

                if (!item.IsSelected && item.Quantity > 0)
                    ModelState.AddModelError($"RequirementsItems[{i}].IsSelected", "باید کالا را انتخاب کنید.");
            }

            if (!ModelState.IsValid)
                return View(model);

            // ایجاد سفارش
            var order = new Order
            {
                CreatedAt = DateTime.Now,
                RequestedBy = "دستیار",
                Items = model.RequirementsItems
                    .Where(i => i.IsSelected && i.Quantity > 0)
                    .Select(i => new OrderItem
                    {
                        CommodityId = i.CommodityId,
                        Quantity = i.Quantity
                    }).ToList()
            };

            order.Quantity = order.Items.Sum(i => i.Quantity);

            _context.Orders.Add(order);
            _context.SaveChanges();

            TempData["OrderId"] = order.OrderId;
            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            ViewBag.OrderId = TempData["OrderId"];
            return View();
        }

        // اکشن برای تولید و دانلود PDF
        //public IActionResult DownloadOrderPdf(int id)
        //{
        //    var order = _context.Orders
        //        .Where(o => o.OrderId == id)
        //        .Select(o => new
        //        {
        //            o.OrderId,
        //            o.CreatedAt,
        //            Items = o.Items.Select(i => new
        //            {
        //                CommodityName = i.Commodity.Name,
        //                i.Quantity
        //            }).ToList()
        //        })
        //        .FirstOrDefault();

        //    if (order == null)
        //        return NotFound();

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        Document doc = new Document(PageSize.A4, 30, 30, 30, 30);
        //        PdfWriter writer = PdfWriter.GetInstance(doc, ms);
        //        doc.Open();

        //        string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "font", "LMU VAZIR.TTF");
        //        BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //        var font = new iTextSharp.text.Font(bf, 12, Font.NORMAL, BaseColor.BLACK);
        //        var fontHeader = new iTextSharp.text.Font(bf, 14, Font.BOLD, BaseColor.BLACK);

        //        // تیتر
        //        PdfPTable titleTable = new PdfPTable(1) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_RTL };
        //        titleTable.AddCell(new PdfPCell(new Phrase("لیست سفارشات", fontHeader))
        //        {
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            Border = Rectangle.NO_BORDER,
        //            PaddingBottom = 10
        //        });
        //        doc.Add(titleTable);

        //        // تاریخ و ساعت
        //        var persianDate = PersianDateTime.ToPersianDateTimeString(order.CreatedAt);
        //        var parts = persianDate.Split(' ');
        //        PdfPTable dateTimeTable = new PdfPTable(2) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR };
        //        dateTimeTable.AddCell(new PdfPCell(new Phrase(parts[1], font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
        //        dateTimeTable.AddCell(new PdfPCell(new Phrase(parts[0], font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        doc.Add(dateTimeTable);
        //        doc.Add(new Paragraph("\n"));

        //        // جدول آیتم‌ها
        //        PdfPTable table = new PdfPTable(2) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_RTL };
        //        table.SetWidths(new float[] { 3f, 1f });
        //        table.AddCell(new PdfPCell(new Phrase("نام کالا", fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5 });
        //        table.AddCell(new PdfPCell(new Phrase("تعداد", fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5 });

        //        int totalQuantity = 0;

        //        foreach (var item in order.Items)
        //        {
        //            table.AddCell(new PdfPCell(new Phrase(item.CommodityName, font)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5 });
        //            table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5 });

        //            totalQuantity += item.Quantity;
        //        }

        //        // ردیف جمع کل
        //        table.AddCell(new PdfPCell(new Phrase("جمع کل", fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase(totalQuantity.ToString(), fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.LIGHT_GRAY });

        //        doc.Add(table);
        //        doc.Close();

        //        return File(ms.ToArray(), "application/pdf", $"Order_{order.OrderId}.pdf");
        //    }
        //}
    }
}
