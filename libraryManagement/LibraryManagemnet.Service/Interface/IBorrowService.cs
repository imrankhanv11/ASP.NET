using LibraryManagement.Service.DTO.Borrow.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Interface
{
    public interface IBorrowService
    {
        Task<AddBorrowResponseDTO> AddBorrowBook(int userId, int bookId);
    }
}
