using Library.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Servicers
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IAuthorRepository2 authorRepository2 = null;
        private IBookRepository2 bookRepository2 = null;

        public LibraryDbContext LibraryDbContext { get; }

        public RepositoryWrapper(LibraryDbContext libraryDbContext)
        {
            LibraryDbContext = libraryDbContext;
        }
        public IBookRepository2 Book => bookRepository2 ?? new BookRepository2(LibraryDbContext);

        public IAuthorRepository2 Author => authorRepository2 ?? new AuthorRepository2(LibraryDbContext);
    }
}
