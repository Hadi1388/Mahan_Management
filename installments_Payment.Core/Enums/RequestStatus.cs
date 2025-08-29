using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Enums
{
    public enum RequestStatuses
    {
        [Display(Name = "در انتظار")]
        Pending = 0,

        [Display(Name = "تایید شده")]
        Approved = 1,

        [Display(Name = "لغو شده")]
        Rejected = 2
    }
}
