using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.DataAccessLayer.Entities.Installments
{
    public class InstallmentPenalty
    {
        public int Id { get; set; }
        public double DailyPenaltyPercent { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
