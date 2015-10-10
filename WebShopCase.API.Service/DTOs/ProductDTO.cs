using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCase.API.Service
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public string ProductPct { get; set; }

        public string CategoryName { get; set; }
        public string CategoryPct { get; set; }
        public decimal VAT { get; set; }

        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
    }
}
