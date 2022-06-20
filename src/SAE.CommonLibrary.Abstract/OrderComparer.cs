using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract
{
    public class OrderComparer : IComparer<object>
    {
        public static OrderComparer Comparer = new OrderComparer();
        public int Compare(object x, object y)
        {
            var xIndex = (x != null && x is IOrdered) ? ((IOrdered)x).Order : 0;
            var yIndex = (y != null && y is IOrdered) ? ((IOrdered)y).Order : 0;
            return xIndex.CompareTo(yIndex);
        }
    }
}
