using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Infrastructure.Common
{
    public class ResponseBase
    {
        public string Status { get; set; } = "OK";
        public string Message { get; set; } = "OK";
    }
    /// <summary>
    /// 带参数的返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseBase<T>: ResponseBase
    {
        public T Data { get; set; }
    }
    /// <summary>
    /// 分页使用的返回参数
    /// </summary>
    public class PageBase<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int CountTotal { get; set; }
        public int PageTotal { get; set; }
        public IEnumerable<T> List { get; set; }

    }

}
