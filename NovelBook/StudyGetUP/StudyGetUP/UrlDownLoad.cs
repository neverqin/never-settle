using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGetUP
{
   public class UrlDownLoad
    {
       public string url { get; set; }
       public string urlname { get; set; }
       public static List<UrlDownLoad> urlList = new List<UrlDownLoad>();
       public UrlDownLoad(string url,string urlname) 
       {
           this.url = url;
           this.urlname = urlname;
       }
    }
}
