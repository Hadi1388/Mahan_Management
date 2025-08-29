using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.DataAccessLayer.Entities.Inventories
{
    public class Commodity
    {
        [Key]
        public int CommodityId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
