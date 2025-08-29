using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.DataAccessLayer.Entities.Requests
{
    public class RequestGuaranteeFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }
        public Request Request { get; set; }

        [Required]
        [Display(Name = "شماره چک یا سفته")]
        public string GuaranteeNumber { get; set; }

        [Required]
        [Display(Name = "مسیر فایل وثیقه")]
        public string FilePath { get; set; }
    }
}
