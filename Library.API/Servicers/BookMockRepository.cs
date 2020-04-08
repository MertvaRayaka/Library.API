using Library.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Data;

namespace Library.API.Servicers
{
    public class BookMockRepository : IBookRespository
    {
        public void AddBook(BookDto book)
        {
            LibraryMockData.Current.Books.Add(book);
        }

        public void DeleteBook(BookDto book)
        { 
            LibraryMockData.Current.Books.Remove(book);
        }

        public BookDto GetBookForAuthor(Guid authorid, Guid bookid)
        {
            var book = LibraryMockData.Current.Books.FirstOrDefault(p => p.AuthorId == authorid && p.Id == bookid);
            return book;
        }

        public IEnumerable<BookDto> GetBooksForAuthor(Guid authorid)
        {
            var books = LibraryMockData.Current.Books.Where(p => p.AuthorId == authorid);
            return books;
        }

        public void UpdateBook(Guid authorid, Guid bookid, BookForUpdateDto bookForUpdateDto)
        {
            var book = GetBookForAuthor(authorid,bookid);
            book.Description = bookForUpdateDto.Description;
            book.Pages = bookForUpdateDto.Pages;
            book.Title = bookForUpdateDto.Title;
        }
    }
}
