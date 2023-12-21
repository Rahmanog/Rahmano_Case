using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace Rahmano_Case.Models
{
    public class Listing
    {
        string path = "";
        string cs = @"URI=file:" + AppContext.BaseDirectory + ConfigurationManager.AppSettings["DB_File"].ToString();
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;

        public List<isiList> CategoryList()
        {
            List<isiList> lst = new List<isiList>();
            isiList dat;
            con = new SQLiteConnection(cs);

            string str = "Select * From Tbl_Category Where Is_Deleted = '0'";
            var cmd = new SQLiteCommand(str, con);

            con.Open();
            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dat = new isiList();
                    dat.ls_id = dr.GetInt16(0);
                    dat.ls_val= dr.GetString(1);
                    lst.Add(dat);
                }
            }
            catch (Exception ex) { }
            con.Close();

            return lst;
        }

        public class isiList
        {
            public int ls_id { get; set; }
            public string ls_val { get; set; }
        }
    }
}