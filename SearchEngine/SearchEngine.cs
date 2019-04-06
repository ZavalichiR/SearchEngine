using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine
{
    /// <summary>
    /// Search Engine
    /// </summary>
    public partial class SearchEngine : MetroFramework.Forms.MetroForm
    {
        private Search _search;
        public SearchEngine()
        {
            InitializeComponent();
            _search = new Search();
        }

        /// <summary>
        /// Search Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink1_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem == null)
            {
                MetroFramework.MetroMessageBox.Show(this, "", "Select the type of searching", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var option = comboBox1.SelectedItem.ToString();

            _search.query = metroTextBox1.Text;

            switch (option)
            {
                case "Vectorial":                                     
                    _search.ComputeQuery();
                    var boolResult = _search.ComputeSimilarity();
                    richTextBox1.Clear();
                    foreach (var r in boolResult)
                        richTextBox1.Text += r.Key + " " + r.Value + "\n";
                    break;

                case "Boolean":                 
                    var vectorialResult = _search.BooleanSearch();
                    richTextBox1.Clear();
                    foreach (var r in vectorialResult)
                        richTextBox1.Text += r + "\n";
                    break;
                default:
                    break;
            }


        }

        /// <summary>
        /// Modules of this application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink2_Click(object sender, EventArgs e)
        {
            Hide();
            Modules modules = new Modules();
            modules.ShowDialog();
            Show();
        }

        /// <summary>
        /// Crawling APP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink3_Click(object sender, EventArgs e)
        {
            MetroFramework.MetroMessageBox.Show(this, "", "This module is not available yet!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Indexing application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroLink4_Click(object sender, EventArgs e)
        {
            Hide();
            IndexingPacket ip = new IndexingPacket();
            ip.ShowDialog();
            Show();
        }

        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchEngine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }


    }
}
