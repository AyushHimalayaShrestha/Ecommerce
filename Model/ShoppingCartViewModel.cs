using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> shoppingCarts;
        /*        public double Total;
         *                public OrderHeader OrderHeader { get; set; }

        */
        public OrderHeader OrderHeader { get; set; }
    }
}
