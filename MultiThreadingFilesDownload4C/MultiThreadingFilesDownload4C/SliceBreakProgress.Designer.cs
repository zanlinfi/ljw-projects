namespace LearningDemo
{
    partial class SliceBreakProgress
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
            this.totalBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.bar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.bar2 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.bar3 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.bar4 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // totalBar
            // 
            this.totalBar.Location = new System.Drawing.Point(44, 119);
            this.totalBar.Name = "totalBar";
            this.totalBar.Size = new System.Drawing.Size(304, 25);
            this.totalBar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(354, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "总进度";
            // 
            // bar1
            // 
            this.bar1.Location = new System.Drawing.Point(44, 150);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(304, 25);
            this.bar1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(354, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "进度1";
            // 
            // bar2
            // 
            this.bar2.Location = new System.Drawing.Point(44, 181);
            this.bar2.Name = "bar2";
            this.bar2.Size = new System.Drawing.Size(304, 25);
            this.bar2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(354, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "进度2";
            // 
            // bar3
            // 
            this.bar3.Location = new System.Drawing.Point(44, 212);
            this.bar3.Name = "bar3";
            this.bar3.Size = new System.Drawing.Size(304, 25);
            this.bar3.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(354, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "进度3";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(44, 75);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "分片下载";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bar4
            // 
            this.bar4.Location = new System.Drawing.Point(44, 243);
            this.bar4.Name = "bar4";
            this.bar4.Size = new System.Drawing.Size(304, 25);
            this.bar4.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(354, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "进度3";
            // 
            // SliceBreakProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bar4);
            this.Controls.Add(this.bar3);
            this.Controls.Add(this.bar2);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.totalBar);
            this.Name = "SliceBreakProgress";
            this.Text = "SliceBreakProgress";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar totalBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar bar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar bar2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar bar3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar bar4;
        private System.Windows.Forms.Label label5;
    }
}