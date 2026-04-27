using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_Main : Form
    {
        private string role;
        private string username;
        public Form_Main(string role, string username)
        {
            InitializeComponent();
            this.role = role;
            this.username = username;
        }

        //private void Form_Main_Load(object sender, EventArgs e)
        //{
        //    lblWelcome.Text = $"Xin chào {username} - Quyền: {role}";

        //    if (role == "Admin")
        //    {
        //        // Hiển thị thêm chức năng quản trị nếu cần
        //        btnQuanLyHeThong.Visible = true;
        //    }
        //}
        private void Form_Main_Load(object sender, EventArgs e)
        {

        }
        private void LoadForm(Form childForm)
        {
            panelMain.Controls.Clear();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            childForm.Show();
        }

        private void btnQuanLy_Click(object sender, EventArgs e)
        {
            LoadForm(new Form_QuanLy());
        }
        private void btnHocVien_Click(object sender, EventArgs e)
        {
            LoadForm(new Form_Quanlyhocvien());
        }

        private void btnGiaoVien_Click(object sender, EventArgs e)
        {
            LoadForm(new Form_Quanlygiaovien());
        }

        private void btnKhoaHoc_Click(object sender, EventArgs e)
        {
            LoadForm(new Form_Quanlykhoahoc());
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            Form reportForm = new Form();
            CrystalDecisions.Windows.Forms.CrystalReportViewer viewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            viewer.Dock = DockStyle.Fill;
            viewer.ReportSource = new MyReport1(); // hoặc MyReport.rpt
            reportForm.Controls.Add(viewer);
            LoadForm(reportForm);
        }



        private void btnNhanSu_Click(object sender, EventArgs e)
        {
            LoadForm(new Form_Quanlynhansu());
        }

        private void btnKhuyenMai_Click(object sender, EventArgs e)
        {
            LoadForm(new Form_KhuyenMai());
        }
        private void btnDiemDanh_Click(object sender, EventArgs e)
        {
            LoadForm(new Form_GVDiemDanh());
        }



        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            Form reportForm = new Form();
            CrystalDecisions.Windows.Forms.CrystalReportViewer viewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            viewer.Dock = DockStyle.Fill;
            viewer.ReportSource = new MyReport1(); // hoặc MyReport1
            reportForm.Controls.Add(viewer);
            LoadForm(reportForm);
        }

        private void btnQuanlyPQ_Click(object sender, EventArgs e)
        {
            // Kiểm tra quyền trước khi mở form phân quyền
            if (this.role == "Admin")
            {
                LoadForm(new Form_PhanQuyen());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

      
    }
}
