using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.ViewModels.Authentication
{
    public class VerifyPhoneViewModel
    {
        [Required]
        public string Phone { get; set; }

        [Required(ErrorMessage = "کد تأیید الزامی است")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "کد تأیید باید بین 4 تا 6 رقم باشد")]
        public string Code { get; set; }
    }

}
