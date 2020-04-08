using Library.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Data;
using System.Runtime.InteropServices;

namespace Library.API.Servicers
{
    public class AuthorMockRepository : IAuthorRepository
    {
        public void AddAuthor(AuthorDto author)
        {
            author.Id = Guid.NewGuid();
            LibraryMockData.Current.Authors.Add(author);
        }

        public void DeleteAuthor(AuthorDto author)
        {
            //先删除子资源，再删除父资源
            LibraryMockData.Current.Books.RemoveAll(p => p.AuthorId == author.Id);
            LibraryMockData.Current.Authors.Remove(author);
        }

        public AuthorDto GetAuthor(Guid authorid)
        {
            var author = LibraryMockData.Current.Authors.FirstOrDefault(p => p.Id == authorid);
            return author;
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            return LibraryMockData.Current.Authors;
        }

        public bool IsAuthorExists(Guid authorid)
        {
            return LibraryMockData.Current.Authors.Any(p => p.Id == authorid);
        }


    }
}
