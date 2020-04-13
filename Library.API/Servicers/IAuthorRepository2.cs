using Library.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Servicers
{
    public interface IAuthorRepository2 : IRepositoryBase<Author>, IRepositoryBase2<Author, Guid>
    {
    }
}
