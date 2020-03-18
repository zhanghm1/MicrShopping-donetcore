using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            context.ExceptionHandled = true;
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string Status = "SERVER_ERROR";
            string Message = "未知错误";
            ResponseBase resp = new ResponseBase();
            if (context.Exception is ApiExceptionBase)
            {
                ApiExceptionBase apiException = context.Exception as ApiExceptionBase;

                resp.Status = apiException.Status;
                resp.Message = apiException.Message;

            }
            else 
            {
                resp.Status = Status;
                resp.Message = Message;
            }


            context.Result = new JsonResult(resp) {StatusCode= (int)status };
        }
    }
}
