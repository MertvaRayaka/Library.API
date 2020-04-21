﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    public class AuthorResourceParameters
    {
        public const int MaxPageSize = 50;
        private int pageSize = 10;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            { 
                return pageSize;
            }
            set 
            {
                pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }

    }
}
