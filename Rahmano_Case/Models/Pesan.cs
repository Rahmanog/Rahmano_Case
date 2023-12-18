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
}