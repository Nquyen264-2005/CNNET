namespace QLHocVienTrungTamTinHoc
{
    partial class Form_GiaoVien
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtdiem = new System.Windows.Forms.TextBox();
            this.txtmabaikiemtra = new System.Windows.Forms.TextBox();
            this.txtmaghidanh = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.dataGridViewdiem = new System.Windows.Forms.DataGridView();
            this.chartDiem = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnThemDiem = new System.Windows.Forms.Button();
            this.btnXoadiem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnDiemdanh = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewdiem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDiem)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtdiem);
            this.groupBox1.Controls.Add(this.txtmabaikiemtra);
            this.groupBox1.Controls.Add(this.txtmaghidanh);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(41, 58);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(404, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Điểm";
            // 
            // txtdiem
            // 
            this.txtdiem.Location = new System.Drawing.Point(113, 90);
            this.txtdiem.Margin = new System.Windows.Forms.Padding(4);
            this.txtdiem.Name = "txtdiem";
            this.txtdiem.Size = new System.Drawing.Size(132, 22);
            this.txtdiem.TabIndex = 5;
            // 
            // txtmabaikiemtra
            // 
            this.txtmabaikiemtra.Location = new System.Drawing.Point(113, 58);
            this.txtmabaikiemtra.Margin = new System.Windows.Forms.Padding(4);
            this.txtmabaikiemtra.Name = "txtmabaikiemtra";
            this.txtmabaikiemtra.Size = new System.Drawing.Size(132, 22);
            this.txtmabaikiemtra.TabIndex = 4;
            // 
            // txtmaghidanh
            // 
            this.txtmaghidanh.Location = new System.Drawing.Point(113, 21);
            this.txtmaghidanh.Margin = new System.Windows.Forms.Padding(4);
            this.txtmaghidanh.Name = "txtmaghidanh";
            this.txtmaghidanh.Size = new System.Drawing.Size(132, 22);
            this.txtmaghidanh.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 92);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Điểm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mã bài kiểm tra";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã ghi danh";
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.BackColor = System.Drawing.Color.Coral;
            this.btnDangXuat.Location = new System.Drawing.Point(954, 13);
            this.btnDangXuat.Margin = new System.Windows.Forms.Padding(4);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(100, 28);
            this.btnDangXuat.TabIndex = 1;
            this.btnDangXuat.Text = "Đăng Xuất";
            this.btnDangXuat.UseVisualStyleBackColor = false;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // dataGridViewdiem
            // 
            this.dataGridViewdiem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewdiem.Location = new System.Drawing.Point(42, 274);
            this.dataGridViewdiem.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewdiem.Name = "dataGridViewdiem";
            this.dataGridViewdiem.RowHeadersWidth = 51;
            this.dataGridViewdiem.Size = new System.Drawing.Size(945, 267);
            this.dataGridViewdiem.TabIndex = 2;
            this.dataGridViewdiem.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewdiem_CellClick);
            // 
            // chartDiem
            // 
            chartArea2.Name = "ChartArea1";
            this.chartDiem.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartDiem.Legends.Add(legend2);
            this.chartDiem.Location = new System.Drawing.Point(656, 58);
            this.chartDiem.Margin = new System.Windows.Forms.Padding(4);
            this.chartDiem.Name = "chartDiem";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartDiem.Series.Add(series2);
            this.chartDiem.Size = new System.Drawing.Size(331, 186);
            this.chartDiem.TabIndex = 3;
            this.chartDiem.Text = "chart1";
            // 
            // btnThemDiem
            // 
            this.btnThemDiem.Location = new System.Drawing.Point(453, 58);
            this.btnThemDiem.Margin = new System.Windows.Forms.Padding(4);
            this.btnThemDiem.Name = "btnThemDiem";
            this.btnThemDiem.Size = new System.Drawing.Size(100, 28);
            this.btnThemDiem.TabIndex = 4;
            this.btnThemDiem.Text = "Thêm";
            this.btnThemDiem.UseVisualStyleBackColor = true;
            this.btnThemDiem.Click += new System.EventHandler(this.btnThemDiem_Click);
            // 
            // btnXoadiem
            // 
            this.btnXoadiem.Location = new System.Drawing.Point(453, 103);
            this.btnXoadiem.Margin = new System.Windows.Forms.Padding(4);
            this.btnXoadiem.Name = "btnXoadiem";
            this.btnXoadiem.Size = new System.Drawing.Size(100, 28);
            this.btnXoadiem.TabIndex = 5;
            this.btnXoadiem.Text = "Xóa";
            this.btnXoadiem.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(453, 144);
            this.btnSua.Margin = new System.Windows.Forms.Padding(4);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(100, 28);
            this.btnSua.TabIndex = 6;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            // 
            // btnDiemdanh
            // 
            this.btnDiemdanh.Location = new System.Drawing.Point(665, 13);
            this.btnDiemdanh.Margin = new System.Windows.Forms.Padding(4);
            this.btnDiemdanh.Name = "btnDiemdanh";
            this.btnDiemdanh.Size = new System.Drawing.Size(100, 28);
            this.btnDiemdanh.TabIndex = 7;
            this.btnDiemdanh.Text = "Điểm Danh";
            this.btnDiemdanh.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(337, 6);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 32);
            this.label4.TabIndex = 8;
            this.label4.Text = "GIÁO VIÊN";
            // 
            // Form_GiaoVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnDiemdanh);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnXoadiem);
            this.Controls.Add(this.btnThemDiem);
            this.Controls.Add(this.chartDiem);
            this.Controls.Add(this.dataGridViewdiem);
            this.Controls.Add(this.btnDangXuat);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_GiaoVien";
            this.Text = "Form_GiaoVien";
            this.Load += new System.EventHandler(this.Form_GiaoVien_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewdiem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDiem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtmaghidanh;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.TextBox txtdiem;
        private System.Windows.Forms.TextBox txtmabaikiemtra;
        private System.Windows.Forms.DataGridView dataGridViewdiem;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDiem;
        private System.Windows.Forms.Button btnThemDiem;
        private System.Windows.Forms.Button btnXoadiem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnDiemdanh;
        private System.Windows.Forms.Label label4;
    }
}