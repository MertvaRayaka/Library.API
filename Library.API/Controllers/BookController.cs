using AutoMapper;
using Library.API.Entities;
using Library.API.Filters;
using Library.API.Models;
using Library.API.Servicers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    [ServiceFilter(typeof(CheckAuthorExistFilterAttribute))]
    public class BookController : ControllerBase
    {
        //public ILogger<BookController> Logger { get; }
        //public IAuthorRepository AuthorRepository { get; }
        //public IBookRespository BookRespository { get; }
        //public BookController(IAuthorRepository authorRepository, IBookRespository bookRespository, ILogger<BookController> logger)
        //{
        //    AuthorRepository = authorRepository;
        //    BookRespository = bookRespository;
        //    Logger = logger;
        //}

        //[HttpGet]
        //public ActionResult<List<BookDto>> GetBooks(Guid authorId)
        //{
        //    Logger.LogInformation(1, "执行一次返回指定作者所有图书");
        //    if (!AuthorRepository.IsAuthorExists(authorId))
        //    {
        //        return NotFound();
        //    }
        //    return BookRespository.GetBooksForAuthor(authorId).ToList();
        //}

        //[HttpGet("{bookId}", Name = nameof(GetSpecifyBook))]
        //public ActionResult<BookDto> GetSpecifyBook(Guid authorid, Guid bookid)
        //{
        //    Logger.LogInformation(2, "执行一次返回指定作者的指定图书");
        //    if (!AuthorRepository.IsAuthorExists(authorid))
        //    {
        //        return NotFound();
        //    }

        //    var target = BookRespository.GetBookForAuthor(authorid, bookid);

        //    if (target == null)
        //    {
        //        return NotFound();
        //    }
        //    return BookRespository.GetBookForAuthor(authorid, bookid);
        //}

        //[HttpPost]
        ////authorid从BookController定义的路由中获取(URL的一部分字符串)
        ////bookForCreation从解析的消息正文(POST上来的Body)中获取
        //public IActionResult CreateBookDto(Guid authorid, BookForCreationDto bookForCreation)
        //{
        //    if (!AuthorRepository.IsAuthorExists(authorid))
        //    {
        //        return NotFound();
        //    }

        //    var BookDto = new BookDto
        //    {
        //        Id = Guid.NewGuid(),
        //        Title = bookForCreation.Title,
        //        Pages = bookForCreation.Pages,
        //        Description = bookForCreation.Description,
        //        AuthorId = authorid
        //    };

        //    BookRespository.AddBook(BookDto);
        //    return CreatedAtRoute(nameof(GetSpecifyBook), new { authorId = authorid, bookID = BookDto.Id }, BookDto);
        //}

        //[HttpDelete("{bookid}")]
        //public IActionResult DeleteBookDto(Guid authorid, Guid bookid)
        //{
        //    if (!AuthorRepository.IsAuthorExists(authorid))
        //    {
        //        return NotFound();
        //    }
        //    var book = BookRespository.GetBookForAuthor(authorid, bookid);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }
        //    BookRespository.DeleteBook(book);
        //    return NoContent();
        //}

        //[HttpPut("{bookid}")]
        ////updatedbook从Body中获取，数据模型绑定
        ////HttpPut：对资源的每个属性都更新，若属性不赋值，则为null或0
        //public ActionResult<BookDto> UpdateBookDto(Guid authorid, Guid bookid, BookForUpdateDto updatedbook)
        //{
        //    if (!AuthorRepository.IsAuthorExists(authorid))
        //    {
        //        return NotFound();
        //    }
        //    var book = BookRespository.GetBookForAuthor(authorid, bookid);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }
        //    BookRespository.UpdateBook(authorid, bookid, updatedbook);
        //    return BookRespository.GetBookForAuthor(authorid, bookid);
        //}

        //[HttpPatch("{bookid}")]
        //public ActionResult<BookDto> PartiallyUpdateBookDto(Guid authorid, Guid bookid, [FromBody]JsonPatchDocument<BookForUpdateDto> patchDocument)
        //{
        //    if (!AuthorRepository.IsAuthorExists(authorid))
        //    {
        //        return NotFound();
        //    }
        //    var book = BookRespository.GetBookForAuthor(authorid, bookid);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    var bookToPatch = new BookForUpdateDto
        //    {
        //        Title = book.Title,
        //        Description = book.Description,
        //        Pages = book.Pages
        //    };

        //    string Error = string.Empty;
        //    patchDocument.ApplyTo(bookToPatch, p =>
        //     {
        //         Error = p.ErrorMessage;
        //         Logger.LogInformation(p.ErrorMessage);
        //     });
        //    if (!String.IsNullOrEmpty(Error))
        //    {
        //        return BadRequest(Error);
        //    }

        //    BookRespository.UpdateBook(authorid, bookid, bookToPatch);
        //    return BookRespository.GetBookForAuthor(authorid, bookid);
        //}


        public ILogger<BookController> Logger { get; }
        public IRepositoryWrapper RepositoryWrapper { get; }
        public IMapper Mapper { get; }

        public BookController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<BookController> logger)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetSpecifyAuthorBooks(Guid authorid)
        {
            var books = await RepositoryWrapper.Book.GetBooksAsync(authorid);

            if (books.Count() == 1)
            {
                RepositoryWrapper.Book.Create(new Book
                {
                    AuthorId = authorid,
                    Description = "超好看的书",
                    Id = Guid.NewGuid(),
                    Page = 999,
                    Title = "文章の标题"
                });

                await RepositoryWrapper.Book.SaveAsync();
            }

            var bookDtoList = Mapper.Map<IEnumerable<BookDto>>(books);

            return bookDtoList.ToList();
        }

        [HttpGet("{bookid}", Name = nameof(GetBookAsync))]
        public async Task<ActionResult<BookDto>> GetBookAsync(Guid authorid, Guid bookid)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorid, bookid);
            if (book == null)
            {
                return NotFound();
            }
            var bookDto = Mapper.Map<BookDto>(book);
            return bookDto;
        }

        [HttpPost]
        public async Task<ActionResult> AddBookAsync([FromRoute]Guid authorid, [FromBody]BookForCreationDto bookForCreationDto)
        {
            var book = Mapper.Map<Book>(bookForCreationDto);

            book.AuthorId = authorid;

            RepositoryWrapper.Book.Create(book);

            if (!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("创建资源Book失败");
            }

            var bookDto = Mapper.Map<BookDto>(book);

            return CreatedAtRoute(nameof(GetBookAsync), new { AuthorId = authorid, BOOKID = bookDto.Id }, bookDto);
        }

        [HttpPut("{bookid}")]
        public async Task<ActionResult> UpdateBookAsync([FromRoute]Guid authorid, [FromRoute]Guid bookid, BookForUpdateDto updatedBook)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorid, bookid);
            if (book == null)
            {
                return NotFound();
            }
            Mapper.Map(updatedBook, book, typeof(BookForUpdateDto), typeof(Book));
            RepositoryWrapper.Book.Update(book);
            if (!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("更新资源Book失败");
            }
            return NoContent();
        }

        [HttpPatch("{bookid}")]
        public async Task<ActionResult> PartiallyUpdateBookAsync(Guid authorid,Guid bookid, JsonPatchDocument<BookForUpdateDto> patchDocument)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorid, bookid);
            if (book == null)
            {
                return NotFound();
            }
            var bookUpdateDto = Mapper.Map<BookForUpdateDto>(book);
            patchDocument.ApplyTo(bookUpdateDto, p =>
             {
                 Logger.LogInformation(p.ErrorMessage);
             });
            Mapper.Map(bookUpdateDto, book, typeof(BookForUpdateDto), typeof(Book));
            RepositoryWrapper.Book.Update(book);
            if (!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("更新资源Book失败");
            }
            return NoContent();
        }
    }
}