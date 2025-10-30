using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Tests.TestData
{
    public class EvenOfAddDataClass
    {
        public static IEnumerable<object[]> EvenOfAddDataClassObj => new List<Object[]>
        {
            new object[] {1,1,2, true},
            new object[] {2,1,3, false}
        };
    }
}
