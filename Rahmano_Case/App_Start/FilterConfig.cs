﻿using System.Web;
using System.Web.Mvc;

namespace Rahmano_Case
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
