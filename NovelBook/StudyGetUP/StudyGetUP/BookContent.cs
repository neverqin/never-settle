using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGetUP
{
    public class BookContent
    {
        public static int id { get; set; }
        public int tid { get; set; }
        public string title{get;set;}
        public string chapter { get; set; }
        public string url { get; set; }
        public static List<BookContent> bookcontentlist = new List<BookContent>();
    }
}
