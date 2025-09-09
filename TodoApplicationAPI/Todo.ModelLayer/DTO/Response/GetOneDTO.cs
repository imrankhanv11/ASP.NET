using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.ModelLayer.DTO.Response
{
    public class GetOneDTO
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly CreatedDate { get; set; }
    }
}
