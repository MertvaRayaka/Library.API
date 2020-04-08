using Library.API.Models;
using Library.API.Servicers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public ILogger<BookController> Logger { get; }
        public IAuthorRepository AuthorRepository { get; }
        public IBookRespository BookRespository { get; }
        public BookController(IAuthorRepository authorRepository, IBookRespository bookRespository, ILogger<BookController> logger)
        {
            AuthorRepository = authorRepository;
            BookRespository = bookRespository;
            Logger = logger;
        }

        [HttpGet]
        public ActionResult<List<BookDto>> GetBooks(Guid authorId)
        {
            Logger.LogInformation(1, "执行一次返回指定作者所有图书");
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }
            return BookRespository.GetBooksForAuthor(authorId).ToList();
        }

        [HttpGet("{bookId}", Name = nameof(GetSpecifyBook))]
        public ActionResult<BookDto> GetSpecifyBook(Guid authorid, Guid bookid)
        {
            Logger.LogInformation(2, "执行一次返回指定作者的指定图书");
            if (!AuthorRepository.IsAuthorExists(authorid))
            {
                return NotFound();
            }

            var target = BookRespository.GetBookForAuthor(authorid, bookid);

            if (target == null)
            {
                return NotFound();
            }
            return BookRespository.GetBookForAuthor(authorid, bookid);
        }

        [HttpPost]
        //authorid从BookController定义的路由中获取(URL的一部分字符串)
        //bookForCreation从解析的消息正文(POST上来的Body)中获取
        public IActionResult CreateBookDto(Guid authorid, BookForCreationDto bookForCreation)
        {
            if (!AuthorRepository.IsAuthorExists(authorid))
            {
                return NotFound();
            }

            var BookDto = new BookDto
            {
                Id = Guid.NewGuid(),
                Title = bookForCreation.Title,
                Pages = bookForCreation.Pages,
                Description = bookForCreation.Description,
                AuthorId = authorid
            };

            BookRespository.AddBook(BookDto);
            return CreatedAtRoute(nameof(GetSpecifyBook), new { authorId = authorid, bookID = BookDto.Id }, BookDto);
        }

        [HttpDelete("{bookid}")]
        public IActionResult DeleteBookDto(Guid authorid, Guid bookid)
        {
            if (!AuthorRepository.IsAuthorExists(authorid))
            {
                return NotFound();
            }
            var book = BookRespository.GetBookForAuthor(authorid, bookid);
            if (book == null)
            {
                return NotFound();
            }
            BookRespository.DeleteBook(book);
            return NoContent();
        }

        [HttpPut("{bookid}")]
        //updatedbook从Body中获取，数据模型绑定
        //HttpPut：对资源的每个属性都更新，若属性不赋值，则为null或0
        public ActionResult<BookDto> UpdateBookDto(Guid authorid, Guid bookid, BookForUpdateDto updatedbook)
        {
            if (!AuthorRepository.IsAuthorExists(authorid))
            {
                return NotFound();
            }
            var book = BookRespository.GetBookForAuthor(authorid, bookid);
            if (book == null)
            {
                return NotFound();
            }
            BookRespository.UpdateBook(authorid, bookid, updatedbook);
            return BookRespository.GetBookForAuthor(authorid, bookid);
        }

        [HttpPatch("{bookid}")]
        public ActionResult<BookDto> PartiallyUpdateBookDto(Guid authorid, Guid bookid, [FromBody]JsonPatchDocument<BookForUpdateDto> patchDocument)
        {
            if (!AuthorRepository.IsAuthorExists(authorid))
            {
                return NotFound();
            }
            var book = BookRespository.GetBookForAuthor(authorid, bookid);
            if (book == null)
            {
                return NotFound();
            }

            var bookToPatch = new BookForUpdateDto
            {
                Title = book.Title,
                Description = book.Description,
                Pages = book.Pages
            };

            string Error = string.Empty;
            patchDocument.ApplyTo(bookToPatch, p =>
             {
                 Error = p.ErrorMessage;
                 Logger.LogInformation(p.ErrorMessage);
             });
            if (!String.IsNullOrEmpty(Error))
            {
                return BadRequest(Error);
            }

            BookRespository.UpdateBook(authorid, bookid, bookToPatch);
            return BookRespository.GetBookForAuthor(authorid, bookid);
        }
    }
}