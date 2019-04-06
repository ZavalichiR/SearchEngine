using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SearchEngine
{
    class Search
    {
        class WordCount
        {
            public string Word;
            public double Count;
            public WordCount()
            {
                Word = "";
                Count = 0;
            }
            public WordCount(string word, double count)
            {
                Word = word;
                Count = count;
            }
            public string GetWord()
            {
                return Word;
            }
            public double GetCount()
            {
                return Count;
            }

        }
        private Dictionary<string, List<string>> _HMidi;
        private Dictionary<string, List<WordCount>> _HMdi;
        private Dictionary<string, double> _idfHM;
        Dictionary<string, List<WordCount>> _tfHM;
        Dictionary<string, List<WordCount>> _vectorialHM;
        Dictionary<string, double> _scalarHM;

        public string query;

        public Search()
        {
            _HMidi = new Dictionary<string, List<string>>();
            _HMdi = new Dictionary<string, List<WordCount>>();
            _idfHM = new Dictionary<string, double>();
            _tfHM = new Dictionary<string, List<WordCount>>();
            _vectorialHM = new Dictionary<string, List<WordCount>>();
            _scalarHM = new Dictionary<string, double>();

            CreateHashMaDirectIndex();
            CreateHashMapIndirectIndex();
            CreateHashMapIndirectIndex();
            CreateIDFandTF();
            ComputeVectorialFiles();
            ComputeScalarFiles();

        }
        public void CreateHashMaDirectIndex()
        {
            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            string IdIpath = path + "\\Data\\Maped Files\\Direct";

            List<string> files = Directory.GetFiles(IdIpath).ToList();

            List<string> filesforWord = new List<string>();

            // Read every file
            foreach (var file in files)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(file);
                var nodes = xDoc.DocumentElement.SelectNodes("File");

                // Get "File" nodes
                foreach (XmlNode node in nodes)
                {

                    string key = node.Attributes[0].Value;

                    List<WordCount> values = new List<WordCount>();
                    // Get child of "File" bnodes
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        string val = child.Attributes[1].Value.ToLower();
                        double key2 = Convert.ToDouble(child.Attributes[0].Value);
                        WordCount value = new WordCount(val, key2);
                        values.Add(value);

                    }
                    if (_HMdi.ContainsKey(key))// hash map       
                    {
                        var aux = _HMdi[key];
                        List<WordCount> newList = new List<WordCount>();
                        aux.Concat(values);
                        _HMdi[key] = aux;
                    }
                    else
                        _HMdi.Add(key, values);
                }
            }
        }
        public void CreateHashMapIndirectIndex()
        {
            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            string IdIpath = path + "\\Data\\Maped Files\\Indirect";

            List<string> files = Directory.GetFiles(IdIpath).ToList();

            List<string> filesforWord = new List<string>();
            // Read every file
            foreach (var file in files)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(file);
                var nodes = xDoc.DocumentElement.SelectNodes("Word");

                // Get "File" nodes
                foreach (XmlNode node in nodes)
                {

                    string key = node.Attributes[0].Value.ToLower();

                    List<string> values = new List<string>();
                    // Get child of "File" bnodes
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        string value = child.Attributes[1].Value;
                        values.Add(value);

                    }
                    if (_HMidi.ContainsKey(key))// hash map       
                    {
                        var aux = _HMidi[key];
                        aux.Concat(values).ToList();
                        _HMidi[key] = aux;
                    }
                    else
                        _HMidi.Add(key, values);

                }
            }
        }

        #region Boolean Search
        public List<string> BooleanSearch()
        {
            List<string> words;
            List<string> wordsForSearch = new List<string>();
            List<string> operation = new List<string>();
            List<string> filesForSearch = new List<string>();
            words = query.Split(' ').ToList();

            List<string> w1 = new List<string>();
            List<string> w2 = new List<string>();
            List<string> result = new List<string>();

            if (words.Count < 3)
            {
                result.Add("Operatie invalida");
                return result;
            }

            for (int i = 0; i < words.Count; ++i)
            {
                bool ok = true;
                if (i % 2 == 1)
                {
                    words[i - 1] = words[i - 1].ToLower();
                    // Not, SoBasic
                    if (_HMidi.ContainsKey(words[i - 1]))
                    {
                        w1 = _HMidi[words[i - 1].ToLower()];
                        if (result.Count == 0)
                        {
                            if (_HMidi.ContainsKey(words[i + 1].ToLower()))
                                w2 = _HMidi[words[i + 1].ToLower()];
                            else
                            {
                                result.Add(" Nu exista:" + words[i + 1].ToLower());
                                ok = false;
                            }
                        }
                        else
                            w2 = result;
                    }
                    else
                    {
                        result.Add(" Nu exista:" + words[i - 1].ToLower());
                        ok = false;
                    }

                    if (ok == true)
                    {
                        var op = words[i].ToUpper();
                        switch (op)
                        {
                            case "AND":
                                result = AND(w1, w2);
                                break;
                            case "OR":
                                result = OR(w1, w2);
                                break;
                            case "NOT":
                                result = NOT(w1, w2);
                                break;
                            default:
                                result.Clear();
                                result.Add(" Operatie invalida ");
                                break;
                        }
                    }

                }
            }
            return result;
        }
        private List<string> AND(List<string> w1, List<string> w2)
        {
            List<string> result = new List<string>();
            foreach (var w in w2)
            {
                if (w1.Contains(w))
                    result.Add(w);
            }
            return result;
        }
        private List<string> OR(List<string> w1, List<string> w2)
        {
            List<string> result = new List<string>();
            result = w1.Concat(w2).ToList();
            result = result.Distinct().ToList();
            return result;
        }
        private List<string> NOT(List<string> w1, List<string> w2)
        {
            List<string> result = new List<string>();
            foreach (var w in w1)
            {
                if (!w2.Contains(w))
                    result.Add(w);
            }
            return result;
        }
        #endregion

        #region Vectorial Search

        /// <summary>
        /// Numarul de aparitii a unui cuvant raportat la numarul total de cuvinte din fisier
        /// tf = count(cuvant)/document.countAll()
        /// per document
        /// </summary>
        /// <returns></returns>
        private void TF()
        {
            foreach (var hm in _HMdi)
            {
                var key = hm.Key;
                List<WordCount> newList = new List<WordCount>();
                foreach (var wc in hm.Value)
                {
                    double count = wc.GetCount() / hm.Value.Count;
                    newList.Add(new WordCount(wc.GetWord(), count));
                }
                _tfHM.Add(key, newList);
            }
        }

        /// <summary>
        /// Logaritm din (numarul total de documente raportat la numarul de documente ce contine cuvantul cuv)
        /// idf = log documents.count/(documents.count where cuv exist))
        /// per cuvant
        /// Creare hashmap cu index
        /// </summary>
        /// <returns></returns>
        private void IDF()
        {
            double nrDocs = _HMidi.Count;
            foreach (var hm in _HMidi)
            {
                double idf = nrDocs / (1 + hm.Value.Count());
                if (idf != 1)
                    idf = Math.Log10(idf);
                _idfHM.Add(hm.Key, idf);
            }
        }

        public void CreateIDFandTF()
        {
            TF();
            IDF();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void ComputeQuery()
        {
            List<WordCount> lWc = new List<WordCount>();
            string[] words = query.Split(' ');
            double scalar = 0;
            foreach (var word in words)
            {

                int nrOfCurrenttWord = Regex.Matches(query, word).Count;
                double tf = (float)nrOfCurrenttWord / words.Count();
                scalar += tf * tf;
                WordCount wc = new WordCount(word, tf);
                lWc.Add(wc);
            }
            _vectorialHM.Add("MyQuerry", lWc);
            scalar = Math.Sqrt(scalar);
            _scalarHM.Add("MyQuerry", scalar);
        }

        /// <summary>
        /// d = tf*idf-cuv1 + tf*df-cuv2 +... 
        /// </summary>
        public void ComputeVectorialFiles()
        {
            foreach (var dir in _tfHM)
            {
                var value = dir.Value;
                foreach (var v in value)
                {
                    v.Count = v.Count * _idfHM[v.Word];
                }
                _vectorialHM.Add(dir.Key, value);
            }
        }

        public void ComputeScalarFiles()
        {
            double scalar = 0;
            foreach (var vect in _vectorialHM)
            {
                var value = vect.Value;
                foreach (var v in value)
                {
                    scalar += v.Count*v.Count;
                }
                scalar = Math.Sqrt(scalar);
                _scalarHM.Add(vect.Key, scalar);
            }
        }

        /// <summary>
        /// d = / q * d / |q| * |d|
        /// </summary>
        private double ComputeScalarDoc()
        {
            double result = 0;
            return result;
        }

        /// <summary>
        /// Calculeaza similaritatea
        /// (d = cuv1-d * cuv1 * q + cuv2-d * cuv2-q) / (d * q)
        /// </summary>
        /// <returns></returns>
        public IOrderedEnumerable<KeyValuePair<string, double>> ComputeSimilarity()
        {
            Dictionary<string, double> filesSimilarity = new Dictionary<string, double>();
            List<string> files = new List<string>();
            List<string> result = new List<string>();
            string[] words = query.Split(' ');
            double scalar = 0;
            double vectorial = 0;
            

            foreach (var word in words)
            {
                if ( _HMidi.ContainsKey(word.ToLower()) )
                    files = files.Concat(_HMidi[word.ToLower()]).ToList();           
            }

            files = files.Distinct().ToList();

            foreach(var file in files)
            {

                var vect1 = _vectorialHM[file];
                var vect2 = _vectorialHM["MyQuerry"];

                scalar = _scalarHM[file] * _scalarHM["MyQuerry"];
                foreach(var v2 in vect2)
                {
                    foreach (var v1 in vect1)
                    {
                        if (v1.Word == v2.Word)
                        {
                            vectorial += v1.Count * v2.Count;
                        }
                    }
                }

                filesSimilarity.Add(file, vectorial / scalar);
            }

            _vectorialHM.Remove("MyQuerry");
            _scalarHM.Remove("MyQuerry");

            var items = from pair in filesSimilarity
                        orderby pair.Value descending
                        select pair;

            return items;
            /// TODO sortare dupa similaritate.
        }
        #endregion
    }
}
