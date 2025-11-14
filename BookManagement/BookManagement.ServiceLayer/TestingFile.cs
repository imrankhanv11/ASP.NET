using BookManagement.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer
{
    public class TestingFile
    {
        private readonly ITestingService _service;

        public TestingFile(ITestingService service)
        {
            _service = service;
        }

        public bool EvenOfAdd(int a, int b)
        {
            var result = _service.AddNumbers(a, b);

            if(result == 0)
            {
                return false;
            }
            else if (result % 2 == 0)
            {
                return true;
            }

            return false;
        }

        public string concatString(string a, string b)
        {
            var result = $"{a}{b}";

            return result; 
        }
    }
}
