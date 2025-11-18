using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.ServiceLayer.DTO.Category.Request
{
    public class UpdateCatRequestDTO
    {
        public int id { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
