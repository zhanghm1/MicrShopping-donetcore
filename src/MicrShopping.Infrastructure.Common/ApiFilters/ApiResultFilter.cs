using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Infrastructure.Common.ApiFilters
{
    public class ApiResultFilter : ResultFilterAttribute, IResultFilter
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("OnResultExecuted");
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("OnResultExecuting");
            var objectResult = context.Result as ObjectResult;
            context.Result = new OkObjectResult(new ResponseBase<object>()
            {
                Status = "OK",
                Message = "OK",
                Data = objectResult.Value
            });
        }
    }
}
