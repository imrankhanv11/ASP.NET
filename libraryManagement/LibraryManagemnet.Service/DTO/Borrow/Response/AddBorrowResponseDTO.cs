using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.DTO.Borrow.Response
{
    public class AddBorrowResponseDTO
    {
        public int Id { get; set; }
        public string Book { get; set; }

        public int UserId { get; set; }

        public DateOnly BorrowDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public string Status { get; set; } = null!;
    }
}
