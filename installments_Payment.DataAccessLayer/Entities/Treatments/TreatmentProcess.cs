using installments_Payment.Core.Enums;
using installments_Payment.DataAccessLayer.Entities.Requests;
using installments_Payment.DataAccessLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static installments_Payment.Core.Enums.ProcessStatuses;
using static installments_Payment.Core.Enums.TreatmentStatuses;

namespace installments_Payment.DataAccessLayer.Entities.Treatments
{
    public class TreatmentProcess
    {
        [Key]
        public int TreatmentProcessId { get; set; }

        [Required]
        public int RequestId { get; set; }
        public Request Request { get; set; }

        [NotMapped]
        public string PatientName { get; set; }

        [Required]
        public string DoctorName { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "وضعیت درمان")]
        public TreatmentStatuses TreatmentStatus { get; set; } = TreatmentStatuses.Examination;
        public ProcessStatuses ProcessStatus { get; set; } = ProcessStatuses.NotEntered;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
