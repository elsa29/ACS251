using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscountInterface;

namespace DiscountClass
{    
    internal class StudentDiscount : IDiscount
    {
        public double Calculate(double price)
        {
            double dis = 0.8;
            return (price * dis);
        }
    }

    internal class CitiCreditCardDiscount : IDiscount
    {
        public double Calculate(double price)
        {
            double dis = 0.5;
            return (price * dis);
        }
    }

    internal class Children10Discount : IDiscount
    {
        public double Calculate(double price)
        {
            double dis = 0.3;
            return (price * dis);
        }
    }

    internal class ElementaryDiscount : IDiscount
    {
        public double Calculate(double price)
        {
            double dis = 0.7;
            return (price * dis);
        }
    }
    
}
