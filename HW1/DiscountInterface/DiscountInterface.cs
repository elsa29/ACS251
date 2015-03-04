using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountInterface
{
    public interface IDiscount
    {
        double Calculate(double price);
    }
}
