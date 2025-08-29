using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Enums
{
    public enum ProcessStatuses
    {
        [Display(Name = "وارد نشده")]
        NotEntered = 1,

        [Display(Name = "انجام شد")]
        Done = 2,

        [Display(Name = "لغو شد")]
        Canceled = 3
    }
}
