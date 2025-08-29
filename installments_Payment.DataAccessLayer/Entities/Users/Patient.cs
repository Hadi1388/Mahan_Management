using System;
using System.ComponentModel.DataAnnotations;

namespace installments_Payment.DataAccessLayer.Entities.Users
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفاً نام و نام خانوادگی را وارد کنید")]
        [Display(Name = "نام و نام خانوادگی")]
        [StringLength(100, ErrorMessage = "نام کامل نمی‌تواند بیشتر از 100 کاراکتر باشد")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "لطفاً شماره تماس را وارد کنید")]
        [Display(Name = "شماره تماس")]
        [Phone(ErrorMessage = "شماره تماس وارد شده معتبر نیست")]
        public string PrimaryPhoneNumber { get; set; }

        [Display(Name = "شماره تماس دوم")]
        [Phone(ErrorMessage = "شماره تماس دوم معتبر نیست")]
        [Required(ErrorMessage = "لطفاً شماره تماس دوم را وارد کنید")]
        public string SecondaryPhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفاً کد ملی را وارد کنید")]
        [Display(Name = "کد ملی")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید دقیقاً 10 رقم باشد")]
        public string NationalCode { get; set; }

        [Display(Name = "تصویر کارت ملی")]
        public string NationalCardImagePath { get; set; }

        [Display(Name = "آدرس")]
        [StringLength(500, ErrorMessage = "آدرس نمی‌تواند بیشتر از 500 کاراکتر باشد")]
        [Required(ErrorMessage = "لطفاً آدرس را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفاً رمز عبور را وارد کنید")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // 📌 فیلدهای مربوط به ورود با کد تأیید
        [Display(Name = "تأیید شماره موبایل")]
        public bool IsPhoneVerified { get; set; } = false;

        [Display(Name = "کد تأیید")]
        public string PhoneVerificationCode { get; set; }

        [Display(Name = "تاریخ انقضای کد تأیید")]
        public DateTime? PhoneVerificationExpiry { get; set; }

        // 📌 فیلدهای سیستمی
        [Display(Name = "تاریخ ثبت")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "تاریخ آخرین بروزرسانی")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "آخرین ورود")]
        public DateTime? LastLoginAt { get; set; }
    }
}
