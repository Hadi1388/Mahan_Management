using installments_Payment.DataAccessLayer.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.DataAccessLayer.Entities.Installments
{
    public class Installment
    {
        public int InstallmentId { get; set; }
        public int? RequestId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Paid { get; set; }

        public decimal? PenaltyAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? ReceiptImagePath { get; set; }
        public bool PaidByGateway { get; set; }

        public virtual Request Request { get; set; }
    }

}
