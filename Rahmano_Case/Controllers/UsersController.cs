using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rahmano_Case.Models;
using System.Data.SQLite;
using System.IO;

namespace Rahmano_Case.Controllers
{
    public class UsersController : Controller
    {
        string path = "";
        string cs = @"URI=file:C:\dayinmitra\Rahmano_Case\Rahmano_Case\Data\SampleDayinMitra.db";
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;

        private DayinMitraContext _db;

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        //LOG IN
        public ActionResult Login()
        {
            UserLogin usr = new UserLogin();
            return View(usr);
        }

        public JsonResult checkLogin(UserLogin usr)
        {
            Pesan psn = new Pesan();
            if (ModelState.IsValid)
            {
                con = new SQLiteConnection(cs);
                string User_Name = "";

                string str = "Select * From Tbl_Users Where User_Login = @User_Login And User_Pwd = @User_Pwd And Is_Active = '1'";
                var cmd = new SQLiteCommand(str, con);
                cmd.Parameters.Add("User_Login", System.Data.DbType.String).Value = usr.User_Login;
                cmd.Parameters.Add("User_Pwd", System.Data.DbType.String).Value = usr.User_Pwd;

                con.Open();
                try
                {
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        User_Name = dr.GetString(1);
                        Session["User_ID"] = dr.GetInt16(0);
                    }
                }
                catch (Exception ex) { }

                con.Close();
                if (User_Name == "")
                {
                    psn.pesan_isi = "Wrong Name or Password";
                    return Json(psn);
                }
                else
                {
                    Session["User_Name"] = User_Name;
                    psn.pesan_id = 0;
                    return Json(psn);
                }
            }
            else
            {
                psn.pesan_isi = "Check Name or Password";
                return Json(psn);
            }
        }

        public JsonResult logout()
        {
            Session.Clear();
            return Json("Log Out");
        }

        //BIODATA
        public ActionResult Biodata()
        {
            Users usr = new Users();
            con = new SQLiteConnection(cs);

            string str = "Select * From Tbl_Users Where User_ID = @User_ID";
            var cmd = new SQLiteCommand(str, con);
            cmd.Parameters.Add("User_ID", System.Data.DbType.Int16).Value = Session["User_ID"];

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    usr.User_ID = dr.GetInt16(0);
                    usr.User_Name = dr.GetString(1);
                    usr.User_Sex = dr.GetString(2);
                    usr.User_BirthDate = Convert.ToDateTime(dr["User_BirthDate"].ToString());
                    //usr.User_BirthDate = DateTime.ParseExact(dr["User_BirthDate"].ToString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);
                    //usr.User_Photo = System.Text.Encoding.Default.GetBytes(dr.GetString(4));
                    usr.User_Photo = (byte[])(dr["User_Photo"]);
                    usr.User_Login = dr.GetString(5);
                    usr.User_Email = dr.GetString(6);
                    usr.User_Pwd = dr.GetString(7);
                    usr.Is_Active = dr.GetString(8);
                    usr.User_GUID = dr.GetString(9);
                    usr.Is_Deleted = dr.GetString(10);
                }
            }
            catch (Exception ex) { }
            con.Close();

            return View(usr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Biodata(Users usr, HttpPostedFileBase file)
        {
            Guid uid = Guid.NewGuid();
            Pesan psn = new Pesan();

            string str = str = "Update Tbl_Users " +
                    "Set User_Name = @User_Name, " +
                    "User_Sex = @User_Sex, " +
                    "User_BirthDate = @User_BirthDate, ";
            if (file != null) { 
                usr.User_Photo = simpanGambar(usr.User_ID.ToString(), file);
                str = str + "User_Photo = @User_Photo, ";
            }
            str = str + "User_Login = @User_Login, " +
                "User_Email = @User_Email, " +
                "User_GUID = @User_GUID " +
                "Where User_ID = @User_ID";


            con = new SQLiteConnection(cs);
            cmd = new SQLiteCommand(str, con);

            cmd.Parameters.Add("User_Name", System.Data.DbType.String).Value = usr.User_Name;
            cmd.Parameters.Add("User_Sex", System.Data.DbType.String).Value = usr.User_Sex;
            cmd.Parameters.Add("User_BirthDate", System.Data.DbType.String).Value = usr.User_BirthDate.ToString();
            if (file != null) { cmd.Parameters.Add("User_Photo", System.Data.DbType.Binary).Value = usr.User_Photo; }
            cmd.Parameters.Add("User_Login", System.Data.DbType.String).Value = usr.User_Login;
            cmd.Parameters.Add("User_Email", System.Data.DbType.String).Value = usr.User_Email;
            cmd.Parameters.Add("User_GUID", System.Data.DbType.String).Value = uid;
            cmd.Parameters.Add("User_ID", System.Data.DbType.Int16).Value = Session["User_ID"];

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    psn.pesan_id = 0;
                }
                Session["User_Name"] = usr.User_Name;
            }
            catch (Exception ex) { }
            con.Close();

            return Redirect("../");
        }
        private byte[] simpanGambar(string noreg, HttpPostedFileBase file)
        {
            Stream fs = file.InputStream;
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((Int32)fs.Length);

            return bytes; 
        }

        //CHANGE PASSOWRD
        public ActionResult changepass()
        {
            UserPassword usr = new UserPassword();
            return View(usr);
        }

        public JsonResult changepassword(UserPassword usr)
        {
            Pesan psn = new Pesan();
            if (ModelState.IsValid)
            {
                bool ada = false;
                Guid uid = Guid.NewGuid();
                string str = "Select * From Tbl_Users Where User_Pwd = @Old_Pwd And User_ID = @User_ID";
                con = new SQLiteConnection(cs);
                cmd = new SQLiteCommand(str, con);
                cmd.Parameters.Add("Old_Pwd", System.Data.DbType.String).Value = usr.Old_pwd;
                cmd.Parameters.Add("User_ID", System.Data.DbType.Int16).Value = Session["User_ID"];

                con.Open();
                try
                {
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ada = true;
                    }
                }
                catch (Exception ex) { }
                con.Close();

                if (ada)
                {
                    str = "Update Tbl_Users Set User_Pwd = @User_Pwd, User_GUID = @User_GUID Where User_ID = @User_ID";
                    con = new SQLiteConnection(cs);
                    cmd = new SQLiteCommand(str, con);
                    cmd.Parameters.Add("User_Pwd", System.Data.DbType.String).Value = usr.User_Pwd;
                    cmd.Parameters.Add("User_GUID", System.Data.DbType.String).Value = uid;
                    cmd.Parameters.Add("User_ID", System.Data.DbType.Int16).Value = Session["User_ID"];

                    con.Open();
                    try
                    {
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            ada = true;
                        }
                        psn.pesan_id = 0;
                        psn.pesan_isi = "Password has been change";
                    }
                    catch (Exception ex) { }
                    con.Close();
                }
                else
                {
                    psn.pesan_isi = "Password Wrong";
                }
            }
            else
            {
                psn.pesan_isi = "Check Name or Password";
            }
            return Json(psn);
        }

        //SIGN IN
        public ActionResult SignIn()
        {
            UserSignin usr = new UserSignin();
            ViewBag.msg = "";
            return View(usr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(UserSignin usr)
        {
            Pesan psn = new Pesan();
            if (ModelState.IsValid)
            {
                con = new SQLiteConnection(cs);

                string str = "Select User_Login, User_Email From Tbl_Users Where User_Login = @User_Login or User_Email = @User_Email And Is_Active = '1'";
                var cmd = new SQLiteCommand(str, con);
                cmd.Parameters.Add("User_Login", System.Data.DbType.String).Value = usr.User_Login;
                cmd.Parameters.Add("User_Email", System.Data.DbType.String).Value = usr.User_Email;
                
                con.Open();
                try
                {
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (usr.User_Login == dr.GetString(1)) {
                            psn.pesan_isi = "Login Name telah terdata";
                        }
                        else
                        {
                            psn.pesan_isi = "Email telah terdata";
                        }
                        con.Close();
                    }
                }
                catch (Exception ex) { }
                con.Close();
                if(psn.pesan_isi != "") { return View(psn); }

                Guid uid = Guid.NewGuid();
                str = "Insert into Tbl_Users(User_Name, User_Login, User_Email, User_Pwd, User_GUID)" +
                    "Values(@User_Name, @User_Login, @User_Email, @User_Pwd, @User_GUID)";
                cmd = new SQLiteCommand(str, con);
                cmd.Parameters.Add("User_Name", System.Data.DbType.String).Value = usr.User_Name;
                cmd.Parameters.Add("User_Login", System.Data.DbType.String).Value = usr.User_Login;
                cmd.Parameters.Add("User_Email", System.Data.DbType.String).Value = usr.User_Email;
                cmd.Parameters.Add("User_Pwd", System.Data.DbType.String).Value = usr.User_Pwd;
                cmd.Parameters.Add("User_GUID", System.Data.DbType.String).Value = uid;

                con.Open();
                try
                {
                    cmd.ExecuteReader();
                }
                catch (Exception ex) { }
                con.Close();
                psn.pesan_id = 0;
                psn.pesan_isi = "User already registerd \n\r Please open email to activation";
            }
            return View(psn);
        }
    }
}