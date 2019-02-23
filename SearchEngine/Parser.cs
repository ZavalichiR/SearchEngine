using HtmlAgilityPack;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    public class Parser
    {
        private string[] _files;
        private HTMLObjects _ho;
        private MetroProgressBar _progressBar1;
        private MetroProgressBar _progressBar2;
        private MetroLabel _status1;
        private MetroLabel _status2;

        public Parser()
        {
            _files = null;
        }
        public List<HTMLObjects> DoParsing(string[] files, MetroProgressBar progressBar1, MetroLabel status1, MetroProgressBar progressBar2, MetroLabel status2)
        {
            progressBar1.Value = progressBar1.Minimum;
            progressBar2.Value = progressBar2.Minimum;

            _progressBar1 = progressBar1;
            _status1 = status1;
            _progressBar2 = progressBar2;
            _status2 = status2;

            List<HTMLObjects> hos = new List<HTMLObjects>();
            foreach (var file in files)
            {

                _ho = new HTMLObjects();
                _ho.Name = file.Substring(file.LastIndexOf("\\"));
                takeObjectsFromHTML(file);
                hos.Add(new HTMLObjects(_ho));
            }
            return hos;
        }

        private void takeObjectsFromHTML(string file)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(File.ReadAllText(file));

            var node = htmlDoc.DocumentNode;
            var titleNodes = node.SelectNodes("//title"); ;
            var metaNodes = node.SelectNodes("//meta"); ;
            var linksNodes = node.SelectNodes("//a[@href]"); ;
            var textNodes = node.SelectNodes("//text()"); ;

            _progressBar1.Maximum = titleNodes.Count + metaNodes.Count + linksNodes.Count + textNodes.Count;

            takeTitle(titleNodes);
            takeMeta(metaNodes);
            takeLinks(linksNodes);
            takeText(textNodes);

            if (_progressBar1.Value == _progressBar1.Maximum)
                _status1.Text = " Parsing Done !";
                
        }

        private void takeTitle(HtmlNodeCollection nodes)
        {
            _status1.Text = "Processing title"; 
            foreach (var node in nodes)
            {
                _progressBar1.Value += 1;
                _ho.Title += node.InnerHtml + "\n";
            }


        }

        private void takeMeta(HtmlNodeCollection nodes)
        {
            _status1.Text = "Processing Meta Informations";
            foreach (var node in nodes)
            {
                _progressBar1.Value += 1;
                string content = node.GetAttributeValue("content", "");
                string tag = node.GetAttributeValue("name", "");
                switch (tag)
                {
                    case "description":
                        _ho.MetaDescription += content + "\n";
                        break;
                    case "keywords":
                        _ho.MetaKeywords += content + "\n";
                        break;
                    case "robots":
                        _ho.MetaRobots += content + "\n";
                        break;
                    default:
                        break;
                }
            }

        }

        private void takeLinks(HtmlNodeCollection nodes)
        {
            _status1.Text = "Processing Links from this page";
            foreach (var node in nodes)
            {
                _progressBar1.Value += 1;
                string hrefValue = node.GetAttributeValue("href", string.Empty);
                string relative = isRelative(hrefValue);

                if (relative.Length != 0)
                    _ho.InternalLinks += relative + "\n";
                else
                _ho.ExternalLinks += hrefValue + "\n";
               
            }
        }

        private void takeText(HtmlNodeCollection nodes)
        {
            _status1.Text = "Processing Body content";
            foreach (var node in nodes)
            {
                if (node.ParentNode.Name != "script")
                    _ho.Text += node.InnerHtml;
                _progressBar1.Value += 1;
            }

            _ho.CreateHashMap();
        }

        private string isRelative(string link)
        {
            if (link.Length > 1)
                return link;
            return "";
        }
    }
}