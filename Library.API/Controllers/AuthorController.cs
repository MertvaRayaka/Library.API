using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Servicers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        //public IAuthorRepository AuthorRepository { get; }
        //public AuthorController(IAuthorRepository authorRepository)
        //{
        //    AuthorRepository = authorRepository;
        //}

        //[HttpGet]
        //public ActionResult<List<AuthorDto>> GetAuthors()
        //{
        //    return AuthorRepository.GetAuthors().ToList();
        //}

        //[HttpGet("{authorId}",Name =nameof(GetAuthor))]
        //public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        //{
        //    var author = AuthorRepository.GetAuthor(authorId);
        //    if (author == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return author;
        //    }
        //}

        //[HttpPost]
        //public IActionResult CreateAuthor(AuthorForCreationDto authorForCreation)//模型绑定传递
        //{
        //    var authorDto = new AuthorDto
        //    {
        //        Name = authorForCreation.Name,
        //        Age=authorForCreation.Age,
        //        Email=authorForCreation.Email
        //    };            AuthorRepository.AddAuthor(authorDto);
        //    return CreatedAtRoute(nameof(GetAuthor),new {authorId=authorDto.Id },authorDto);
        //}

        //[HttpDelete("{authorid}")]
        //public IActionResult DeleteAuthor(Guid authorid)
        //{
        //    if (!AuthorRepository.IsAuthorExists(authorid))
        //    {
        //        return NotFound();
        //    }
        //    var author = AuthorRepository.GetAuthor(authorid);
        //    if (author == null)
        //    {
        //        return NotFound();
        //    }
        //    AuthorRepository.DeleteAuthor(author);
        //    return NoContent();
        //}

        public IRepositoryWrapper RepositoryWrapper { get; }
        public IMapper Mapper { get; }

        public AuthorController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAuthorsAsync))]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthorsAsync([FromQuery]AuthorResourceParameters parameters)
        {
            //分页用PagedList<T>类来操作
            var pagedList = await RepositoryWrapper.Author.GetAllAsync(parameters);
            var pageinationMetadata = new
            {
                totalCount = pagedList.TotalCount,
                pageSize = pagedList.PageSize,
                currentPage = pagedList.CurrentPage,
                totalPages = pagedList.TotalPages,
                previousPageLink = pagedList.HasPrevious ? Url.Link(nameof(GetAuthorsAsync), new
                {
                    pageNumber = pagedList.CurrentPage - 1,
                    pageSize = pagedList.PageSize,
                    birthPlace = parameters.BirthPlace,
                    searchQuery = parameters.SearchQuery,
                    sortBy = parameters.SortBy,
                }) : null,
                nextPageLink = pagedList.HasNext ? Url.Link(nameof(GetAuthorsAsync), new
                {
                    pageNumber = pagedList.CurrentPage + 1,
                    pageSize = pagedList.PageSize,
                    birthPlace = parameters.BirthPlace,
                    searchQuery = parameters.SearchQuery,
                    sortBy = parameters.SortBy,
                }) : null,
            };
            Response.Headers.Add("X-Pagination",JsonConvert.SerializeObject(pageinationMetadata));
            var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(pagedList);
            return authorDtoList.ToList();


            //分页写在Controller中
            //var authors = await RepositoryWrapper.Author.GetAllAsync();

            //var PartAuthors = authors.Skip(parameters.PageSize * (parameters.PageNumber - 1)).Take(parameters.PageSize);

            //var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(PartAuthors);

            //return authorDtoList.ToList();
            //未分页
            //var authors = (await RepositoryWrapper.Author.GetAllAsync()).OrderBy(author => author.Name);

            //var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(authors);//RepositoryBase类中的延迟方法（Task.FromResult(...)）会到运行到“使用AutoMapper进行对象映射”才实际去执行查询

            //return authorDtoList.ToList();
        }

        [HttpGet("{authorid}", Name = nameof(GetAuthorAsync))]
        public async Task<ActionResult<AuthorDto>> GetAuthorAsync(Guid authorId)
        {
            var authors = await RepositoryWrapper.Author.GetByConditionAsync(p => p.Id == authorId);

            var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(authors);

            return authorDtoList.FirstOrDefault();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthorAsync(AuthorForCreationDto authorForCreationDto)
        {
            var author = Mapper.Map<Author>(authorForCreationDto);

            RepositoryWrapper.Author.Create(author);

            var result = await RepositoryWrapper.Author.SaveAsync();

            if (!result)
            {
                throw new Exception("创建资源author失败");
            }

            var authorCreated = Mapper.Map<AuthorDto>(author);

            return CreatedAtRoute(nameof(GetAuthorAsync), new { authorid = authorCreated.Id }, authorCreated);
        }

        [HttpDelete("{authorid}")]
        public async Task<ActionResult> DeleteAuthorAsync(Guid authorid)
        {
            var author = await RepositoryWrapper.Author.GetByIdAsync(authorid);

            if (author == null)
            {
                return NotFound();
            }

            RepositoryWrapper.Author.Delete(author);

            var result = await RepositoryWrapper.Author.SaveAsync();
            if (!result)
            {
                throw new Exception("删除资源author失败");
            }

            return NoContent();
        }
    }
}