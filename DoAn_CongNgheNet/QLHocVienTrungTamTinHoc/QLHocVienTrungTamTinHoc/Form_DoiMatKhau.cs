using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_DoiMatKhau : Form
    {
        // Chuỗi kết nối, nên được lấy từ file cấu hình (App.config) hoặc một lớp chung (Utility Class)
        // Tuy nhiên, để đơn giản, ta giữ nguyên chuỗi kết nối như trong Form_DangNhap
        private readonly string connStr =
            @"Data Source=LAPTOP-OH9DSGKI\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";
        // Thêm biến thành viên
        private string userName;

        // Thêm Constructor nhận tham số để nhận tên đăng nhập và sửa lỗi gạch chân
        public Form_DoiMatKhau(string username)
        {
            InitializeComponent();
            this.userName = username;

            // Giả sử bạn có TextBox tên là txtTenDangNhap trên Form Đổi Mật Khẩu
            txtTendangnhap.Text = username;
            txtTendangnhap.ReadOnly = true;
        }

        // Nếu bạn cần Constructor không tham số cho mục đích thiết kế, hãy giữ lại nó
        public Form_DoiMatKhau()
        {
            InitializeComponent();
        }
        private void Form_DoiMatKhau_Load(object sender, EventArgs e)
        {
            // Thiết lập mật khẩu cũ và mới ở chế độ ẩn
            txtMatkhaucu.UseSystemPasswordChar = true;
            txtMatkhaumoi.UseSystemPasswordChar = true;

            // Đặt focus vào ô Tên đăng nhập
            txtTendangnhap.Focus();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            string username = txtTendangnhap.Text.Trim();
            string oldPassword = txtMatkhaucu.Text.Trim();
            string newPassword = txtMatkhaumoi.Text.Trim();

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (username.Length > 100 || oldPassword.Length > 100 || newPassword.Length > 100)
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu vượt quá 100 ký tự.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("SP_ChangePassword", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar, 100).Value = username;
                    cmd.Parameters.Add("@MatKhauCu", SqlDbType.NVarChar, 100).Value = oldPassword;
                    cmd.Parameters.Add("@MatKhauMoi", SqlDbType.NVarChar, 100).Value = newPassword;

                    conn.Open();
                    object scalarResult = cmd.ExecuteScalar();

                    int result;
                    if (scalarResult == null || !int.TryParse(scalarResult.ToString(), out result))
                    {
                        MessageBox.Show("Stored procedure không trả về kết quả hợp lệ.", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string msg;
                    switch (result)
                    {
                        case 0: msg = "Đổi mật khẩu thành công!"; this.Close(); break;
                        case 1: msg = "Tên đăng nhập không tồn tại!"; txtTendangnhap.Focus(); break;
                        case 2: msg = "Mật khẩu cũ không chính xác hoặc tài khoản bị khóa!"; txtMatkhaucu.Focus(); break;
                        case 3: msg = "Mật khẩu mới phải khác mật khẩu cũ!"; txtMatkhaumoi.Focus(); break;
                        default: msg = "Lỗi không xác định. Mã lỗi: " + result; break;
                    }
                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK,
                        result == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống/SQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbHienthimatkhau_CheckedChanged(object sender, EventArgs e)
        {
            // Chức năng hiện/ẩn mật khẩu (giả định bạn đã đặt tên checkbox là cbHienthimatkhau)
            if (cbHienthimatkhau.Checked)
            {
                txtMatkhaucu.UseSystemPasswordChar = false;
                txtMatkhaumoi.UseSystemPasswordChar = false;
            }
            else
            {
                txtMatkhaucu.UseSystemPasswordChar = true;
                txtMatkhaumoi.UseSystemPasswordChar = true;
            }
        }
    }
}