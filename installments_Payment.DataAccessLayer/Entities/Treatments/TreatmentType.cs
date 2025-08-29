using Azure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using installments_Payment.DataAccessLayer.Entities;

namespace installments_Payment.DataAccessLayer.Entities.Treatments
{
    public class TreatmentType
    {
        public int TreatmentTypeId { get; set; }

        [Required(ErrorMessage = "نوع درمان الزامی است")]
        [Display(Name = "نوع درمان")]
        public string TreatmentTypeName { get; set; }

        public ICollection<Requests.Request> Requests { get; set; }
    }
}
