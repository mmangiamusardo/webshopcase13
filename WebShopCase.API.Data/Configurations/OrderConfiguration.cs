using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCase.API.Domain;

namespace WebShopCase.API.Data.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Orders");
            
            Property(o => o.ShipName).IsRequired();
            Property(o => o.ShipAddress).IsRequired();
            Property(o => o.ShipCity).IsRequired();
            Property(o => o.ShipCountry).IsRequired();
            Property(o => o.ShipPostalCode).IsRequired();
        }
    }
}
