using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudyGetUP
{
    public partial class FrmFind : Form
    {
        public FrmFind()
        {
            InitializeComponent(); 
        }
        public FrmFind(Timer trim)
        {
            timer = trim;
            InitializeComponent();
        }
        //下载
        public Timer timer;
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource==null)
            {
                MessageBox.Show("请先点击搜索按钮！");
                return;
            }
            string url = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string title = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            Encoding enc = Encoding.GetEncoding("utf-8");
            string html = HttpHelper.DownloadHtml(url, enc);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            string liPath = "//*[@class='volume']/ul/li";
            HtmlNodeCollection nodeList = doc.DocumentNode.SelectNodes(liPath);
            foreach (HtmlNode node in nodeList)
            {
                string secondHtml = node.InnerHtml;
                if (string.IsNullOrWhiteSpace(secondHtml)) continue;
                HtmlAgilityPack.HtmlDocument secondDoc = new HtmlAgilityPack.HtmlDocument();
                secondDoc.LoadHtml(secondHtml);
                HtmlNode secondNode = secondDoc.DocumentNode.SelectSingleNode("//a");
                BookContent bc = new BookContent();
                bc.tid = BookContent.id;
                bc.title = title;
                if (secondNode.Attributes["href"] != null)
                {
                    bc.url = secondNode.Attributes["href"].Value;
                }
                bc.chapter = secondNode.InnerText;
                BookContent.bookcontentlist.Add(bc);
            }
            MessageBox.Show("载入成功");
            BookContent.id++;
            timer.Start();
            this.Hide();
        }
        //搜索
        private void button1_Click(object sender, EventArgs e)
        {
            BookDownLoad.bookList.Clear();
            Encoding enc = Encoding.GetEncoding("utf-8");
            //UrlDownLoad.urlList.ForEach(r =>
            //{
            //});
            foreach (var item in UrlDownLoad.urlList)
            {
                string html = HttpHelper.DownloadHtml(item.url, enc);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                string pageNumberPath = "//*[@id='page-container']";
                HtmlNode pageNumberNode = doc.DocumentNode.SelectSingleNode(pageNumberPath);
                if (pageNumberNode != null)
                {
                    string numurl = pageNumberNode.Attributes["data-url"].Value;
                    for (int i = 1; i < 10; i++)
                    {
                        string pageUrl = string.Format("{0}&pageNum={1}",item.url+"?"+numurl.Substring(numurl.LastIndexOf(';')+1) , i);
                        newdown(item, pageUrl);
                    }
                }
            }
            dataGridView1.DataSource = BookDownLoad.bookList;
        }

        private static void newdown(UrlDownLoad item,string url)
        {
            string html = HttpHelper.DownloadUrl(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            string liPath = "//*[@class='right-book-list']/ul/li";
            HtmlNodeCollection nodeList = doc.DocumentNode.SelectNodes(liPath);
            foreach (HtmlNode node in nodeList)
            {
                string secondHtml = node.InnerHtml;
                if (string.IsNullOrWhiteSpace(secondHtml)) continue;
                HtmlAgilityPack.HtmlDocument secondDoc = new HtmlAgilityPack.HtmlDocument();
                secondDoc.LoadHtml(secondHtml);
                HtmlNode secondNode = secondDoc.DocumentNode.SelectSingleNode("//h3/a");
                BookDownLoad bb = new BookDownLoad();
                if (secondNode.Attributes["href"] != null)
                {
                    bb.url = item.url.Substring(0, item.url.LastIndexOf('/')) + secondNode.Attributes["href"].Value;
                }
                bb.title = secondNode.Attributes["title"].Value;
                HtmlNode secondNode2 = secondDoc.DocumentNode.SelectSingleNode("//h4/a");
                bb.writer = secondNode2.InnerText;
                HtmlNode secondNode3 = secondDoc.DocumentNode.SelectSingleNode("//*[@class='intro']");
                string content = secondNode3.InnerText;
                bb.desc = content.Trim().Substring(0, 7) + "...";
                BookDownLoad.bookList.Add(bb);
            }
        }
    }
}
