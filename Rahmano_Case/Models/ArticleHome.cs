using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rahmano_Case.Models
{
    public class ArticleHome
    {
        public IEnumerable<Article> Article { get; set; }
        public IEnumerable<Categories> Categories { get; set; }
    }
}