using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Infrastructure.Common.ApiResults
{
    public class ApiExceptionResult : ObjectResult
    {
        public ApiExceptionResult(int code,string status, string message)
            : base(new ApiExceptionBase(status,message))
        {
            StatusCode = code;
        }
    }

}
