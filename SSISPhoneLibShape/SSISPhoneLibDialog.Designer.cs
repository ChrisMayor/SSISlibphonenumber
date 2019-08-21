namespace SSISPhoneLibShape
{
    partial class SSISPhoneLibDialog
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
            this.chkInputColumn = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIso2Default = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkInputColumnISO = new System.Windows.Forms.CheckedListBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkInputColumn
            // 
            this.chkInputColumn.FormattingEnabled = true;
            this.chkInputColumn.Location = new System.Drawing.Point(30, 52);
            this.chkInputColumn.Name = "chkInputColumn";
            this.chkInputColumn.Size = new System.Drawing.Size(268, 364);
            this.chkInputColumn.TabIndex = 0;
            this.chkInputColumn.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkInputColumns_ItemCheck);
            this.chkInputColumn.SelectedIndexChanged += new System.EventHandler(this.ChkInputColumns_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Column (phone number)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(364, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Input column default country (ISO2 code eg. DE)";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(243, 438);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(363, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "will be used if ISO input colum is not selected";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(363, 338);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(239, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Please specify default ISO 2 country code by text";
            // 
            // txtIso2Default
            // 
            this.txtIso2Default.Location = new System.Drawing.Point(482, 396);
            this.txtIso2Default.MaxLength = 2;
            this.txtIso2Default.Name = "txtIso2Default";
            this.txtIso2Default.Size = new System.Drawing.Size(100, 20);
            this.txtIso2Default.TabIndex = 6;
            this.txtIso2Default.Text = "DE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(366, 399);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "ISO 2 default country:";
            // 
            // chkInputColumnISO
            // 
            this.chkInputColumnISO.FormattingEnabled = true;
            this.chkInputColumnISO.Location = new System.Drawing.Point(360, 52);
            this.chkInputColumnISO.Name = "chkInputColumnISO";
            this.chkInputColumnISO.Size = new System.Drawing.Size(256, 274);
            this.chkInputColumnISO.TabIndex = 8;
            this.chkInputColumnISO.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkInputColumnISO_ItemCheck);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(12, 476);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(283, 13);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://www.github.com/ChrisMayor/SSISlibphonenumber";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(325, 438);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(363, 375);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "or as fallback if ISO input is empty";
            // 
            // SSISPhoneLibDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 498);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.chkInputColumnISO);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtIso2Default);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkInputColumn);
            this.Name = "SSISPhoneLibDialog";
            this.Text = "SSIS libphonenumber settings";
            this.Load += new System.EventHandler(this.SSISPhoneLibDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chkInputColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIso2Default;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox chkInputColumnISO;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label6;
    }
}