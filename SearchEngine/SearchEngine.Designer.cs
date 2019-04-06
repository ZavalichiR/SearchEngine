namespace SearchEngine
{
    partial class SearchEngine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.metroTextBox1 = new MetroFramework.Controls.MetroTextBox();
            this.metroLink1 = new MetroFramework.Controls.MetroLink();
            this.metroLink2 = new MetroFramework.Controls.MetroLink();
            this.metroLink3 = new MetroFramework.Controls.MetroLink();
            this.metroLink4 = new MetroFramework.Controls.MetroLink();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // metroTextBox1
            // 
            // 
            // 
            // 
            this.metroTextBox1.CustomButton.Image = null;
            this.metroTextBox1.CustomButton.Location = new System.Drawing.Point(233, 1);
            this.metroTextBox1.CustomButton.Name = "";
            this.metroTextBox1.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox1.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox1.CustomButton.TabIndex = 1;
            this.metroTextBox1.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox1.CustomButton.UseSelectable = true;
            this.metroTextBox1.CustomButton.Visible = false;
            this.metroTextBox1.Lines = new string[] {
        "Search here"};
            this.metroTextBox1.Location = new System.Drawing.Point(36, 132);
            this.metroTextBox1.MaxLength = 32767;
            this.metroTextBox1.Name = "metroTextBox1";
            this.metroTextBox1.PasswordChar = '\0';
            this.metroTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox1.SelectedText = "";
            this.metroTextBox1.SelectionLength = 0;
            this.metroTextBox1.SelectionStart = 0;
            this.metroTextBox1.ShortcutsEnabled = true;
            this.metroTextBox1.Size = new System.Drawing.Size(255, 23);
            this.metroTextBox1.TabIndex = 0;
            this.metroTextBox1.Text = "Search here";
            this.metroTextBox1.UseSelectable = true;
            this.metroTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLink1
            // 
            this.metroLink1.BackColor = System.Drawing.Color.White;
            this.metroLink1.ForeColor = System.Drawing.Color.White;
            this.metroLink1.Location = new System.Drawing.Point(334, 132);
            this.metroLink1.Name = "metroLink1";
            this.metroLink1.Size = new System.Drawing.Size(75, 23);
            this.metroLink1.TabIndex = 1;
            this.metroLink1.Text = "Search";
            this.metroLink1.UseSelectable = true;
            this.metroLink1.Click += new System.EventHandler(this.metroLink1_Click);
            // 
            // metroLink2
            // 
            this.metroLink2.Location = new System.Drawing.Point(29, 63);
            this.metroLink2.Name = "metroLink2";
            this.metroLink2.Size = new System.Drawing.Size(129, 23);
            this.metroLink2.TabIndex = 2;
            this.metroLink2.Text = "Indexing Modules";
            this.metroLink2.UseSelectable = true;
            this.metroLink2.Click += new System.EventHandler(this.metroLink2_Click);
            // 
            // metroLink3
            // 
            this.metroLink3.Location = new System.Drawing.Point(165, 63);
            this.metroLink3.Name = "metroLink3";
            this.metroLink3.Size = new System.Drawing.Size(134, 23);
            this.metroLink3.TabIndex = 3;
            this.metroLink3.Text = "Crawling Modules";
            this.metroLink3.UseSelectable = true;
            this.metroLink3.Click += new System.EventHandler(this.metroLink3_Click);
            // 
            // metroLink4
            // 
            this.metroLink4.BackColor = System.Drawing.Color.White;
            this.metroLink4.ForeColor = System.Drawing.Color.Black;
            this.metroLink4.Location = new System.Drawing.Point(36, 103);
            this.metroLink4.Name = "metroLink4";
            this.metroLink4.Size = new System.Drawing.Size(255, 23);
            this.metroLink4.TabIndex = 4;
            this.metroLink4.Text = "Load new indexing set";
            this.metroLink4.UseSelectable = true;
            this.metroLink4.Click += new System.EventHandler(this.metroLink4_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(29, 161);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(299, 184);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Boolean",
            "Vectorial"});
            this.comboBox1.Location = new System.Drawing.Point(334, 161);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(75, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.Text = "Select";
            // 
            // SearchEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 369);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.metroLink4);
            this.Controls.Add(this.metroLink3);
            this.Controls.Add(this.metroLink2);
            this.Controls.Add(this.metroLink1);
            this.Controls.Add(this.metroTextBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.KeyPreview = true;
            this.Name = "SearchEngine";
            this.Padding = new System.Windows.Forms.Padding(23, 60, 23, 20);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Text = "Search Engine";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchEngine_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox metroTextBox1;
        private MetroFramework.Controls.MetroLink metroLink1;
        private MetroFramework.Controls.MetroLink metroLink2;
        private MetroFramework.Controls.MetroLink metroLink3;
        private MetroFramework.Controls.MetroLink metroLink4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

