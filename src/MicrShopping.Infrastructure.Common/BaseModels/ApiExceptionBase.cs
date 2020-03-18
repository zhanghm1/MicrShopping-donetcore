using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Infrastructure.Common
{
    public class ApiExceptionBase: Exception
    {
        public ApiExceptionBase(string status, string Message):base(Message)
        {
            Status = status;
        }
        public string Status { get; set; }
    }
}
