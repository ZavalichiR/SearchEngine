using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    public class HTMLObjects
    {
        public Dictionary<string, int> HashMap;
        public string Name;
        public string Title;
        public string MetaDescription;
        public string MetaKeywords;
        public string MetaRobots;
        public string ExternalLinks;
        public string InternalLinks;
        public string Text;

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

        public void CreateHashMap()
        {
            char[] separators = { ' ', '\n', '\r', '\t', ',', '.', '!', '?', '/', '\\', '\'', '\"', '(', ')', '[', ']', '{', '}',':',';'};
            string word = "";
            for (int i = 0; i < Text.Length; ++i)
            {
                if (!separators.Contains(Text[i]))
                { 
                    word += Text[i];
                }
                else if(word.Length > 2 )
                {
                    if (!HashMap.ContainsKey(word))
                        HashMap.Add(word, 1);
                    else
                        HashMap[word]++;
                    word = "";
                }

            }
        }
        public string ShowHashMap()
        {
            string hm = "";
            foreach (var item in HashMap)
            {
                hm += item.Key + " -- " + item.Value + "\n";
            }
            return hm;
        }
    }   
}
