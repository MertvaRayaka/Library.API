using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    public class PagedList<T>:List<T>
    {
        /// <summary>
        /// PagedList构造器
        /// </summary>
        /// <param name="items">List<T>列表，该列表中的元素会被添加到当前的PagedList<T>实例中</param>
        /// <param name="totalCount">总数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页记录数</param>
        public PagedList(List<T> items,int totalCount,int pageNumber,int pageSize)
        {
            TotalCount = totalCount;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount/PageSize);
            AddRange(items);
        }

        public int CurrentPage { get; private set; }
        /// <summary>
        /// 总页数=总数/每页记录数
        /// </summary>
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,int pageNumber,int pageSize)
        {
            var totalCount = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var list = new PagedList<T>(items,totalCount,pageNumber,pageSize);
            return await Task.FromResult(list);
        }
    }
}
