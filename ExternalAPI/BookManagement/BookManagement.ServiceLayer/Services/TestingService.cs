using BookManagement.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.Services
{
    public class TestingService : ITestingService
    {
        public int AddNumbers(int a, int b)
        {
            return a + b;
        }

    }
}
