using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Models;

namespace Library.API.Servicers
{
    public interface IAuthorRepository
    {
        IEnumerable<AuthorDto> GetAuthors();
        AuthorDto GetAuthor(Guid authorid);
        bool IsAuthorExists(Guid authorid);
        void AddAuthor(AuthorDto author);
        void DeleteAuthor(AuthorDto author);
    }
}
