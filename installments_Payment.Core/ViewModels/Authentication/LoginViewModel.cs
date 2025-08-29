using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace installments_Payment.Core.ViewModels.Authentication
{

    public class LoginViewModel
    {
        [Required(ErrorMessage = "لطفاً شماره تلفن را وارد کنید")]
        [Phone(ErrorMessage = "شماره تلفن وارد شده معتبر نیست")]
        [Display(Name = "شماره تلفن")]
        public string PrimaryPhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفاً رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
    }
}
