﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.API.Models
{
    //包括Author类除Id外的所有属性
    public class AuthorForCreationDto
    {
        [Required(ErrorMessage ="必须提供姓名")]
        [MaxLength(20,ErrorMessage ="姓名的最大长度为20个字符！")]
        public string Name { get; set; }
        public int Age { get; set; }

        [EmailAddress(ErrorMessage ="邮箱格式不正确")]
        public string Email { get; set; }

    }
}
