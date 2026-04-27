using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLHocVienTrungTamTinHoc
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmReportHS1());
           // Application.Run(new frmReportHS());
           //Application.Run(new Form_Admin());
           Application.Run(new Form_DangNhap());
           //Application.Run(new Form_DangKy());
           //Application.Run(new Form_QuanLy());
           //Application.Run(new Form_Admin());
          // Application.Run(new Form_Main());
           //Application.Run(new Form_KhuyenMai());


        }
    }
}
