using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace Rahmano_Case.Models
{
    public class Listing
    {
        string path = "";
        string cs = @"URI=file:C:\dayinmitra\Rahmano_Case\Rahmano_Case\Data\SampleDayinMitra.db";
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;

        public List<isiList> CategoryList()
        {
            List<isiList> lst = new List<isiList>();
            isiList dat;
            con = new SQLiteConnection(cs);

            con.Open();
            string str = "Select * From Tbl_Category Where Is_Deleted = '0'";
            var cmd = new SQLiteCommand(str, con);

            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dat = new isiList();
                dat.ls_id = dr.GetInt16(0);
                dat.ls_val= dr.GetString(1);
                lst.Add(dat);
            }
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