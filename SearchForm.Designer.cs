namespace SMSapplication
{
    partial class SearchForm
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
            this.tbxPhone = new System.Windows.Forms.MaskedTextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gbxPhone = new System.Windows.Forms.GroupBox();
            this.gbxFIO = new System.Windows.Forms.GroupBox();
            this.tbxFIO = new System.Windows.Forms.TextBox();
            this.gbxBadge = new System.Windows.Forms.GroupBox();
            this.tbxBadge = new System.Windows.Forms.MaskedTextBox();
            this.rbBadge = new System.Windows.Forms.RadioButton();
            this.rbFIO = new System.Windows.Forms.RadioButton();
            this.rbPhone = new System.Windows.Forms.RadioButton();
            this.gbxPhone.SuspendLayout();
            this.gbxFIO.SuspendLayout();
            this.gbxBadge.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxPhone
            // 
            this.tbxPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxPhone.Location = new System.Drawing.Point(224, 13);
            this.tbxPhone.Margin = new System.Windows.Forms.Padding(4);
            this.tbxPhone.Mask = "00-000-0000";
            this.tbxPhone.Name = "tbxPhone";
            this.tbxPhone.PromptChar = '#';
            this.tbxPhone.Size = new System.Drawing.Size(123, 22);
            this.tbxPhone.TabIndex = 12;
            this.tbxPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(180, 17);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(36, 16);
            this.label17.TabIndex = 11;
            this.label17.Text = "+998";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSearch.Location = new System.Drawing.Point(316, 174);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 29);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Поиск";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gbxPhone
            // 
            this.gbxPhone.Controls.Add(this.label17);
            this.gbxPhone.Controls.Add(this.tbxPhone);
            this.gbxPhone.Enabled = false;
            this.gbxPhone.Location = new System.Drawing.Point(54, 112);
            this.gbxPhone.Name = "gbxPhone";
            this.gbxPhone.Size = new System.Drawing.Size(354, 44);
            this.gbxPhone.TabIndex = 14;
            this.gbxPhone.TabStop = false;
            // 
            // gbxFIO
            // 
            this.gbxFIO.Controls.Add(this.tbxFIO);
            this.gbxFIO.Enabled = false;
            this.gbxFIO.Location = new System.Drawing.Point(54, 62);
            this.gbxFIO.Name = "gbxFIO";
            this.gbxFIO.Size = new System.Drawing.Size(354, 44);
            this.gbxFIO.TabIndex = 15;
            this.gbxFIO.TabStop = false;
            // 
            // tbxFIO
            // 
            this.tbxFIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxFIO.Location = new System.Drawing.Point(125, 13);
            this.tbxFIO.Name = "tbxFIO";
            this.tbxFIO.Size = new System.Drawing.Size(223, 22);
            this.tbxFIO.TabIndex = 11;
            // 
            // gbxBadge
            // 
            this.gbxBadge.Controls.Add(this.tbxBadge);
            this.gbxBadge.Location = new System.Drawing.Point(54, 12);
            this.gbxBadge.Name = "gbxBadge";
            this.gbxBadge.Size = new System.Drawing.Size(354, 44);
            this.gbxBadge.TabIndex = 16;
            this.gbxBadge.TabStop = false;
            // 
            // tbxBadge
            // 
            this.tbxBadge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxBadge.Location = new System.Drawing.Point(247, 16);
            this.tbxBadge.Mask = "0000000";
            this.tbxBadge.Name = "tbxBadge";
            this.tbxBadge.PromptChar = '#';
            this.tbxBadge.Size = new System.Drawing.Size(100, 22);
            this.tbxBadge.TabIndex = 7;
            this.tbxBadge.Click += new System.EventHandler(this.tbxBadge_Click);
            // 
            // rbBadge
            // 
            this.rbBadge.AutoSize = true;
            this.rbBadge.Checked = true;
            this.rbBadge.Location = new System.Drawing.Point(38, 30);
            this.rbBadge.Name = "rbBadge";
            this.rbBadge.Size = new System.Drawing.Size(100, 17);
            this.rbBadge.TabIndex = 17;
            this.rbBadge.TabStop = true;
            this.rbBadge.Text = "Номер бэйджа";
            this.rbBadge.UseVisualStyleBackColor = true;
            this.rbBadge.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbFIO
            // 
            this.rbFIO.AutoSize = true;
            this.rbFIO.Location = new System.Drawing.Point(38, 78);
            this.rbFIO.Name = "rbFIO";
            this.rbFIO.Size = new System.Drawing.Size(52, 17);
            this.rbFIO.TabIndex = 17;
            this.rbFIO.Text = "ФИО";
            this.rbFIO.UseVisualStyleBackColor = true;
            this.rbFIO.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbPhone
            // 
            this.rbPhone.AutoSize = true;
            this.rbPhone.Location = new System.Drawing.Point(38, 128);
            this.rbPhone.Name = "rbPhone";
            this.rbPhone.Size = new System.Drawing.Size(84, 17);
            this.rbPhone.TabIndex = 17;
            this.rbPhone.Text = "Мобильный";
            this.rbPhone.UseVisualStyleBackColor = true;
            this.rbPhone.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 215);
            this.Controls.Add(this.rbPhone);
            this.Controls.Add(this.rbFIO);
            this.Controls.Add(this.rbBadge);
            this.Controls.Add(this.gbxBadge);
            this.Controls.Add(this.gbxFIO);
            this.Controls.Add(this.gbxPhone);
            this.Controls.Add(this.btnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SearchForm";
            this.Text = "Искать";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchForm_FormClosing);
            this.gbxPhone.ResumeLayout(false);
            this.gbxPhone.PerformLayout();
            this.gbxFIO.ResumeLayout(false);
            this.gbxFIO.PerformLayout();
            this.gbxBadge.ResumeLayout(false);
            this.gbxBadge.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox tbxPhone;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox gbxPhone;
        private System.Windows.Forms.GroupBox gbxFIO;
        private System.Windows.Forms.GroupBox gbxBadge;
        private System.Windows.Forms.RadioButton rbBadge;
        private System.Windows.Forms.RadioButton rbFIO;
        private System.Windows.Forms.RadioButton rbPhone;
        private System.Windows.Forms.TextBox tbxFIO;
        private System.Windows.Forms.MaskedTextBox tbxBadge;
    }
}