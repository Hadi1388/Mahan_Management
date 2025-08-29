using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace installments_Payment.Core.ViewModels.Patient
{
    public class PatientEditInfoViewModel
    {
        [Required(ErrorMessage = "لطفاً شماره تماس دوم را وارد کنید")]
        [Phone(ErrorMessage = "شماره تماس دوم معتبر نیست")]
        [Display(Name = "شماره تلفن دوم")]
        public string SecondaryPhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفاً کد ملی را وارد کنید")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید دقیقاً 10 رقم باشد")]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        // اگر تصویر اجباری نیست، ولیدیشن را بردار:
        [Display(Name = "تصویر کارت ملی")]
        public IFormFile? NationalCardImageFile { get; set; }

        [Required(ErrorMessage = "لطفاً آدرس را وارد کنید")]
        [StringLength(500, ErrorMessage = "آدرس نمی‌تواند بیشتر از 500 کاراکتر باشد")]
        [Display(Name = "آدرس")]
        public string Address { get; set; }

        // مسیر تصویر را فقط نگهدار ولی لازم نیست ورودی باشه
        public string? NationalCardImagePath { get; set; }


        public bool isRequestCompleted { get; set; }
    }
}
