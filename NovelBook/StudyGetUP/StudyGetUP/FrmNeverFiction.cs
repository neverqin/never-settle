using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudyGetUP
{
    public partial class FrmNeverFiction : Form
    {
        public FrmNeverFiction()
        {
            InitializeComponent();
        }

        private void 搜索ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFind ff = new FrmFind(timer1);
            ff.StartPosition = FormStartPosition.CenterScreen;
            ff.Show();
        }
        private void FrmNeverFiction_Load(object sender, EventArgs e)
        {
            UrlDownLoad.urlList.Add(new UrlDownLoad("https://www.hongxiu.com/all", "红袖添香"));
            BookContent.id = 1;
        }
        static int index = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (BookContent.bookcontentlist.Count > 0)
            {
                int num1 = 0;
                for (int i = 0; i < BookContent.bookcontentlist.Count; i++)
                {
                    if (i == BookContent.bookcontentlist.Count - 1)
                    {
                        break;
                    }
                    if (BookContent.bookcontentlist[i].tid==index+1)
                    {
                        if (num1==0)
                        {
                            TreeNode Msnode1 = new TreeNode();
                            Msnode1.Text = BookContent.bookcontentlist[i].title;
                            treeView1.Nodes.Add(Msnode1);
                            num1 = 1;
                        }
                        TreeNode Pronode = new TreeNode();
                        Pronode.Text = BookContent.bookcontentlist[i].chapter;
                        Pronode.Tag = BookContent.bookcontentlist[i].url;
                        treeView1.Nodes[index].Nodes.Add(Pronode);
                    }
                }
                index++;
                timer1.Stop();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string url = "";
            if (treeView1.SelectedNode.Tag != null)
            {
                url = "https:"+treeView1.SelectedNode.Tag.ToString();
                Encoding enc = Encoding.GetEncoding("utf-8");
                string html = HttpHelper.DownloadHtml(url, enc);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                string pageNumberPath = "//*[@class='read-content j_readContent']";
                HtmlNode pageNumberNode = doc.DocumentNode.SelectSingleNode(pageNumberPath);
                string str = System.Environment.CurrentDirectory;
                str = str +@"\"+ treeView1.SelectedNode.Parent.Text;
                DirectoryInfo di = new DirectoryInfo(str);
                if (!di.Exists)
                {
                    di.Create();
                }
                str = str + @"\" + treeView1.SelectedNode.Text;
                FileInfo fi = new FileInfo(str);
                if (!fi.Exists)
                {
                    StreamWriter sw = fi.CreateText();
                    string info = pageNumberNode.InnerText;
                    sw.Write(info);
                    sw.Close();
                }
                txtcontent.Text = pageNumberNode.InnerText;
            }
        }
    }
}
