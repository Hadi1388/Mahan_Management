using installments_Payment.DataAccessLayer.Entities.Treatments;
using installments_Payment.DataAccessLayer.Entities.Users;
using static installments_Payment.Core.Enums.PaymentTypes;
using static installments_Payment.Core.Enums.GuaranteeTypes;
using static installments_Payment.Core.Enums.RequestStatuses;
using System.ComponentModel.DataAnnotations;
using installments_Payment.DataAccessLayer.Entities.Requests;
using installments_Payment.Core.Extensions;
using installments_Payment.Core.Enums;

namespace installments_Payment.DataAccessLayer.Entities.Requests
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [Display(Name = "بیمار")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [RequiredIfNotNullOrEmpty(ErrorMessage = "نام درمان الزامی است")]
        [Display(Name = "نام درمان")]
        public int? TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        [RequiredIfNotNullOrEmpty(ErrorMessage = "نوع درمان الزامی است")]
        [Display(Name = "نوع درمان")]
        public int? TreatmentTypeId { get; set; }
        public TreatmentType TreatmentType { get; set; }

        [RequiredIfNotNullOrEmpty(ErrorMessage = "تاریخ شروع درمان الزامی است")]
        [Display(Name = "تاریخ شروع درمان")]
        public DateTime? StartDate { get; set; }

        [RequiredIfNotNullOrEmpty(ErrorMessage = "نوع پرداخت الزامی است")]
        [Display(Name = "نوع پرداخت")]
        public PaymentTypes? PaymentType { get; set; }

        [RequiredIfNotNullOrEmpty(ErrorMessage = "نوع وثیقه الزامی است")]
        [Display(Name = "نوع وثیقه")]
        public GuaranteeTypes? GuaranteeType { get; set; }

        public ICollection<RequestGuaranteeFile> GuaranteeFiles { get; set; }

        [Required(ErrorMessage = "نام پزشک الزامی است")]
        [Display(Name = "نام پزشک")]
        public string? DoctorName { get; set; }

        [RequiredIfNotNullOrEmpty(ErrorMessage = "مبلغ کل الزامی است")]
        [Display(Name = "مبلغ کل")]
        [DataType(DataType.Currency)]
        public int? TotalAmount { get; set; }

        [RequiredIfNotNullOrEmpty(ErrorMessage = "پیش پرداخت الزامی است")]
        [Display(Name = "پیش پرداخت")]
        [DataType(DataType.Currency)]
        public int? Prepayment { get; set; }

        [RequiredIfNotNullOrEmpty(ErrorMessage = "تعداد اقساط الزامی است")]
        [Display(Name = "تعداد اقساط")]
        public int? InstallmentsCount { get; set; }

        [Display(Name = "وضعیت درخواست")]
        public RequestStatuses Status { get; set; } = RequestStatuses.Pending;

        [Display(Name = "تاریخ ثبت درخواست")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "تاریخ آخرین بروزرسانی")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
