using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.ViewModels.Request
{
    public class UploadMultipleGuaranteeFilesViewModel
    {
        public int RequestId { get; set; }

        [Required(ErrorMessage = "شماره چک یا سفته الزامی است")]
        public string GuaranteeNumber { get; set; }

        [Required(ErrorMessage = "آپلود فایل الزامی است")]
        public IFormFile File { get; set; }
        public string PatientFullName { get; set; }
    }

}
