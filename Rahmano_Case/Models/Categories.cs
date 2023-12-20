using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rahmano_Case.Models
{
    public class Categories
    {
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
        public string Category_Desc { get; set; }
        public string Is_Deleted { get; set; }
    }

}