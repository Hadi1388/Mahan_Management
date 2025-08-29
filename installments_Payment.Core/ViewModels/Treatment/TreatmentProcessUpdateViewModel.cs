using installments_Payment.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.ViewModels.Treatment
{
    public class TreatmentProcessUpdateViewModel
    {
        public int TreatmentProcessId { get; set; }

        [Display(Name = "وضعیت درمان")]
        public TreatmentStatuses TreatmentStatus { get; set; }

        [Display(Name = "وضعیت انجام")]
        public ProcessStatuses ProcessStatus { get; set; }
    }
}
