using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCase.API.Domain
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
    }
}
