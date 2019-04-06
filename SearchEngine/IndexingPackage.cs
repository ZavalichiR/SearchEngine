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

namespace SearchEngine
{
    public partial class IndexingPacket : MetroFramework.Forms.MetroForm
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

        public IndexingPacket()
        {
            InitializeComponent();
            panel1.Visible = false;
        }

        /// <summary>
        /// Start indexing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            SelectDirectory();
            Parsing();
            DirectIndexing();
            IndirectIndexing();
        }

        /// <summary>
        /// Select input folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDirectory()
        {
            MessageBox.Show(this, "Select input directory!");

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
        }

        /// <summary>
        /// Parsing HTML content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parsing()
        {
            // Read Exception and Stopwords file
            ReadExceptionFile();
            ReadStopWordsFile();

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
        private void ReadExceptionFile()
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
        private void ReadStopWordsFile()
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
        private void DirectIndexing()
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
        private void IndirectIndexing()
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

        
    }
}
