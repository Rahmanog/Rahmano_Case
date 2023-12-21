using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rahmano_Case.Models;

namespace Rahmano_Case.Controllers
{
    public class ArticlesController : Controller
    {
        string path = "";
        string cs = @"URI=file:" + AppContext.BaseDirectory + ConfigurationManager.AppSettings["DB_File"].ToString();
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;

        // GET: Articles
        public ActionResult Index()
        {
            ArticleHome artH = new ArticleHome();
            con = new SQLiteConnection(cs);

            string str = "Select * From Tbl_Category Where Is_Deleted = '0'";
            var cmd = new SQLiteCommand(str, con);
            List<Categories> lcat = new List<Categories>();
            Categories cat;

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cat = new Categories();
                    cat.Category_ID = dr.GetInt16(0);
                    cat.Category_Name = dr.GetString(1);
                    cat.Category_Desc = dr.GetString(2);
                    lcat.Add(cat);
                }
            }
            catch (Exception ex)
            {
                
            }
            con.Close();
            artH.Categories = lcat;

            str = "Select Article_ID, A.User_id, U.User_name, Article_Title, A.Category_ID, C.Category_Name, A.Article_Photo, A.Article_Desc, A.Article_Publish " +
                "From Tbl_Articles A " +
                "Left join Tbl_Users U on A.User_ID = U.User_ID " +
                "Left Join Tbl_Category C on A.Category_ID = C.Category_ID " +
                "Where A.Is_Deleted = '0' And Is_Publish = '1' " +
                "Order by Article_Publish";
            cmd = new SQLiteCommand(str, con);
            List<Article> lart = new List<Article>();
            Article art;

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    art = new Article();
                    art.Article_ID = dr.GetInt16(0);
                    art.User_id = dr.GetInt16(1);
                    art.User_Name = dr.GetString(2);
                    art.Article_Title = dr.GetString(3);
                    art.Category_ID = dr.GetInt16(4);
                    art.Category_Name = dr.GetString(5);
                    if (dr["Article_Photo"].ToString() != "") { art.Article_Photo = (byte[])(dr["Article_Photo"]); }
                    art.Article_Desc = dr.GetString(7);
                    art.Article_Publish = Convert.ToDateTime(dr["Article_Publish"].ToString());
                    lart.Add(art);
                }
            }
            catch (Exception ex)
            {

            }

            con.Close();
            artH.Article = lart;
            return View(artH);
        }
        public ActionResult List()
        {
            List<Article> lart = new List<Article>();
            Article art;
            Int16 id = 0;
            con = new SQLiteConnection(cs);

            string str = "Select Article_ID, A.User_id, U.User_name, Article_Title, " +
                "A.Category_ID, C.Category_Name, A.Article_Photo, A.Article_Desc, " +
                "A.Article_Text, A.Article_Publish " +
                "From Tbl_Articles A " +
                "Left join Tbl_Users U on A.User_ID = U.User_ID " +
                "Left Join Tbl_Category C on A.Category_ID = C.Category_ID " +
                "Where A.User_ID = @User_ID And A.Is_Deleted = '0' " +
                "Order by Article_Publish";
            var cmd = new SQLiteCommand(str, con);
            if (Session["User_ID"] != null)
            {
                id = (Int16)Session["User_id"];
            }

            cmd.Parameters.Add("User_ID", System.Data.DbType.Int16).Value = id;

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    art = new Article();
                    art.Article_ID = dr.GetInt16(0);
                    art.User_id = dr.GetInt16(1);
                    art.User_Name = dr.GetString(2);
                    art.Article_Title = dr.GetString(3);
                    art.Category_ID = dr.GetInt16(4);
                    art.Category_Name = dr.GetString(5);
                    if (dr["Article_Photo"].ToString() != "") { art.Article_Photo = (byte[])(dr["Article_Photo"]); }
                    art.Article_Desc = dr.GetString(7);
                    art.Article_Text = dr.GetString(8);
                    if(dr["Article_Publish"].ToString() == "")
                    {
                        art.Article_Publish = Convert.ToDateTime("1/1/1");
                    }
                    else
                    {
                        art.Article_Publish = Convert.ToDateTime(dr["Article_Publish"].ToString());
                    }

                    lart.Add(art);
                }
            }
            catch (Exception ex) { }
            con.Close();
            return View(lart);
        }
        public ActionResult Create(int id)
        {
            Articles art = new Articles();
            if (id == 0) {
                art.User_id = (Int16)Session["User_ID"];
            }
            else
            {                 
                con = new SQLiteConnection(cs);

                string str = "Select * From Tbl_Articles Where Article_ID = @Article_ID ";
                var cmd = new SQLiteCommand(str, con);

                cmd.Parameters.Add("Article_ID", System.Data.DbType.Int16).Value = id;

                con.Open();
                try
                {
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        art.Article_ID = dr.GetInt16(0);
                        art.User_id = dr.GetInt16(1);
                        art.Article_Create = dr["Article_Create"].ToString();
                        art.Article_Title = dr.GetString(3);
                        art.Category_ID = dr.GetInt16(4);
                        if (dr["Article_Photo"].ToString() != "") { art.Article_Photo = (byte[])(dr["Article_Photo"]); }
                        art.Article_Desc = dr.GetString(6);
                        art.Article_Text = dr.GetString(7);
                        art.Article_Publish = dr["Article_Publish"].ToString();
                        if (dr.GetString(9).ToString() == "1") { art.Is_Publish = "Yes"; } else { art.Is_Publish = "No"; }
                    }
                }
                catch (Exception ex ){ }

                con.Close();
            }
            Listing lst = new Listing();
            ViewBag.Category = new SelectList(lst.CategoryList(), "ls_id", "ls_val", art.Category_ID);
            return View(art);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Articles usr, HttpPostedFileBase file)
        {
            Guid uid = Guid.NewGuid();
            Pesan psn = new Pesan();
            if (usr.Is_Publish == "Yes") { usr.Is_Publish = "1"; } else { usr.Is_Publish = "0"; }
            usr.Is_Deleted = "0";
            string str = "";
            if (usr.Article_ID == 0)
            {
                str = "INSERT INTO Tbl_Articles(User_ID, Article_Create, Article_Title, Category_ID, Article_Photo, Article_Desc, Article_Text, Article_Publish, Is_Publish, Article_GUID) " +
                        "VALUES(@User_ID, @Article_Create, @Article_Title, @Category_ID, @Article_Photo, @Article_Desc, @Article_Text, @Article_Publish, @Is_Publish, @Article_GUID)";
                if (file != null)
                {
                    usr.Article_Photo = simpanGambar(file);
                    str = str + "Article_Photo = @Article_Photo, ";
                }
                else { usr.Article_Photo = null; }
            }
            else
            {
                str = "UPDATE Tbl_Articles " +
                    "Set User_ID = @User_ID, " +
                    "Article_Create = @Article_Create, " +
                    "Article_Title = @Article_Title,  " +
                    "Category_ID = @Category_ID,  ";
                if (file != null)
                {
                    usr.Article_Photo = simpanGambar(file);
                    str = str + "Article_Photo = @Article_Photo, ";
                }
                str = str + "Article_Desc = @Article_Desc, " +
                    "Article_Text =@Article_Text, " +
                    "Article_Publish = @Article_Publish, " +
                    "Is_Publish = @Is_Publish, " +
                    "Article_GUID = @Article_GUID " +
                    "WHERE Article_ID = @Article_ID ";
            }
            if(usr.Article_Publish== null) { usr.Article_Publish = ""; }

            con = new SQLiteConnection(cs);
            cmd = new SQLiteCommand(str, con);
            //if(usr.Is_Publish == "01/01/2001 00:00:00") { usr.Is_Publish = ""; }
            cmd.Parameters.Add("User_ID", System.Data.DbType.Int16).Value = usr.User_id;
            cmd.Parameters.Add("Article_Create", System.Data.DbType.String).Value = usr.Article_Create.ToString();
            cmd.Parameters.Add("Article_Title", System.Data.DbType.String).Value = usr.Article_Title;
            cmd.Parameters.Add("Category_ID", System.Data.DbType.Int16).Value = usr.Category_ID;
            if (file != null || usr.Article_ID == 0) { cmd.Parameters.Add("Article_Photo", System.Data.DbType.Binary).Value = usr.Article_Photo; }
            cmd.Parameters.Add("Article_Desc", System.Data.DbType.String).Value = usr.Article_Desc;
            cmd.Parameters.Add("Article_Text", System.Data.DbType.String).Value = usr.Article_Text;
            cmd.Parameters.Add("Article_Publish", System.Data.DbType.String).Value = usr.Article_Publish.ToString();
            cmd.Parameters.Add("Is_Publish", System.Data.DbType.Int16).Value = usr.Is_Publish;
            cmd.Parameters.Add("Article_GUID", System.Data.DbType.String).Value = uid;
            if (usr.Article_ID != 0) { cmd.Parameters.Add("Article_ID", System.Data.DbType.Int16).Value = usr.Article_ID; }

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    psn.pesan_id = 0;
                }
            }
            catch (Exception ex) { }
            con.Close();

            //Session["User_Name"] = usr.User_Name;
            return Redirect("../");
        }
        private byte[] simpanGambar(HttpPostedFileBase file)
        {
            Stream fs = file.InputStream;
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((Int32)fs.Length);
            return bytes; 
        }

        public JsonResult publish(int id)
        {
            Pesan psn = new Pesan();
            con = new SQLiteConnection(cs);

            string str = "Update Tbl_Articles set Is_Publish = '1', Article_Publish = '" + DateTime.Now.ToString("dd/mm/yyyy hh:MM:ss") + "' Where Article_ID = @Article_ID";
            var cmd = new SQLiteCommand(str, con);
            cmd.Parameters.Add("Article_ID", System.Data.DbType.String).Value = id;

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                psn.pesan_isi = "Article has been Publishing";
            }
            catch (Exception ex)
            {
                psn.pesan_isi = ex.Message;
            }
            con.Close();

            return Json(psn);
        }

        public JsonResult Delete(int id = 0)
        {
            Pesan psn = new Pesan();
            con = new SQLiteConnection(cs);

            string str = "Update Tbl_Articles set Is_Deleted = '1' Where Article_ID = @Article_ID";
            var cmd = new SQLiteCommand(str, con);
            cmd.Parameters.Add("Article_ID", System.Data.DbType.String).Value = id;

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                psn.pesan_isi = "Article has been Deleted";
            }
            catch (Exception ex)
            {
                psn.pesan_isi = ex.Message;
            }
            con.Close();

            return Json(psn);
        }

        public ActionResult Update()
        {
            return View();
        }
        public ActionResult Details(int id =0)
        {
            if (id == 0)
            {
                return Redirect("/");
            }

            Article art = new Article();
            con = new SQLiteConnection(cs);

            string str = "Select Article_ID, A.User_id, U.User_name, Article_Title, " +
                "A.Category_ID, C.Category_Name, A.Article_Photo, A.Article_Desc, " +
                "A.Article_Text, A.Article_Publish " +
                "From Tbl_Articles A " +
                "Left join Tbl_Users U on A.User_ID = U.User_ID " +
                "Left Join Tbl_Category C on A.Category_ID = C.Category_ID " +
                "Where A.Article_ID = @Article_ID " +
                "Order by Article_Publish";
            var cmd = new SQLiteCommand(str, con);
            cmd.Parameters.Add("Article_ID", System.Data.DbType.Int16).Value = id;

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    art.Article_ID = dr.GetInt16(0);
                    art.User_id = dr.GetInt16(1);
                    art.User_Name = dr.GetString(2);
                    art.Article_Title = dr.GetString(3);
                    art.Category_ID = dr.GetInt16(4);
                    art.Category_Name = dr.GetString(5);
                    if (dr["Article_Photo"].ToString() != "") { art.Article_Photo = (byte[])(dr["Article_Photo"]); }
                    art.Article_Desc = dr.GetString(7);
                    art.Article_Text = dr.GetString(8);
                    art.Article_Publish = Convert.ToDateTime(dr["Article_Publish"].ToString());
                }
            }
            catch (Exception ex) { }

            con.Close();
            ViewBag.Name = Session["User_Name"];
            ViewBag.ArtID = id;

            return View(art);
        }

        public JsonResult loadMessage(int id)
        {
            Pesan psn = new Pesan();
            List<comments> lcom = new List<comments>();
            comments cmt;

            string str = "Select Article_id, C.User_ID, Comment_Cretae, User_Name, Comment " +
                "From Tbl_Article_Comment C " +
                "Left Join Tbl_Users U on C.User_ID = U.User_ID " +
                "Where Article_ID = @Article_ID ANd C.Is_Deleted = '0' " +
                "Order by Comment_Cretae";
            con = new SQLiteConnection(cs);
            cmd = new SQLiteCommand(str, con);
            cmd.Parameters.Add("Article_ID", System.Data.DbType.Int16).Value = id;

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cmt = new comments();
                    cmt.Article_ID = id;
                    cmt.User_ID = dr.GetInt16(1);
                    cmt.Comment_Create = dr.GetString(2);
                    cmt.User_Name = dr.GetString(3);
                    cmt.comment = dr.GetString(4);
                    lcom.Add(cmt);
                }
            }
            catch (Exception ex)
            {
                psn.pesan_isi = ex.Message;
            }
            con.Close();

            return Json(lcom);
        }
        public JsonResult saveMessage(int id, string wkt, string message)
        {
            Pesan psn = new Pesan();
            string str = "Insert Into Tbl_Article_Comment(Article_ID, User_ID, Comment_Cretae, Comment) " +
                "Values(@Article_ID, @User_ID, @Comment_Cretae, @Comment)";
            con = new SQLiteConnection(cs);
            cmd = new SQLiteCommand(str, con);
            cmd.Parameters.Add("Article_ID", System.Data.DbType.Int16).Value = id;
            cmd.Parameters.Add("User_ID", System.Data.DbType.Int16).Value = Session["User_ID"];
            cmd.Parameters.Add("Comment_Cretae", System.Data.DbType.String).Value = wkt;
            cmd.Parameters.Add("Comment", System.Data.DbType.String).Value = message;

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                psn.pesan_isi = ex.Message;
            }
            con.Close();

            return Json(psn);
        }
    }
}