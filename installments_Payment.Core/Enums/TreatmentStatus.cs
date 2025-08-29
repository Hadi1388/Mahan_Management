using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Enums
{
    public enum TreatmentStatuses
    {
        [Display(Name = "معاینه")]
        Examination = 0,
        [Display(Name = "ادامه درمان")]
        InProgress = 1,
        [Display(Name = "پایان درمان")]
        Completed = 2
    }
}
