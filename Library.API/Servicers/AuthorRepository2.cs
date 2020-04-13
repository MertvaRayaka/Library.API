using Library.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Servicers
{
    public class AuthorRepository2 : RepositoryBase<Author, Guid>,IAuthorRepository2
    {
        public AuthorRepository2(DbContext dbContext):base(dbContext)
        {

        }
    }
}
