using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.ModelLayer.Models
{
    public class RefershToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public string UserEmail { get; set; } = null!;

        public string Roll {  get; set; } = null!;  
        public DateTime Expiration { get; set; }
        public bool IsRevoked { get; set; }
    }
}
