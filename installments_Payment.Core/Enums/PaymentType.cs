using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Enums
{
    public enum PaymentTypes
    {
        [Display(Name = "قسطی")]
        Installment = 1,

        [Display(Name = "نقدی")]
        Cash = 2
    }
}
