using Microsoft.AspNetCore.Mvc.Filters;
using MicrShopping.Infrastructure.Common.ApiResults;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MicrShopping.Infrastructure.Common.ApiFilters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string Status = "Fail";
            //处理各种异常

            context.ExceptionHandled = true;
            context.Result = new ApiExceptionResult((int)status, Status, context.Exception.Message);
        }
    }
}
