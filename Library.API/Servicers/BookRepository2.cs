using Library.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Library.API.Servicers
{
    public class BookRepository2 : RepositoryBase<Book, Guid>, IBookRepository2
    {
        public BookRepository2(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
