using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Entities
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int Page { get; set; }

        //引用导航属性
        [ForeignKey("{Authorid}")]//指明属性的外键
        public Author Author { get; set; }

        public Guid AuthorId { get; set; }
    }
}
