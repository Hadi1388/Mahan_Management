using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.ViewModels.Inventory
{
    public class OrderItemViewModel
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int Quantity { get; set; }
    }
}
