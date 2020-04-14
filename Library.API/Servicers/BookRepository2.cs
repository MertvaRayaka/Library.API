using Library.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Servicers
{
    public class BookRepository2 : RepositoryBase<Book, Guid>, IBookRepository2
    {
        public BookRepository2(DbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Book> GetBookAsync(Guid authorid, Guid bookid)
        {
            return await DbContext.Set<Book>().SingleOrDefaultAsync(book => book.AuthorId == authorid && book.Id == bookid);
        }

        public Task<IEnumerable<Book>> GetBooksAsync(Guid authorid)
        {
            return Task.FromResult(DbContext.Set<Book>().Where(p => p.AuthorId == authorid).AsEnumerable());
        }
    }
}
