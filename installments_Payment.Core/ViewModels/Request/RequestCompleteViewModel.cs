using Microsoft.AspNetCore.Http;
using static installments_Payment.Core.Enums.GuaranteeTypes;
using System.ComponentModel.DataAnnotations;
using static installments_Payment.Core.Enums.PaymentTypes;
using installments_Payment.Core.Enums;

namespace installments_Payment.Core.ViewModels.Request
{
    public class RequestCompleteViewModel
    {
        public int RequestId { get; set; }

        // اطلاعات بیمار
        [Display(Name = "نام و نام خانوادگی بیمار")]
        public string FullName { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "کارت ملی")]
        public string NationalCardImagePath { get; set; }

        [Display(Name = "شماره تماس اصلی")]
        public string PrimaryPhoneNumber { get; set; }

        [Display(Name = "شماره تماس دوم")]
        public string SecondaryPhoneNumber { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        // از سمت ادمین
        [Required(ErrorMessage = "نوع درمان الزامی است")]
        [Display(Name = "نوع درمان")]
        public int? TreatmentTypeId { get; set; }

        [Required(ErrorMessage = "درمان الزامی است")]
        [Display(Name = "درمان")]
        public int? TreatmentId { get; set; }

        [Required(ErrorMessage = "تاریخ شروع درمان الزامی است")]
        //[DataType(DataType.Date)]
        [Display(Name = "تاریخ شروع درمان")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "نوع پرداخت الزامی است")]
        [Display(Name = "نوع پرداخت")]
        public PaymentTypes? PaymentType { get; set; }

        [Required(ErrorMessage = "نوع وثیقه الزامی است")]
        [Display(Name = "نوع وثیقه")]
        public GuaranteeTypes? GuaranteeType { get; set; }

        [Required(ErrorMessage = "نام پزشک الزامی است")]
        [Display(Name = "نام پزشک")]
        public string? DoctorName { get; set; }

        [Required(ErrorMessage = "مبلغ کل الزامی است")]
        [Range(0, double.MaxValue, ErrorMessage = "مبلغ کل معتبر نیست")]
        [Display(Name = "مبلغ کل")]
        public int? TotalAmount { get; set; }

        [Required(ErrorMessage = "پیش‌پرداخت الزامی است")]
        [Range(0, double.MaxValue, ErrorMessage = "پیش‌پرداخت معتبر نیست")]
        [Display(Name = "پیش‌پرداخت")]
        public int? Prepayment { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "تعداد اقساط باید حداقل ۱ باشد")]
        [Display(Name = "تعداد اقساط")]
        public int? InstallmentsCount { get; set; }

        [Display(Name = "فایل‌های ضمانت")]
        public List<IFormFile> GuaranteeFiles { get; set; }

        [Display(Name = "شماره‌های ضمانت")]
        public List<string> GuaranteeNumbers { get; set; }
    }
}
