using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine
{
    /// <summary>
    /// Contains the objects from HTML file
    /// </summary>
    public class HTMLObjects
    {
        
        public string BaseLink;
        public string Name;
        public string Title;
        public string MetaDescription;
        public string MetaKeywords;
        public string MetaRobots;
        public string ExternalLinks;
        public string InternalLinks;
        public string Text;
        public Dictionary<string, int> HashMap;

        /// <summary>
        /// Implicit constructor
        /// </summary>
        public HTMLObjects()
        {
            if (HashMap == null)
                HashMap = new Dictionary<string, int>();

            Name = "";
            Title = "";
            MetaDescription = "";
            MetaKeywords = "";
            MetaRobots = "";
            ExternalLinks = "";
            InternalLinks = "";
            Text = "";
        }

        /// <summary>
        /// Explicit constructor
        /// </summary>
        /// <param name="name">File name</param>
        /// <param name="title">HTML title</param>
        /// <param name="md">HTML Meta description</param>
        /// <param name="mk">HTML Meta Keywords</param>
        /// <param name="mr">HTML Meta robots</param>
        /// <param name="el">HTML External links</param>
        /// <param name="il">HTML Internal Links</param>
        /// <param name="text">HTML Body content</param>
        /// <param name="bLink">(TODO) HTML Base Link</param>
        public HTMLObjects(string name, string title, string md, string mk, string mr, string el, string il, string text)
        {
            if (HashMap == null)
                HashMap = new Dictionary<string, int>();

            Name = name;
            Title = title;
            MetaDescription = md;
            MetaKeywords = mk;
            MetaRobots = mr;
            ExternalLinks = el;
            InternalLinks = il;
            Text = text;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="ho">HTMLObjects</param>
        public HTMLObjects(HTMLObjects ho)
        {
            if (HashMap == null)
                HashMap = new Dictionary<string, int>();

            Name = ho.Name;
            Title = ho.Title;
            MetaDescription = ho.MetaDescription;
            MetaKeywords = ho.MetaKeywords;
            MetaRobots = ho.MetaRobots;
            ExternalLinks = ho.ExternalLinks;
            InternalLinks = ho.InternalLinks;
            Text = ho.Text;
            HashMap = ho.HashMap;
        }

        /// <summary>
        /// Create Hash MAP for the HTML from this object
        /// </summary>
        public void CreateHashMap()
        {
            char[] separators = { ' ', '\n', '\r', '\t', ',', '.', '!',
                                 '?', '/', '\\', '\'', '\"', '(', ')',
                                 '[', ']', '{', '}',':',';'
                                 ,'@', '#','$','%','^','&','*','<','>' };
            
            string word = "";
            for (int i = 0; i < Text.Length; ++i)
            {
                if (!separators.Contains(Text[i]))
                {                    
                    word += Text[i];
                }
                else if(word.Length > 2 )
                {
                    int lineIndex = word.IndexOf('-');

                    if (lineIndex == 0)
                        word = word.Substring(1);

                    lineIndex = word.LastIndexOf('-');
                    if (lineIndex == word.Length - 1)
                        word = word.Substring(0, lineIndex);

                    if (!HashMap.ContainsKey(word))
                        HashMap.Add(word, 1);
                    else
                        HashMap[word]++;
                    word = "";
                }

            }
        }

        /// <summary>
        /// Create a string with key--value from HashMap
        /// </summary>
        /// <returns>string</returns>
        public string ShowHashMap()
        {
            string hm = "";
            foreach (var item in HashMap)
            {
                hm += item.Key + " -- " + item.Value + "\n";
            }
            return hm;
        }

        /// <summary>
        /// Create a string with key from HashMap
        /// </summary>
        /// <returns>string</returns>
        public string ShowHashMapKeys()
        {
            string hm = "";
            foreach (var item in HashMap)
            {
                hm += item.Key + "\n";
            }
            return hm;
        }

        /// <summary>
        /// Create a string with value from HashMap
        /// </summary>
        /// <returns>string</returns>
        public string ShowHashMapValues()
        {
            string hm = "";
            foreach (var item in HashMap)
            {
                hm += item.Value + "\n";
            }
            return hm;
        }

        /// <summary>
        /// Create a string with all value-key from HashMap for Direct Index
        /// </summary>
        /// <param name="exceptionWords"></param>
        /// <param name="stopWords"></param>
        public string ShowHashMapDirectIndex()
        {
            string hm = "["+Name+"]" + ":{ ";
            foreach (var item in HashMap)
            {
                hm += item.Key + ":" + item.Value + ",";
            }
            hm += "}";
            return hm;
        }

        /// <summary>
        /// Remove stop words from Hash Map
        /// Thw stop words are from StopWords.txt file
        /// </summary>
        /// <param name="exceptionWords"></param>
        /// <param name="stopWords"></param>
        public void removeStopWords(List<string> exceptionWords,List<string> stopWords)
        {
            List<string> stpW = stopWords.Except(exceptionWords).ToList();

            foreach (var sW in stpW)
            {
                HashMap.Remove(sW);
            }
                
        }
    }   
}
