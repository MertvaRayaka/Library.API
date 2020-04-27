using Library.API.Entities;
using Library.API.Extensions;
using Library.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Library.API.Servicers
{
    public class AuthorRepository2 : RepositoryBase<Author, Guid>, IAuthorRepository2
    {
        private Dictionary<string, PropertyMapping> mappingDic = null;

        public AuthorRepository2(DbContext dbContext) : base(dbContext)
        {
            mappingDic = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);
            mappingDic.Add("Name",new PropertyMapping("Name"));
            mappingDic.Add("Age", new PropertyMapping("BirthDate",true));
            mappingDic.Add("BirthPlace", new PropertyMapping("BirthPlace"));
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
                querableAuthors = querableAuthors.Where(p => p.BirthPlace.ToLower().Contains(parameters.SearchQuery.ToLower()) || p.Name.ToLower().Contains(parameters.SearchQuery.ToLower()));
            }
            //if (parameters.SortBy == "Name")
            //{
            //    querableAuthors = querableAuthors.OrderBy(author => author.Name);
            //}
            var orderedAuthors = querableAuthors.Sort(parameters.SortBy,mappingDic);
            return PagedList<Author>.CreateAsync(orderedAuthors, parameters.PageNumber, parameters.PageSize);
        }
    }
}
