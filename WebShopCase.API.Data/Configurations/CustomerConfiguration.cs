using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCase.API.Domain;

namespace WebShopCase.API.Data.Configurations
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("Customers");
            /*
            Property(o => o.FirstName).IsRequired();
            Property(o => o.LastName).IsRequired();
            Property(o => o.City).IsRequired();
            Property(o => o.Country).IsRequired();
            Property(o => o.Address).IsRequired();
            Property(o => o.PostalCode).IsRequired();
            */
         }
    }
}
