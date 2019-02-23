using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace SearchEngine
{

    /// <summary>
    /// Show content of Hash Map
    /// </summary>
    public partial class HMContent : MetroFramework.Forms.MetroForm
    {

        public event EventHandler VScroll;
        public enum ScrollBarType : uint
        {
            SbHorz = 0,
            SbVert = 1,
            SbCtl = 2,
            SbBoth = 3
        }

        public enum Message : uint
        {
            WM_VSCROLL = 0x0115
        }

        public enum ScrollBarCommands : uint
        {
            SB_THUMBPOSITION = 4
        }

        [DllImport("User32.dll")]
        public extern static int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("User32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        

        private List<HTMLObjects> _hos;
        public HMContent(List<HTMLObjects> hos)
        {
            InitializeComponent();
            richTextBox1.Visible = false;
            richTextBox7.Visible = false;
            _hos = hos;
            foreach (var ho in hos)
                metroComboBox1.Items.Add(ho.Name);
        }

        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HMContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        /// <summary>
        /// Show information form selected object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox7.Text = "";
            richTextBox1.Text = "";

            int i = 0;
            foreach (var ho in _hos)
            {
                if (metroComboBox1.SelectedItem.ToString() == ho.Name)
                {
                    richTextBox7.Text = ho.ShowHashMapKeys();
                    richTextBox1.Text = ho.ShowHashMapValues();
                    
                }
            }
            richTextBox1.Visible = true;
            richTextBox7.Visible = true;
        }

        private void richTextBox7_VScroll(object sender, EventArgs e)
        {
            ulong nPos = (ulong)GetScrollPos(richTextBox7.Handle, (int)ScrollBarType.SbVert);
            nPos <<= 16;
            int wParam = (int)ScrollBarCommands.SB_THUMBPOSITION | (int)nPos;
            SendMessage(richTextBox1.Handle, (int)Message.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));
        }
    }
}
