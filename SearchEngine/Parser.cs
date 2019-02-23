using HtmlAgilityPack;
using MetroFramework.Controls;
using SearchEngine;
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
        private HTMLObjects _ho;
        private MetroProgressBar _progressBar1;
        private MetroLabel _status1;
        private MetroLabel _status2;
        private MetroLabel _status3;

        public Parser()
        {
            
        }

        /// <summary>
        /// Parsing function
        /// </summary>
        /// <param name="files">files from input folder</param>
        /// <param name="progressBar1">progressbar</param>
        /// <param name="status1">number of files processed</param>
        /// <param name="status2">name of the current file</param>
        /// <param name="status2">status of processing file</param>
        /// <returns></returns>
        public List<HTMLObjects> DoParsing(string[] files, MetroProgressBar progressBar1, MetroLabel status1, MetroLabel status2, MetroLabel status3)
        {
            _progressBar1 = progressBar1;
            _progressBar1.Value = progressBar1.Minimum;
            _progressBar1.Update();
            _status1 = status1;
            _status2 = status2;
            _status3 = status3;

            List<HTMLObjects> hos = new List<HTMLObjects>();
            int i = 0;
            foreach (var file in files)
            {
                _progressBar1.Value = _progressBar1.Minimum;
                _progressBar1.Update();

                _status1.Text = (++i).ToString() + "/" + files.Count();
                _status1.Update();
                          
                _ho = new HTMLObjects();
                _ho.Name = file.Substring(file.LastIndexOf("\\"));

                _status2.Text = _ho.Name;
                _status2.Update();

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
            {
                _status2.Text = "Parsing Done !";
                _status3.Text = "";
            }
                
                
        }

        private void takeTitle(HtmlNodeCollection nodes)
        {
            _status3.Text = "Processing title               ";
            _status3.Update();
            foreach (var node in nodes)
            {
                _progressBar1.Value += 1;
                _progressBar1.Update();
                _ho.Title += node.InnerHtml + "\n";
            }


        }

        private void takeMeta(HtmlNodeCollection nodes)
        {
            _status3.Text = "Processing Meta Informations  ";
            _status3.Update();
            foreach (var node in nodes)
            {
                _progressBar1.Value += 1;
                _progressBar1.Update();

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
            _status3.Text = "Processing Links from this page";
            _status3.Update();

            foreach (var node in nodes)
            {
                _progressBar1.Value += 1;
                _progressBar1.Update();
                string hrefValue = node.GetAttributeValue("href", string.Empty);
                string intern = isIntern(hrefValue);

                if (intern.Length != 0)
                    _ho.InternalLinks += intern + "\n";
                else
                _ho.ExternalLinks += hrefValue + "\n";
               
            }
        }

        private void takeText(HtmlNodeCollection nodes)
        {
            _status3.Text = "Processing Body content         ";
            _status3.Update();

            foreach (var node in nodes)
            {
                if (node.ParentNode.Name != "script")
                    _ho.Text += node.InnerHtml;
                _progressBar1.Value += 1;
                _progressBar1.Update();
            }

            _ho.CreateHashMap();
        }

        private string isIntern(string link)
        {
            Uri baseLink = new Uri("http://riweb.tibeica.com/tests/l1_basic");
            
            Uri absolute;
            Uri.TryCreate(baseLink, link, out absolute);

            if (absolute == null)
                return "";
            // If link has query it is not internal
            else if (absolute.Query != "")
                return "";
            else if (absolute.ToString().Contains(baseLink.ToString()))
                return absolute.ToString();
            else
                return "";
        }

    }
}