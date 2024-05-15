using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{
    public class Order:BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingaddress,DeliveryMethod deliveryMethod,ICollection<OrderItem>items,decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingaddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subtotal;
        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate= DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }//navigational ProP
        public ICollection<OrderItem> Items { get; set; }=new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
        public decimal GetTotal()
        =>   SubTotal + DeliveryMethod.Cost;
        public string PaymentIntendId { get; set; }=string.Empty;
    }
}
