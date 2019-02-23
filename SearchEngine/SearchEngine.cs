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
    public partial class SearchEngine : MetroFramework.Forms.MetroForm
    {
        public SearchEngine()
        {
            InitializeComponent();
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            MetroFramework.MetroMessageBox.Show(this, "", "This module is not available yet!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            Hide();
            Modules modules = new Modules();
            modules.ShowDialog();
            Show();
        }

        private void SearchEngine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }
    }
}
