using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{ // DM=> Order 1-M
    public class DeliveryMethod:BaseEntity
    {
        public DeliveryMethod()
        {
            
        }
        public DeliveryMethod(string shortname,string description,decimal cost,string deliverytime)
        {
            ShortName = shortname;
            Description = description;
            Cost = cost;
            DeliveryTime = deliverytime;
        }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string DeliveryTime { get; set; }
    }
}
