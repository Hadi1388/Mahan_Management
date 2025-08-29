using installments_Payment.Core.Extensions;
using installments_Payment.DataAccessLayer.Context;
//using iTextSharp.text.pdf;
//using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace installments_Payment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        public OrdersController(AppDbContext context)
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

        [HttpPost]
        public async Task<IActionResult> ConfirmCheck(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.CheckByManager = true;
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

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

        //        // تاریخ
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

        //        // جمع کل
        //        table.AddCell(new PdfPCell(new Phrase("جمع کل", fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5 });
        //        table.AddCell(new PdfPCell(new Phrase(totalQuantity.ToString(), fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5 });

        //        doc.Add(table);
        //        doc.Close();

        //        return File(ms.ToArray(), "application/pdf", $"Order_{order.OrderId}.pdf");
        //    }
        //}
    }
}
