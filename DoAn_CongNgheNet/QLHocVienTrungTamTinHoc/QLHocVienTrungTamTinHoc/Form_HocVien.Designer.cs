namespace QLHocVienTrungTamTinHoc
{
    partial class Form_HocVien
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_HocVien));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridViewKhoahoc = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDoiMatKhau = new System.Windows.Forms.Button();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.groupctHocVien = new System.Windows.Forms.GroupBox();
            this.txtghichu = new System.Windows.Forms.TextBox();
            this.txtdiachi = new System.Windows.Forms.TextBox();
            this.txtngaysinh = new System.Windows.Forms.TextBox();
            this.txtGioiTinh = new System.Windows.Forms.TextBox();
            this.txtMahocvien = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtngaytaotaikhoan = new System.Windows.Forms.TextBox();
            this.btnngaytaotaikhoan = new System.Windows.Forms.Label();
            this.txtquyentruycap = new System.Windows.Forms.TextBox();
            this.txtsodienthoai = new System.Windows.Forms.TextBox();
            this.txtemail = new System.Windows.Forms.TextBox();
            this.txthoten = new System.Windows.Forms.TextBox();
            this.txttendangnhap = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.chartDiemThanhPham = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewKhoahoc)).BeginInit();
            this.groupctHocVien.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDiemThanhPham)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewKhoahoc
            // 
            resources.ApplyResources(this.dataGridViewKhoahoc, "dataGridViewKhoahoc");
            this.dataGridViewKhoahoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewKhoahoc.Name = "dataGridViewKhoahoc";
            this.dataGridViewKhoahoc.Click += new System.EventHandler(this.dataGridViewKhoahoc_SelectionChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnDoiMatKhau
            // 
            resources.ApplyResources(this.btnDoiMatKhau, "btnDoiMatKhau");
            this.btnDoiMatKhau.Name = "btnDoiMatKhau";
            this.btnDoiMatKhau.UseVisualStyleBackColor = true;
            this.btnDoiMatKhau.Click += new System.EventHandler(this.btnDoiMatKhau_Click);
            // 
            // btnDangXuat
            // 
            resources.ApplyResources(this.btnDangXuat, "btnDangXuat");
            this.btnDangXuat.BackColor = System.Drawing.Color.Coral;
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.UseVisualStyleBackColor = false;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // groupctHocVien
            // 
            resources.ApplyResources(this.groupctHocVien, "groupctHocVien");
            this.groupctHocVien.Controls.Add(this.txtghichu);
            this.groupctHocVien.Controls.Add(this.txtdiachi);
            this.groupctHocVien.Controls.Add(this.txtngaysinh);
            this.groupctHocVien.Controls.Add(this.txtGioiTinh);
            this.groupctHocVien.Controls.Add(this.txtMahocvien);
            this.groupctHocVien.Controls.Add(this.label6);
            this.groupctHocVien.Controls.Add(this.label5);
            this.groupctHocVien.Controls.Add(this.label4);
            this.groupctHocVien.Controls.Add(this.label3);
            this.groupctHocVien.Controls.Add(this.label2);
            this.groupctHocVien.Name = "groupctHocVien";
            this.groupctHocVien.TabStop = false;
            // 
            // txtghichu
            // 
            resources.ApplyResources(this.txtghichu, "txtghichu");
            this.txtghichu.Name = "txtghichu";
            // 
            // txtdiachi
            // 
            resources.ApplyResources(this.txtdiachi, "txtdiachi");
            this.txtdiachi.Name = "txtdiachi";
            // 
            // txtngaysinh
            // 
            resources.ApplyResources(this.txtngaysinh, "txtngaysinh");
            this.txtngaysinh.Name = "txtngaysinh";
            // 
            // txtGioiTinh
            // 
            resources.ApplyResources(this.txtGioiTinh, "txtGioiTinh");
            this.txtGioiTinh.Name = "txtGioiTinh";
            // 
            // txtMahocvien
            // 
            resources.ApplyResources(this.txtMahocvien, "txtMahocvien");
            this.txtMahocvien.Name = "txtMahocvien";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.txtngaytaotaikhoan);
            this.groupBox1.Controls.Add(this.btnngaytaotaikhoan);
            this.groupBox1.Controls.Add(this.txtquyentruycap);
            this.groupBox1.Controls.Add(this.txtsodienthoai);
            this.groupBox1.Controls.Add(this.txtemail);
            this.groupBox1.Controls.Add(this.txthoten);
            this.groupBox1.Controls.Add(this.txttendangnhap);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtngaytaotaikhoan
            // 
            resources.ApplyResources(this.txtngaytaotaikhoan, "txtngaytaotaikhoan");
            this.txtngaytaotaikhoan.Name = "txtngaytaotaikhoan";
            // 
            // btnngaytaotaikhoan
            // 
            resources.ApplyResources(this.btnngaytaotaikhoan, "btnngaytaotaikhoan");
            this.btnngaytaotaikhoan.Name = "btnngaytaotaikhoan";
            // 
            // txtquyentruycap
            // 
            resources.ApplyResources(this.txtquyentruycap, "txtquyentruycap");
            this.txtquyentruycap.Name = "txtquyentruycap";
            // 
            // txtsodienthoai
            // 
            resources.ApplyResources(this.txtsodienthoai, "txtsodienthoai");
            this.txtsodienthoai.Name = "txtsodienthoai";
            // 
            // txtemail
            // 
            resources.ApplyResources(this.txtemail, "txtemail");
            this.txtemail.Name = "txtemail";
            // 
            // txthoten
            // 
            resources.ApplyResources(this.txthoten, "txthoten");
            this.txthoten.Name = "txthoten";
            // 
            // txttendangnhap
            // 
            resources.ApplyResources(this.txttendangnhap, "txttendangnhap");
            this.txttendangnhap.Name = "txttendangnhap";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // btnSua
            // 
            resources.ApplyResources(this.btnSua, "btnSua");
            this.btnSua.Name = "btnSua";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnCapNhat
            // 
            resources.ApplyResources(this.btnCapNhat, "btnCapNhat");
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.UseVisualStyleBackColor = true;
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
            // 
            // chartDiemThanhPham
            // 
            resources.ApplyResources(this.chartDiemThanhPham, "chartDiemThanhPham");
            chartArea1.Name = "ChartArea1";
            this.chartDiemThanhPham.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartDiemThanhPham.Legends.Add(legend1);
            this.chartDiemThanhPham.Name = "chartDiemThanhPham";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartDiemThanhPham.Series.Add(series1);
            this.chartDiemThanhPham.Click += new System.EventHandler(this.Form_HocVien_Load);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.Name = "label13";
            // 
            // Form_HocVien
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label13);
            this.Controls.Add(this.chartDiemThanhPham);
            this.Controls.Add(this.btnCapNhat);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupctHocVien);
            this.Controls.Add(this.btnDangXuat);
            this.Controls.Add(this.btnDoiMatKhau);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewKhoahoc);
            this.Name = "Form_HocVien";
            this.Load += new System.EventHandler(this.Form_HocVien_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewKhoahoc)).EndInit();
            this.groupctHocVien.ResumeLayout(false);
            this.groupctHocVien.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDiemThanhPham)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewKhoahoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDoiMatKhau;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.GroupBox groupctHocVien;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtghichu;
        private System.Windows.Forms.TextBox txtdiachi;
        private System.Windows.Forms.TextBox txtngaysinh;
        private System.Windows.Forms.TextBox txtGioiTinh;
        private System.Windows.Forms.TextBox txtMahocvien;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtngaytaotaikhoan;
        private System.Windows.Forms.Label btnngaytaotaikhoan;
        private System.Windows.Forms.TextBox txtquyentruycap;
        private System.Windows.Forms.TextBox txtsodienthoai;
        private System.Windows.Forms.TextBox txtemail;
        private System.Windows.Forms.TextBox txthoten;
        private System.Windows.Forms.TextBox txttendangnhap;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDiemThanhPham;
        private System.Windows.Forms.Label label13;
    }
}