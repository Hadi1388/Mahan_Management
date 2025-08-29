using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Enums
{
    public enum GuaranteeTypes
    {
        [Display(Name = "چک")]
        Cheque = 1,

        [Display(Name = "سفته")]
        PromissoryNote = 2,

        [Display(Name = "نقد")]
        Cash = 3
    }
}
