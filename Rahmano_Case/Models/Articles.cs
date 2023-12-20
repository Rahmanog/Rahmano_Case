using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rahmano_Case.Models
{
    public class Articles
    {
        public int Article_ID { get; set; }
        public int User_id { get; set; }
        public string User_Name { get; set; }
        public string Article_Create { get; set; }
        public string Article_Title { get; set; }
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
        public byte[] Article_Photo { get; set; }
        public string Article_Desc { get; set; }
        [AllowHtml]
        public string Article_Text { get; set; }
        public string Article_GUID { get; set; }
        public string Is_Publish { get; set; }
        public string Article_Publish { get; set; }
        public string Is_Deleted { get; set; }

        public Articles()
        {
            Article_ID = 0;
            Article_Create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Category_ID = 0;
            Is_Publish = "No";
            Is_Deleted = "0";
        }
    }

    public class Article
    {
        public int Article_ID { get; set; }
        public int User_id { get; set; }
        public string User_Name { get; set; }
        public string Article_Title { get; set; }
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
        public byte[] Article_Photo { get; set; }
        public string Article_Desc { get; set; }
        public string Article_Text { get; set; }
        public DateTime Article_Publish { get; set; }
    }
}