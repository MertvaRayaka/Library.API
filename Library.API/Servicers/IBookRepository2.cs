using Library.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Servicers
{
    public interface IBookRepository2:IRepositoryBase<Book>,IRepositoryBase2<Book,Guid>
    {

    }
}
