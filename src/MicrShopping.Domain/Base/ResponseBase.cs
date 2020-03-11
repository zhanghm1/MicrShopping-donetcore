using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Domain.Base
{
    public class ResponseBase
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class ResponseBase<T>: ResponseBase
    {
        public T Data { get; set; }
    }
    /// <summary>
    /// 分页使用的返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponsePageBase<T> : ResponseBase
    {
        public PageBase<T> Data { get; set; }
    }
    public class PageBase<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Total { get; set; }
        public int PageTotal { get; set; }
        public IEnumerable<T> List { get; set; }

    }

}
