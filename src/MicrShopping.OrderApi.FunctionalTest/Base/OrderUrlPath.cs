using System;
using System.Collections.Generic;
using System.Text;
using MicrShopping.Infrastructure.Common;

namespace MicrShopping.OrderApi.FunctionalTest.Base
{
    public class OrderUrlPath
    {
        public static string Create()
        {
            return "Order/Create";
        }

        public static string List(RequestPageBase request)
        {
            return PageHelper.GetUrl("Order/List", request);
        }
    }

    public class PageHelper
    {
        public static string GetUrl<T>(string url, T t)
        {
            var type = typeof(T);
            foreach (var item in type.GetProperties())
            {
                if (url.IndexOf("?") == -1)
                {
                    url = url + "?";
                }
                else
                {
                    url = url + "&";
                }
                url = url + item.Name + "=" + item.GetValue(t);
            }

            return url;
        }
    }
}