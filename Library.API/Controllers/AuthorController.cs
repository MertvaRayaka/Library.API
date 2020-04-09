using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.API.Servicers;
using Library.API.Models;

namespace Library.API.Controllers
{
    //测试推送功能
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {      
        public IAuthorRepository AuthorRepository { get; }
        public AuthorController(IAuthorRepository authorRepository)
        {
            AuthorRepository = authorRepository;
        }

        [HttpGet]
        public ActionResult<List<AuthorDto>> GetAuthors()
        {
            return AuthorRepository.GetAuthors().ToList();
        }

        [HttpGet("{authorId}",Name =nameof(GetAuthor))]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var author = AuthorRepository.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }
            else
            {
                return author;
            }
        }

        [HttpPost]
        public IActionResult CreateAuthor(AuthorForCreationDto authorForCreation)//模型绑定传递
        {
            var authorDto = new AuthorDto
            {
                Name = authorForCreation.Name,
                Age=authorForCreation.Age,
                Email=authorForCreation.Email
            };            AuthorRepository.AddAuthor(authorDto);
            return CreatedAtRoute(nameof(GetAuthor),new {authorId=authorDto.Id },authorDto);
        }

        [HttpDelete("{authorid}")]
        public IActionResult DeleteAuthor(Guid authorid)
        {
            if (!AuthorRepository.IsAuthorExists(authorid))
            {
                return NotFound();
            }
            var author = AuthorRepository.GetAuthor(authorid);
            if (author == null)
            {
                return NotFound();
            }
            AuthorRepository.DeleteAuthor(author);
            return NoContent();
        }
    }
}