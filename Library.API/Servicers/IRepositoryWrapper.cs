using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Servicers
{
    public interface IRepositoryWrapper
    {
        IBookRepository2 Book { get; }

        IAuthorRepository2 Author { get; } 
    }
}
