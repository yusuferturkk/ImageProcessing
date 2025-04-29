namespace ImageProcessing
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
            btnCrop = new Button();
            btnToGray = new Button();
            openFileDialog1 = new OpenFileDialog();
            btnResetImage = new Button();
            btnToHVS = new Button();
            txtCropWidth = new TextBox();
            txtCropHeight = new TextBox();
            trackBarZoomScroll = new TrackBar();
            btnLoadImage = new Button();
            btnToCMYK = new Button();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarZoomScroll).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(350, 325);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // btnCrop
            // 
            btnCrop.Location = new Point(258, 481);
            btnCrop.Name = "btnCrop";
            btnCrop.Size = new Size(114, 23);
            btnCrop.TabIndex = 7;
            btnCrop.Text = "Kırpma";
            btnCrop.UseVisualStyleBackColor = true;
            btnCrop.Click += btnCrop_Click;
            // 
            // btnToGray
            // 
            btnToGray.Location = new Point(610, 403);
            btnToGray.Name = "btnToGray";
            btnToGray.Size = new Size(114, 23);
            btnToGray.TabIndex = 11;
            btnToGray.Text = "Gri Dönüşüm";
            btnToGray.UseVisualStyleBackColor = true;
            btnToGray.Click += btnToGray_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnResetImage
            // 
            btnResetImage.Location = new Point(22, 442);
            btnResetImage.Name = "btnResetImage";
            btnResetImage.Size = new Size(114, 23);
            btnResetImage.TabIndex = 6;
            btnResetImage.Text = "Resmi Sıfırla";
            btnResetImage.UseVisualStyleBackColor = true;
            btnResetImage.Click += btnResetImage_Click;
            // 
            // btnToHVS
            // 
            btnToHVS.Location = new Point(610, 443);
            btnToHVS.Name = "btnToHVS";
            btnToHVS.Size = new Size(114, 23);
            btnToHVS.TabIndex = 12;
            btnToHVS.Text = "HVS Dönüşüm";
            btnToHVS.UseVisualStyleBackColor = true;
            btnToHVS.Click += btnToHVS_Click;
            // 
            // txtCropWidth
            // 
            txtCropWidth.Location = new Point(258, 403);
            txtCropWidth.Name = "txtCropWidth";
            txtCropWidth.Size = new Size(114, 23);
            txtCropWidth.TabIndex = 8;
            // 
            // txtCropHeight
            // 
            txtCropHeight.Location = new Point(258, 443);
            txtCropHeight.Name = "txtCropHeight";
            txtCropHeight.Size = new Size(114, 23);
            txtCropHeight.TabIndex = 9;
            // 
            // trackBarZoomScroll
            // 
            trackBarZoomScroll.Location = new Point(437, 459);
            trackBarZoomScroll.Maximum = 20;
            trackBarZoomScroll.Minimum = 1;
            trackBarZoomScroll.Name = "trackBarZoomScroll";
            trackBarZoomScroll.Size = new Size(114, 45);
            trackBarZoomScroll.TabIndex = 10;
            trackBarZoomScroll.Value = 10;
            trackBarZoomScroll.Scroll += trackBarZoomScroll_Scroll;
            // 
            // btnLoadImage
            // 
            btnLoadImage.Location = new Point(22, 402);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(114, 23);
            btnLoadImage.TabIndex = 5;
            btnLoadImage.Text = "Resim Yükle";
            btnLoadImage.UseVisualStyleBackColor = true;
            btnLoadImage.Click += btnLoadImage_Click_1;
            // 
            // btnToCMYK
            // 
            btnToCMYK.Location = new Point(610, 481);
            btnToCMYK.Name = "btnToCMYK";
            btnToCMYK.Size = new Size(114, 23);
            btnToCMYK.TabIndex = 13;
            btnToCMYK.Text = "CMYK Dönüşüm";
            btnToCMYK.UseVisualStyleBackColor = true;
            btnToCMYK.Click += btnToCMYK_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(381, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(350, 325);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(189, 407);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 15;
            label1.Text = "Genişlik";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(181, 446);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 16;
            label2.Text = "Yükseklik";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(430, 421);
            label3.Name = "label3";
            label3.Size = new Size(121, 15);
            label3.TabIndex = 17;
            label3.Text = "Yakınlaştır / Uzaklaştır";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(146, 357);
            label4.Name = "label4";
            label4.Size = new Size(80, 15);
            label4.TabIndex = 18;
            label4.Text = "Orijinal Resim";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(532, 357);
            label5.Name = "label5";
            label5.Size = new Size(85, 15);
            label5.TabIndex = 19;
            label5.Text = "İşlenmiş Resim";
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(22, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(731, 325);
            panel1.TabIndex = 20;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(770, 516);
            Controls.Add(panel1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnToCMYK);
            Controls.Add(btnLoadImage);
            Controls.Add(trackBarZoomScroll);
            Controls.Add(txtCropHeight);
            Controls.Add(txtCropWidth);
            Controls.Add(btnToHVS);
            Controls.Add(btnResetImage);
            Controls.Add(btnToGray);
            Controls.Add(btnCrop);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarZoomScroll).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox1;
        private Button btnCrop;
        private Button btnToGray;
        private OpenFileDialog openFileDialog1;
        private Button btnResetImage;
        private Button btnToHVS;
        private TextBox txtCropWidth;
        private TextBox txtCropHeight;
        private TrackBar trackBarZoomScroll;
        private Button btnLoadImage;
        private Button btnToCMYK;
        private PictureBox pictureBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Panel panel1;
    }
}
