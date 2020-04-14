using Library.API.Servicers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Filters
{
    public class CheckAuthorExistFilterAttribute:ActionFilterAttribute
    {
        public IRepositoryWrapper RepositoryWrapper { get;}
        public CheckAuthorExistFilterAttribute(IRepositoryWrapper repositoryWrapper)
        {
            RepositoryWrapper = repositoryWrapper;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context,ActionExecutionDelegate next)
        {
            //context.ActionArguments获取传入Action的参数
            var authorIdParameter = context.ActionArguments.Single(m=>m.Key =="authorid");
            Guid authorid = (Guid)authorIdParameter.Value;
            var isExist = await RepositoryWrapper.Author.IsExistAsync(authorid);
            if (!isExist)
            {
                context.Result = new NotFoundResult();
            }

            await base.OnActionExecutionAsync(context,next);
        }
    }
}
