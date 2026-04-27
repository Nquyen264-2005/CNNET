namespace QLHocVienTrungTamTinHoc
{
    partial class Form_QuanLy
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
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btntimkiemtuyendung = new System.Windows.Forms.Button();
            this.btnxoatuyendung = new System.Windows.Forms.Button();
            this.btnthemtuyendung = new System.Windows.Forms.Button();
            this.txttimkiemtuyendung = new System.Windows.Forms.TextBox();
            this.txtmatuyendung = new System.Windows.Forms.TextBox();
            this.txtmanhansu = new System.Windows.Forms.TextBox();
            this.txtmota = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.maskedTextBoxngaytuyendung = new System.Windows.Forms.MaskedTextBox();
            this.btnsuatuyendung = new System.Windows.Forms.Button();
            this.dataGridViewtuyendung = new System.Windows.Forms.DataGridView();
            this.btnNhapMoi = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewtuyendung)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.BackColor = System.Drawing.Color.Coral;
            this.btnDangXuat.Location = new System.Drawing.Point(911, 15);
            this.btnDangXuat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(100, 28);
            this.btnDangXuat.TabIndex = 0;
            this.btnDangXuat.Text = "Đăng Xuất";
            this.btnDangXuat.UseVisualStyleBackColor = false;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mã tuyển dụng";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 65);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Mã nhân sự";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 94);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Ngày tuyển dụng";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 126);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Mô tả";
            // 
            // btntimkiemtuyendung
            // 
            this.btntimkiemtuyendung.Location = new System.Drawing.Point(752, 52);
            this.btntimkiemtuyendung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btntimkiemtuyendung.Name = "btntimkiemtuyendung";
            this.btntimkiemtuyendung.Size = new System.Drawing.Size(100, 28);
            this.btntimkiemtuyendung.TabIndex = 7;
            this.btntimkiemtuyendung.Text = "Tìm kiếm";
            this.btntimkiemtuyendung.UseVisualStyleBackColor = true;
            this.btntimkiemtuyendung.Click += new System.EventHandler(this.btntimkiemtuyendung_Click);
            // 
            // btnxoatuyendung
            // 
            this.btnxoatuyendung.Location = new System.Drawing.Point(752, 143);
            this.btnxoatuyendung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnxoatuyendung.Name = "btnxoatuyendung";
            this.btnxoatuyendung.Size = new System.Drawing.Size(100, 28);
            this.btnxoatuyendung.TabIndex = 8;
            this.btnxoatuyendung.Text = "Xóa";
            this.btnxoatuyendung.UseVisualStyleBackColor = true;
            this.btnxoatuyendung.Click += new System.EventHandler(this.btnxoatuyendung_Click);
            // 
            // btnthemtuyendung
            // 
            this.btnthemtuyendung.Location = new System.Drawing.Point(752, 102);
            this.btnthemtuyendung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnthemtuyendung.Name = "btnthemtuyendung";
            this.btnthemtuyendung.Size = new System.Drawing.Size(100, 28);
            this.btnthemtuyendung.TabIndex = 9;
            this.btnthemtuyendung.Text = "Thêm";
            this.btnthemtuyendung.UseVisualStyleBackColor = true;
            this.btnthemtuyendung.Click += new System.EventHandler(this.btnthemtuyendung_Click);
            // 
            // txttimkiemtuyendung
            // 
            this.txttimkiemtuyendung.Location = new System.Drawing.Point(580, 52);
            this.txttimkiemtuyendung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txttimkiemtuyendung.Name = "txttimkiemtuyendung";
            this.txttimkiemtuyendung.Size = new System.Drawing.Size(132, 22);
            this.txttimkiemtuyendung.TabIndex = 10;
            // 
            // txtmatuyendung
            // 
            this.txtmatuyendung.Location = new System.Drawing.Point(139, 23);
            this.txtmatuyendung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtmatuyendung.Name = "txtmatuyendung";
            this.txtmatuyendung.Size = new System.Drawing.Size(323, 22);
            this.txtmatuyendung.TabIndex = 11;
            // 
            // txtmanhansu
            // 
            this.txtmanhansu.Location = new System.Drawing.Point(139, 62);
            this.txtmanhansu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtmanhansu.Name = "txtmanhansu";
            this.txtmanhansu.Size = new System.Drawing.Size(323, 22);
            this.txtmanhansu.TabIndex = 12;
            // 
            // txtmota
            // 
            this.txtmota.Location = new System.Drawing.Point(139, 122);
            this.txtmota.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtmota.Name = "txtmota";
            this.txtmota.Size = new System.Drawing.Size(323, 22);
            this.txtmota.TabIndex = 14;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.maskedTextBoxngaytuyendung);
            this.groupBox2.Controls.Add(this.txtmatuyendung);
            this.groupBox2.Controls.Add(this.txtmota);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtmanhansu);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(44, 48);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(495, 168);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tuyển dụng";
            // 
            // maskedTextBoxngaytuyendung
            // 
            this.maskedTextBoxngaytuyendung.Location = new System.Drawing.Point(140, 95);
            this.maskedTextBoxngaytuyendung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.maskedTextBoxngaytuyendung.Mask = "00/00/0000";
            this.maskedTextBoxngaytuyendung.Name = "maskedTextBoxngaytuyendung";
            this.maskedTextBoxngaytuyendung.Size = new System.Drawing.Size(321, 22);
            this.maskedTextBoxngaytuyendung.TabIndex = 15;
            this.maskedTextBoxngaytuyendung.ValidatingType = typeof(System.DateTime);
            // 
            // btnsuatuyendung
            // 
            this.btnsuatuyendung.Location = new System.Drawing.Point(752, 186);
            this.btnsuatuyendung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnsuatuyendung.Name = "btnsuatuyendung";
            this.btnsuatuyendung.Size = new System.Drawing.Size(100, 28);
            this.btnsuatuyendung.TabIndex = 16;
            this.btnsuatuyendung.Text = "Sửa";
            this.btnsuatuyendung.UseVisualStyleBackColor = true;
            this.btnsuatuyendung.Click += new System.EventHandler(this.btnsuatuyendung_Click);
            // 
            // dataGridViewtuyendung
            // 
            this.dataGridViewtuyendung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewtuyendung.Location = new System.Drawing.Point(44, 267);
            this.dataGridViewtuyendung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewtuyendung.Name = "dataGridViewtuyendung";
            this.dataGridViewtuyendung.Size = new System.Drawing.Size(808, 251);
            this.dataGridViewtuyendung.TabIndex = 17;
            // 
            // btnNhapMoi
            // 
            this.btnNhapMoi.Location = new System.Drawing.Point(752, 222);
            this.btnNhapMoi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNhapMoi.Name = "btnNhapMoi";
            this.btnNhapMoi.Size = new System.Drawing.Size(100, 28);
            this.btnNhapMoi.TabIndex = 18;
            this.btnNhapMoi.Text = "Cập nhật";
            this.btnNhapMoi.UseVisualStyleBackColor = true;
            this.btnNhapMoi.Click += new System.EventHandler(this.btnNhapMoi_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(344, 9);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(195, 31);
            this.label13.TabIndex = 50;
            this.label13.Text = "TUYỂN DỤNG";
            // 
            // Form_QuanLy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 553);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnNhapMoi);
            this.Controls.Add(this.dataGridViewtuyendung);
            this.Controls.Add(this.btnsuatuyendung);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txttimkiemtuyendung);
            this.Controls.Add(this.btnthemtuyendung);
            this.Controls.Add(this.btnxoatuyendung);
            this.Controls.Add(this.btntimkiemtuyendung);
            this.Controls.Add(this.btnDangXuat);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form_QuanLy";
            this.Text = "Quản Lý";
            this.Load += new System.EventHandler(this.Form_QuanLy_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewtuyendung)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btntimkiemtuyendung;
        private System.Windows.Forms.Button btnxoatuyendung;
        private System.Windows.Forms.Button btnthemtuyendung;
        private System.Windows.Forms.TextBox txttimkiemtuyendung;
        private System.Windows.Forms.TextBox txtmatuyendung;
        private System.Windows.Forms.TextBox txtmanhansu;
        private System.Windows.Forms.TextBox txtmota;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnsuatuyendung;
        private System.Windows.Forms.DataGridView dataGridViewtuyendung;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxngaytuyendung;
        private System.Windows.Forms.Button btnNhapMoi;
        private System.Windows.Forms.Label label13;
    }
}