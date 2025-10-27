using LibararyManagement.Data.Interface;
using LibararyManagement.Data.Models;
using LibraryManagement.Service.DTO.Borrow.Response;
using LibraryManagement.Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Service
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepo _repo;
        private readonly IBookRepo _repoBook;

        public BorrowService(IBorrowRepo repo, IBookRepo repoBook)
        {
            _repo = repo;
            _repoBook = repoBook;
        }

        public enum BookStatus
        {
            Pending,
            Completed,
            Overdue
        }



        public async Task<AddBorrowResponseDTO> AddBorrowBook(int userId, int bookId)
        {
            var book = await _repoBook.GetoneBookByIdRepo(bookId);

            if(book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            if(book.Stock == 0)
            {
                throw new ValidationException("Book out of Stock");
            }

            var existsBorrowed = await _repo.BookBorrowedExits(bookId, userId);

            if(existsBorrowed != null)
            {
                throw new ValidationException("You Alrady Borrowed this one");
            }

            book.Stock -= 1;

            await _repoBook.UpdateBookRepo(book);

            var borrowbook = new Borrow
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                Status = BookStatus.Pending.ToString(),
            };

            // Orignal Model
            var model = await _repo.BorrowBookRepo(borrowbook);

            var bokBorrwedModel = await _repo.BorrowGetById(model.Id);

            return new AddBorrowResponseDTO
            {
                Book = bokBorrwedModel.Book.BookName,
                UserId = bokBorrwedModel.UserId,
                Status = bokBorrwedModel.Status,
                ReturnDate = bokBorrwedModel.ReturnDate,
                BorrowDate = bokBorrwedModel.BorrowDate,
                Id = bokBorrwedModel.Id,
            };
        }
    }
}
