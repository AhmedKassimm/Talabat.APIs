using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{// 1 to 1 Total
    public class Address
    {
        public Address()
        {
            
        }
        public Address(string fname,string lname,string street,string city,string country)
        {
            FName = fname;
            LName = lname;
            Street = street;
            City = city;
            Country = country;
        }
        public string FName { get; set; }
        public string LName { get; set; }   
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
