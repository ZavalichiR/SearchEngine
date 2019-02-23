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
    /// <summary>
    /// Modules of this application
    /// </summary>
    public partial class Modules : MetroFramework.Forms.MetroForm
    {
        private List<HTMLObjects> HOs;

        /// <summary>
        /// Files from selected input folder
        /// </summary>
        private string[] _files;

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

                _files = Directory.GetFiles(fbd.SelectedPath);
                _filesCount = _files.Length;

                metroLabel1.Text = _files.Length.ToString() + " selected files";
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
    }
}
