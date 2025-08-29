using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.DataAccessLayer.Entities.Inventories
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int CommodityId { get; set; }
        public Commodity Commodity { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
