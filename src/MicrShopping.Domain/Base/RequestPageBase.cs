using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Domain.Base
{
    public class RequestPageBase
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
