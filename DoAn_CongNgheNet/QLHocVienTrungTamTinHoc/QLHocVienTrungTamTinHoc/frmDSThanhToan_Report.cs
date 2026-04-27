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
    public partial class frmReportHS1 : Form
    {
        public frmReportHS1()
        {
            InitializeComponent();
        }
        private void frmReportHS1_Load(object sender, EventArgs e)
        {
            MyReport1 rpt = new MyReport1();
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.DisplayStatusBar = false;
            crystalReportViewer1.DisplayToolbar = true;
            crystalReportViewer1.Refresh();
        }

        private void btn_Show_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng báo cáo từ file .rpt đã thiết kế
            MyReport1 rpt = new MyReport1();

            // Gán báo cáo vào CrystalReportViewer để hiển thị
            crystalReportViewer1.ReportSource = rpt;

            // Thiết lập thông tin đăng nhập CSDL để không bị yêu cầu nhập tay
            rpt.SetDatabaseLogon("sa", "123", "LAPTOP-OH9DSGKI\\SQLPROD2012", "QLHocVienTinHoc");

            // Làm mới viewer và ẩn thanh công cụ + thanh trạng thái
            crystalReportViewer1.Refresh();
            crystalReportViewer1.DisplayToolbar = false;
            crystalReportViewer1.DisplayStatusBar = false;
        }
    }
}
