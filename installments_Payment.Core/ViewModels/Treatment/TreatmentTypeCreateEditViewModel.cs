using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.ViewModels.Treatment
{
    public class TreatmentTypeCreateEditViewModel
    {
        public int TreatmentTypeId { get; set; }

        [Required(ErrorMessage = "نوع درمان الزامی است")]
        [Display(Name = "نوع درمان")]
        public string TreatmentTypeName { get; set; }
    }
}
