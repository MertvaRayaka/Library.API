using Library.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Library.API.Servicers
{
    public interface IBookRepository2:IRepositoryBase<Book>,IRepositoryBase2<Book,Guid>
    {
        Task<IEnumerable<Book>> GetBooksAsync(Guid authorid);
        Task<Book> GetBookAsync(Guid authorid,Guid bookid);
    }
}
