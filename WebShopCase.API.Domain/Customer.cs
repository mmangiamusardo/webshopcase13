﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCase.API.Domain
{
    public class Customer
    {
        public int CustomerID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public Customer()
        {
            this.Orders = new HashSet<Order>();
        }

        public string AppUserID { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}