using System.Drawing;
using System.Windows.Forms;

namespace EdgeDetectionApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            button1 = new Button();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            menuStrip1 = new MenuStrip();
            uploadToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            comboBox1 = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(43, 58);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(300, 300);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // Allows you to scale the loaded bitmap to fit the size of the PictureBox control
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(472, 58);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(300, 300);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(373, 194);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click; // Link Button1_Click to button1's Click event
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(43, 397);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(729, 29);
            progressBar1.TabIndex = 3;
            progressBar1.Click += ProgressBar1_Click; // Link ProgressBar1_Click to progressBar1's Click event
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(349, 379);
            label1.Name = "label1";
            label1.Size = new Size(117, 15);
            label1.TabIndex = 4;
            label1.Text = "Execution time: 0 ms";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { uploadToolStripMenuItem, saveToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(828, 24);
            menuStrip1.TabIndex = 5;
            menuStrip1.Text = "menuStrip1";
            // 
            // uploadToolStripMenuItem
            // 
            uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            uploadToolStripMenuItem.Size = new Size(57, 20);
            uploadToolStripMenuItem.Text = "Upload";
            uploadToolStripMenuItem.Click += UploadToolStripMenuItem_Click; // Link UploadToolStripMenuItem_Click to uploadToolStripMenuItem's Click event
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(43, 20);
            saveToolStripMenuItem.Text = "Save";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "C++", "Assembly" });
            comboBox1.Location = new Point(349, 290);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(117, 23);
            comboBox1.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(386, 272);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 7;
            label2.Text = "Library";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(103, 40);
            label3.Name = "label3";
            label3.Size = new Size(136, 15);
            label3.TabIndex = 8;
            label3.Text = "Photo before processing";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(551, 40);
            label4.Name = "label4";
            label4.Size = new Size(126, 15);
            label4.TabIndex = 9;
            label4.Text = "Photo after processing";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 467);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Controls.Add(button1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Edge Detection App";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button button1;
        private ProgressBar progressBar1;
        private Label label1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem uploadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ComboBox comboBox1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
