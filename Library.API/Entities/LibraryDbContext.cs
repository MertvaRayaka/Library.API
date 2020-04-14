using Library.API.Servicers;
using Microsoft.EntityFrameworkCore;
using System;

namespace Library.API.Entities
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.SeedData();

            //设置主键
            modelBuilder.Entity<Author>().HasKey(p => p.Id);
            modelBuilder.Entity<Book>().HasKey(p => p.Id);
            // 映射实体关系，一对多
            modelBuilder.Entity<Author>().HasMany(p => p.Books);
        }
    }

    public static class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder, IRepositoryWrapper repositoryWrapper, Guid authorid)
        {
            //modelBuilder.Entity<Author>().HasData(new Author
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Neil",
            //    BirthDate = new DateTimeOffset(new DateTime(1993, 12, 19)),
            //    BirthPlace = "江苏·如东",
            //    Email = "author@xxx.com"
            //});
            modelBuilder.Entity<Book>().HasData(new Book
            {
                Id = Guid.NewGuid(),
                AuthorId = authorid,
                Description = "Neil's Book",
                Page = 888,
                Title = "真香系列"
            });
        }
    }
}
