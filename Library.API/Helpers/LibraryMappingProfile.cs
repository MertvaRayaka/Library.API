using AutoMapper;
using Library.API.Entities;
using Library.API.Models;
using System;

namespace Library.API.Helpers
{
    public class LibraryMappingProfile : Profile
    {
        public LibraryMappingProfile()
        {
            CreateMap<Author, AuthorDto>().ForMember(dest => dest.Age, config => config.MapFrom(src => DateTimeOffset.Now.Year - src.BirthDate.Year));

            CreateMap<Book, BookDto>().ForMember(dest => dest.Pages, config => config.MapFrom(src => src.Page));

            CreateMap<AuthorForCreationDto, Author>()
                .ForMember(dest => dest.BirthDate, config => config.MapFrom(src => "1994-11-14"))
                .ForMember(dest => dest.BirthPlace, config => config.MapFrom(src => "江苏·如东"))
                .ForMember(dest => dest.Id, config => config.MapFrom(src => Guid.NewGuid()));

            CreateMap<BookForCreationDto, Book>();

            CreateMap<BookForUpdateDto, Book>();
        }
    }
}
