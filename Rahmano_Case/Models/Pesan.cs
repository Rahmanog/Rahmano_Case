using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rahmano_Case.Models
{
    public class Pesan
    {
        public int pesan_id { get; set; }
        public string pesan_isi { get; set; }
        public string Keterangan { get; set; }

        public Pesan()
        {
            pesan_id = 1;
            pesan_isi = "";
            Keterangan = "";
        }
    }
    public class comments
    {
        public int Article_ID { get; set; }
        public int User_ID { get; set; }
        public string Comment_Create { get; set; }
        public string User_Name { get; set; }
        public string comment { get; set; }
    }
}