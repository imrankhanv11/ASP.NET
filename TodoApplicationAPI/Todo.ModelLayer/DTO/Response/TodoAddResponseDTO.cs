using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Todo.ModelLayer.DTO.Response
{
    public class TodoAddResponseDTO
    {
        public int Id { get; set; }
        //[JsonPropertyName("user_id")]
        public int UserId { get; set; }

        //[JsonPropertyName("category_id")]
        public int? CategoryId { get; set; }

        //[JsonPropertyName("status_id")]
        public int? StatusId { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }
    }
}
