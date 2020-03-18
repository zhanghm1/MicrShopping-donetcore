using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Infrastructure.Common
{
    public class RequestPageBase
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
