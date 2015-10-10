using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCase.API.Domain
{
    public class Wish
    {
        public int WishID { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual ICollection<Product> ProdWishes { get; set; }
        public Wish()
        {
            this.ProdWishes = new HashSet<Product>();
        }
    }
}
