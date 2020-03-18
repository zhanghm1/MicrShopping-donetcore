using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MicrShopping.Infrastructure.Common.ApiFilters
{
    public class ApiResultFilter : ResultFilterAttribute, IResultFilter
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            //Console.WriteLine("OnResultExecuted");
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //Console.WriteLine("OnResultExecuting");
            //期望的返回值不需要携带ResponseBase
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult.Value is ResponseBase)
                {
                    //有返回ResponseBase则不处理信息
                }
                else if (objectResult.Value is HttpResponseMessage)
                {
                    
                }
                else 
                {
                    context.Result = new OkObjectResult(new ResponseBase<object>()
                    {
                        Status = "OK",
                        Message = "OK",
                        Data = objectResult.Value
                    });
                }
                
            }
            else 
            { 
            
            }
        }
    }
}
