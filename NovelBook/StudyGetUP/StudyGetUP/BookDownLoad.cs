using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGetUP
{
    public class BookDownLoad
    {
        public string title { get; set; }
        public string writer { get; set; }
        public string desc { get; set; }
        public string url { get; set; }
        public static List<BookDownLoad> bookList = new List<BookDownLoad>();
        public BookDownLoad(string title,string writer,string desc,string url) 
        {
            this.title = title;
            this.writer = writer;
            this.desc = desc;
            this.url = url;
        }
        public BookDownLoad()
        {
        }
    }
}
