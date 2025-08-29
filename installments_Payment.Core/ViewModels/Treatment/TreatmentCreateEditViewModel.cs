using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.ViewModels.Treatment
{
    public class TreatmentCreateEditViewModel
    {
        public int TreatmentId { get; set; }

        [Display(Name = "نام درمان")]
        [Required(ErrorMessage = "نام درمان الزامی است")]
        public string TreatmentName { get; set; }
    }
}
