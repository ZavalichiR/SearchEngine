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
    public partial class FileContent : MetroFramework.Forms.MetroForm
    {
        private List<HTMLObjects> _hos;
        public FileContent(List<HTMLObjects> hos)
        {
            InitializeComponent();
            _hos = hos;
            foreach (var ho in hos)
                metroComboBox1.Items.Add(ho.Name);
        }

        private void FileContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
               
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var ho in _hos)
            {
                if (metroComboBox1.SelectedItem.ToString() == ho.Name)
                {
                    richTextBox1.Text = ho.Title;
                    richTextBox2.Text = ho.MetaDescription;
                    richTextBox3.Text = ho.MetaKeywords;
                    richTextBox4.Text = ho.MetaRobots;
                    richTextBox5.Text = "    External Links:";
                    richTextBox5.Text = ho.ExternalLinks;
                    richTextBox6.Text = "    Internal Links:";
                    richTextBox6.Text = ho.InternalLinks;
                    richTextBox7.Text = ho.Text;
                }
            }
        }
    }
}
