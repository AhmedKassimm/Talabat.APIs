using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderSpecification:BaseSpecification<Order>
    {
       

        public OrderSpecification(string buyerEmail):base(O=>O.BuyerEmail==buyerEmail)
        {
            // Set up your criteria, includes, and order by here
            
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderByDescending(o => o.OrderDate);
         
        }
        public OrderSpecification(int id,string buyerEmail) : base(O => O.BuyerEmail == buyerEmail&&O.Id==id)
        {
            // Set up your criteria, includes, and order by here

            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

        }
    }
}
