using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.DataAccessLayer.Entities.Inventories
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string RequestedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool CheckByManager { get; set; } = false;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
