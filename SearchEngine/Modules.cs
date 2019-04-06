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
using System.Xml;

namespace SearchEngine
{
    /// <summary>
    /// Modules of this application
    /// </summary>
    public partial class Modules : MetroFramework.Forms.MetroForm
    {
        private List<HTMLObjects> HOs;
        private List<string> _exceptionWords = new List<string>();
        private List<string> _stopWords = new List<string>();

        /// <summary>
        /// Files from selected input folder
        /// </summary>
        private List<string> _files;

        /// <summary>
        /// Number of files founded
        /// </summary>
        private int _filesCount;

        /// <summary>
        /// Constructor implicit
        /// </summary>
        public Modules()
        {
            InitializeComponent();

            metroLink2.Enabled = false;
            metroLink3.Enabled = false;
            metroLink4.Enabled = false;
            metroLink5.Enabled = false;
            metroLink6.Enabled = false;
            //metroLink7.Enabled = false;

            metroProgressBar1.Visible = false;
            metroProgressBar1.Value = metroProgressBar1.Maximum;

            metroLabel2.Visible = false;
            metroLabel3.Visible = false;
            metroLabel4.Visible = false;
        }

        /// <summary>
        /// Select input folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;

            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            path += "\\Data\\HTML files"; // Set HTML files as default 

            fbd.SelectedPath = path;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                metroTextBox1.Text = fbd.SelectedPath;

                _files = new List<string>();
                _files = Directory.GetFiles(fbd.SelectedPath).ToList();
                DirSearch(fbd.SelectedPath);
                _filesCount = _files.Count();

                metroLabel1.Text = _files.Count.ToString() + " selected files";
            }

            // Parsing button is available only if exists files
            if (_filesCount == 0)
                metroLink2.Enabled = false;
            else
            {
                metroLink2.ForeColor = Color.Green;
                metroLink2.Enabled = true;
            }
        }

        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Modules_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        /// <summary>
        /// Parsing HTML content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink2_Click(object sender, EventArgs e)
        {
            // Read Exception and Stopwords file
            readExceptionFile();
            readStopWordsFile();

            // Parse Html files
            Parser parser = new Parser();
            metroLabel2.Visible = true;
            metroLabel3.Visible = true;
            metroLabel4.Visible = true;

            metroProgressBar1.Visible = true;
            metroProgressBar1.ForeColor = Color.Blue;

            metroLabel4.Text = " / " + _files.Count();
            metroLabel4.Update();

            HOs = parser.DoParsing(_files, metroProgressBar1, metroLabel2, metroLabel3, metroLabel4);

            metroLink2.Enabled = false;
            metroLink3.Enabled = true;
            metroLink4.Enabled = true;
            metroLink4.Enabled = true;
            metroLink5.Enabled = true;
            metroLink6.Enabled = true;
            metroLink7.Enabled = true;
        }

        /// <summary>
        /// Save files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink3_Click(object sender, EventArgs e)
        {
            metroLabel2.Text = "0 / " + HOs.Count; ;
            metroLabel2.Update();

            metroProgressBar1.Value = metroProgressBar1.Minimum;
            metroProgressBar1.Maximum = HOs.Count;

            // Select folder for saving files
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;

            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            path += "\\Data\\Parsed Files"; // Select Parsed Files as default

            fbd.SelectedPath = path;
            if (fbd.ShowDialog() == DialogResult.OK)
                path = fbd.SelectedPath;

            int j = 0;
            foreach (var ho in HOs)
            {
                metroLabel2.Text = (++j).ToString() + "/" + HOs.Count; ;
                metroLabel2.Update();

                metroProgressBar1.Value += 1;

                metroLabel3.Text = "Saving " + ho.Name;
                metroLabel3.Update();

                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(path + "\\" + ho.Name))
                {
                    file.WriteLine("------TITLE\n");
                    file.WriteLine(ho.Title);
                    file.WriteLine("------META\n");
                    file.WriteLine("-----------Description\n");
                    file.WriteLine(ho.MetaDescription);
                    file.WriteLine("-----------KeyWords\n");
                    file.WriteLine(ho.MetaKeywords);
                    file.WriteLine("-----------Robots\n");
                    file.WriteLine(ho.MetaRobots);
                    file.WriteLine("------LINKS\n");
                    file.WriteLine("-----------External Links\n");
                    file.WriteLine(ho.ExternalLinks);
                    file.WriteLine("-----------Internal Links\n");
                    file.WriteLine(ho.InternalLinks);
                    file.WriteLine("------BODY\n");
                    file.WriteLine(ho.Text);
                    file.WriteLine("\n-------------------------------HASH MAP----------------------------------------\n");
                    file.WriteLine(ho.ShowHashMap());
                }
            }

            if (metroProgressBar1.Value == metroProgressBar1.Maximum)
                metroLabel3.Text = "All files are saved";

            metroLink3.Enabled = false;
        }

        /// <summary>
        /// Show files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink4_Click(object sender, EventArgs e)
        {
            Hide();
            FileContent fc = new FileContent(HOs);
            fc.ShowDialog();
            Show();
        }

        /// <summary>
        /// Show Hash Map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink5_Click(object sender, EventArgs e)
        {
            Hide();
            HMContent fc = new HMContent(HOs);
            fc.ShowDialog();
            Show();
        }

        /// <summary>
        /// Read files from a directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<string> readFiles(string path)
        {
            List<string> files = new List<string>();
            var directory = Directory.GetDirectories(path);
            files.Concat(Directory.GetFiles(path).ToList());

            if (directory.Count() < 0)
                return files;
            else
            {
                foreach (var dir in directory)
                    readFiles(dir);
            }

            return files;
        }

        /// <summary>
        /// Read files form directory and subDirectory
        /// </summary>
        /// <param name="sDir"></param>
        private void DirSearch(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                        _files.Add(f);
                    DirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        /// <summary>
        /// Read froam ExceptionFile
        /// </summary>
        private void readExceptionFile()
        {
            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            path += "\\Data"; // Set Data folder

            string line;

            // Read exception file
            System.IO.StreamReader file =
                new System.IO.StreamReader(path + "\\ExceptionWords.txt");
            while ((line = file.ReadLine()) != null)
            {
                _exceptionWords.Add(line);
            }
            file.Close();
        }

        /// <summary>
        /// Read from StopWordsfile
        /// </summary>
        private void readStopWordsFile()
        {
            // Read stopwords file
            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            path += "\\Data"; // Set HTML files as default 

            string line;
            System.IO.StreamReader file =
               new System.IO.StreamReader(path + "\\StopWords.txt");
            while ((line = file.ReadLine()) != null)
            {
                _stopWords.Add(line);
            }
            file.Close();
        }

        /// <summary>
        /// Direct Indexing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink6_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            path += "\\Data\\Maped Files\\Direct";


            foreach (var file in Directory.GetFiles(path).ToList())
                File.Delete(file);

            Indexing.DirectIndex2(path, HOs);         
        }

        /// <summary>
        /// Indirect indexing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink7_Click(object sender, EventArgs e)
        {
            

            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            string DIpath = path + "\\Data\\Maped Files\\Direct";
            string IdIpath = path + "\\Data\\Maped Files\\Indirect";

            foreach (var file in Directory.GetFiles(IdIpath).ToList())
                File.Delete(file);

            Indexing.IndirectIndex2(DIpath, IdIpath);          
        }

        /// <summary>
        /// Boolean searching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton1_Click(object sender, EventArgs e)
        {
            List<string> result = new List<string>();
            Search s = new Search();
            s.query = metroTextBox2.Text;
            result = s.BooleanSearch();
            richTextBox1.Clear();
            foreach (var r in result)
            {
                richTextBox1.Text += r + "\n";
            }
        }

        /// <summary>
        /// Vectorial searching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton2_Click(object sender, EventArgs e)
        {
            Search s = new Search();
            s.query = metroTextBox3.Text;
            s.ComputeQuery();

            var result = s.ComputeSimilarity();

            richTextBox2.Clear();
            foreach (var r in result)
            {
                richTextBox2.Text += r.Key +  " " + r.Value + "\n";
            }

        }
    }
}
