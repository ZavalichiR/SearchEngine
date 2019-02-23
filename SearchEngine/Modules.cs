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
    public partial class Modules : MetroFramework.Forms.MetroForm
    {
        private List<HTMLObjects> HOs;
        private string[] _files;
        private int _filesCount;
        public Modules()
        {
            InitializeComponent();
            
            metroLink2.Enabled = false;
            metroLink3.Enabled = false;
            metroLink4.Enabled = false;

            metroProgressBar1.Visible = false;
            metroProgressBar1.Value = metroProgressBar1.Maximum;
            metroLabel2.Visible = false;

            metroProgressBar2.Visible = false;
            metroProgressBar2.Value = metroProgressBar2.Maximum;
            metroLabel3.Visible = false;
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
            path += "\\Data\\HTML files";

            fbd.SelectedPath = path;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                metroTextBox1.Text = fbd.SelectedPath;
                _files = Directory.GetFiles(fbd.SelectedPath);
                _filesCount = _files.Length;
                metroLabel1.Text = _files.Length.ToString() + " files selected";
            }

            if (_filesCount == 0)
                metroLink2.Enabled = false;
            else
            {
                metroLink2.ForeColor = Color.Green;
                metroLink2.Enabled = true;
            }

        }

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
            metroProgressBar1.Visible = true;
            metroProgressBar1.ForeColor = Color.Blue;
            HOs = parser.DoParsing(_files, metroProgressBar1, metroLabel2, metroProgressBar2, metroLabel3);
            metroLink2.Enabled = false;
            metroLink3.Enabled = true;
            metroLink4.Enabled = true;
        }

        /// <summary>
        /// Save files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink3_Click(object sender, EventArgs e)
        {           
            metroProgressBar1.Value = metroProgressBar1.Minimum;
            metroProgressBar1.Maximum = HOs.Count;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;

            string path = Application.StartupPath;
            for (int i = 1; i <= 3; ++i)
                path = path.Substring(0, path.LastIndexOf('\\'));
            path += "\\Data\\Parsed Files";

            fbd.SelectedPath = path;
            if (fbd.ShowDialog() == DialogResult.OK)
                path = fbd.SelectedPath;
            foreach (var ho in HOs)
            {
                metroProgressBar1.Value += 1;
                metroLabel2.Text = "Saving " + ho.Name;
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
                metroLabel2.Text = "All files are saved";
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


    }
}
