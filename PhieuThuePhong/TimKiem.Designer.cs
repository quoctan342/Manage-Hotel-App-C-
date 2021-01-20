
namespace QuanLyKhachSan.PTP
{
    partial class TimKiem
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
            this.cbbP = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbbDMP = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbbP
            // 
            this.cbbP.FormattingEnabled = true;
            this.cbbP.Location = new System.Drawing.Point(303, 12);
            this.cbbP.Name = "cbbP";
            this.cbbP.Size = new System.Drawing.Size(173, 21);
            this.cbbP.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(259, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Phòng";
            // 
            // cbbDMP
            // 
            this.cbbDMP.FormattingEnabled = true;
            this.cbbDMP.Location = new System.Drawing.Point(108, 12);
            this.cbbDMP.Name = "cbbDMP";
            this.cbbDMP.Size = new System.Drawing.Size(145, 21);
            this.cbbDMP.TabIndex = 11;
            this.cbbDMP.SelectedIndexChanged += new System.EventHandler(this.cbbDMP_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Danh mục phòng";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(105, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 26);
            this.button1.TabIndex = 14;
            this.button1.Text = "Tìm kiếm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(262, 40);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(151, 26);
            this.button2.TabIndex = 15;
            this.button2.Text = "Thoát";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TimKiem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 73);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbbP);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbbDMP);
            this.Controls.Add(this.label2);
            this.Name = "TimKiem";
            this.Text = "TimKiem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbbDMP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}