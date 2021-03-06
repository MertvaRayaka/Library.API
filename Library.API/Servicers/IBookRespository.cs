﻿using Library.API.Models;
using System;
using System.Collections.Generic;

namespace Library.API.Servicers
{
    public interface IBookRespository
    {
        IEnumerable<BookDto> GetBooksForAuthor(Guid authorid);
        BookDto GetBookForAuthor(Guid authorid, Guid bookid);
        void AddBook(BookDto book);
        void DeleteBook(BookDto book);
        void UpdateBook(Guid authorid, Guid bookid, BookForUpdateDto bookForUpdateDto);
    }
}
