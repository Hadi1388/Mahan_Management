using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.ViewModels.Inventory
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string CommodityName { get; set; }
        public int Quantity { get; set; }
        public string RequestedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool CheckByManager { get; set; }

        public List<OrderItemViewModel> Items { get; set; } = new();
    }
}
