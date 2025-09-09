using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.ModelLayer.DTO.Request
{
    public class AddCatDTO
    {
        [Required]
        [StringLength(50, MinimumLength =3, ErrorMessage ="Minimum More than 3 char")]
        public string name { get; set; }
    }
}
