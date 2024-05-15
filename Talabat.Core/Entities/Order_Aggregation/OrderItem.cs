using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{//OI=>POI=>1:1 Total
    public class OrderItem:BaseEntity
    {
        public OrderItem(ProductOrderItem productItemOrdered, decimal price, int quantity)
        {
            Price = price;
            Quantity = quantity;
        }

        public ProductOrderItem Product {  get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
