using Library.API.Entities;
using Library.API.Helpers;
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

        public Task<PagedList<Author>> GetAllAsync(AuthorResourceParameters parameters)
        {
            IQueryable<Author> querableAuthors = DbContext.Set<Author>();
            if (!string.IsNullOrWhiteSpace(parameters.BirthPlace))
            {
                querableAuthors = querableAuthors.Where(p => p.BirthPlace == parameters.BirthPlace);
            }
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                querableAuthors = querableAuthors.Where(p => p.BirthPlace.ToLower().Contains(parameters.SearchQuery.ToLower())|| p.Name.ToLower().Contains(parameters.SearchQuery.ToLower()));
            }
            return PagedList<Author>.CreateAsync(querableAuthors,parameters.PageNumber,parameters.PageSize);
        }
    }
}
