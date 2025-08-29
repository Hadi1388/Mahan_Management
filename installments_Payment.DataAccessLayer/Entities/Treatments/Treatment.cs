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
    public class Treatment
    {
        public int TreatmentId { get; set; }

        [Required(ErrorMessage = "نام درمان الزامی است")]
        [Display(Name = "نام درمان")]
        public string TreatmentName { get; set; }

        public ICollection<Requests.Request> Requests { get; set; }
    }

}
