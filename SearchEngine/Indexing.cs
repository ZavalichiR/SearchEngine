using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SearchEngine
{
    /// <summary>
    /// This class contain the file and the number of aparition of an word
    /// </summary>
    class FileValue
    {
        public string File;
        public string Counter;

        public FileValue()
        {
            File = "";
            Counter = "";
        }
        public FileValue(string file, string counter)
        {
            File = file;
            Counter = counter;
        }
    }
    class Indexing
    {
        /// <summary>
        /// Create XML files with: file {word : counter}
        /// </summary>
        /// <param name="path"></param>
        /// <param name="HOs"></param>
        public static void DirectIndex(string path, List<HTMLObjects> HOs)
        {
            int counter = 1;    //Number of files
            int lineWrited = 0; //Number of lines from a file
            foreach (var ho in HOs)
            {
                string currentFile = path + "\\" + "ID" + counter + ".xml";

                // Write in an existing XML
                if (File.Exists(currentFile))
                {
                    lineWrited++;

                    XmlDocument doc = new XmlDocument();
                    doc.Load(currentFile);

                    var node = doc.LastChild;

                    //Create a new element
                    XmlElement ele = doc.CreateElement("File");
                    ele.SetAttribute("name", ho.Name);
                    node.AppendChild(ele);

                    foreach (var hm in ho.HashMap)
                    {
                        XmlElement ele2 = doc.CreateElement("key-value");

                        ele2.SetAttribute("value", hm.Value.ToString());
                        ele2.SetAttribute("key", hm.Key);

                        ele.AppendChild(ele2);
                    }

                    doc.Save(currentFile);
                }
                // Create a new XML and write
                else
                {
                    lineWrited++;

                    XmlWriter xmlWriter = XmlWriter.Create(currentFile);

                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("DirectIndex");
                    xmlWriter.WriteStartElement("File");
                    xmlWriter.WriteAttributeString("name", ho.Name);

                    foreach (var hm in ho.HashMap)
                    {
                        xmlWriter.WriteStartElement("key-value");

                        xmlWriter.WriteAttributeString("value", hm.Value.ToString());
                        xmlWriter.WriteAttributeString("key", hm.Key);

                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Close();
                }

                // TODO optimization
                if ((lineWrited % 5) == 0)
                {
                    lineWrited = 0;
                    counter++;
                }
            }

            MessageBox.Show("Done");
        }

        /// <summary>
        /// Create an HashMap with  Word {file : counter, file2 : counter}
        /// </summary>
        /// <param name="DIpath"></param>
        /// <param name="IdIpath"></param>
        public static void IndirectIndex(string DIpath, string IdIpath)
        {

            // Hash map: Word {file: counter, file2: counter, file2:counter ...}
            Dictionary<string, List<FileValue>> hmIdI = new Dictionary<string, List<FileValue>>();

            //  file1:1, file2:2 ...
            List<FileValue> fv = new List<FileValue>();
            
            List<string> files = Directory.GetFiles(DIpath).ToList();

            // Read every file
            foreach (var file in files)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(file);
                var nodes = xDoc.DocumentElement.SelectNodes("File");
                
                // Get "File" nodes
                foreach (XmlNode node in nodes)
                {
                    var fileName = node.Attributes[0].Value;

                    // Get child of "File" bnodes
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        string key = child.Attributes[1].Value;
                        string value = child.Attributes[0].Value;

                        fv.Add(new FileValue(fileName, value));

                        if (hmIdI.ContainsKey(key))
                        {
                            var values = hmIdI[key];
                            values = values.Concat(fv).ToList();
                            hmIdI[key] = values;
                        }
                        else
                            hmIdI.Add(key, fv);

                        fv.Clear();
                    }
                }
            }

            MessageBox.Show("HasMap with Idirect Index has been created");
            MessageBox.Show("Saving the HashMap...");

            saveHashMap(hmIdI, IdIpath);

        }

        /// <summary>
        /// Save the HashMap in XML files
        /// </summary>
        /// <param name="hmIdI"></param>
        /// <param name="IdIpath"></param>
        private static void saveHashMap(Dictionary<string, List<FileValue>> hmIdI, string IdIpath)
        {
            int counter = 1;
            int lineWrited = 0;
            foreach (var hm in hmIdI)
            {
                string currentFile = IdIpath + "\\" + "IdI" + counter + ".xml";

                if (File.Exists(currentFile))
                {
                    lineWrited++;

                    XmlDocument doc = new XmlDocument();
                    doc.Load(currentFile);

                    var node = doc.LastChild;

                    //Create a new element
                    XmlElement ele = doc.CreateElement("Word");
                    ele.SetAttribute("name", hm.Key);
                    node.AppendChild(ele);

                    foreach (var val in hm.Value)
                    {
                        XmlElement ele2 = doc.CreateElement("key-value");

                        ele2.SetAttribute("counter", val.Counter);
                        ele2.SetAttribute("file", val.File);

                        ele.AppendChild(ele2);
                    }

                    doc.Save(currentFile);
                }
                else
                {
                    lineWrited++;

                    XmlWriter xmlWriter = XmlWriter.Create(currentFile);

                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("IndirectIndex");
                    xmlWriter.WriteStartElement("Word");
                    xmlWriter.WriteAttributeString("name", hm.Key);

                    foreach (var val in hm.Value)
                    {
                        xmlWriter.WriteStartElement("key-value");

                        xmlWriter.WriteAttributeString("counter", val.Counter);
                        xmlWriter.WriteAttributeString("file", val.File);

                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Close();
                }

                // TODO optimization
                if ((lineWrited % 5) == 0)
                {
                    lineWrited = 0;
                    counter++;
                }
            }

            MessageBox.Show("Done");
        }
    }
}
