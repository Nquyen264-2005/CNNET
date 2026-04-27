using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_DangNhap : Form
    {
        private readonly string connStr =
            "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        public static string CurrentRole = ""; // Biến toàn cục để lưu phân quyền người dùng
        public Form_DangNhap()
        {
            InitializeComponent();
        }

        
        private void Form_DangNhap_Load(object sender, EventArgs e)
        {
            // Viết logic khởi tạo form ở đây
            // Ví dụ: clear textbox, focus vào txtTenDangNhap
            txtTendangnhap.Clear();
            txtMatkhau.Clear();
            txtTendangnhap.Focus();
        }



        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtTendangnhap.Text.Trim();
            string password = txtMatkhau.Text.Trim();

            // ✅ Kiểm tra trước khi xử lý đăng nhập
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("SP_AuthoLogin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = username;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = password;

                    conn.Open();
                    object scalarResult = cmd.ExecuteScalar();

                    if (scalarResult == null || scalarResult == DBNull.Value)
                    {
                        MessageBox.Show("Stored procedure không trả về giá trị.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int result;
                    if (!int.TryParse(scalarResult.ToString(), out result))
                    {
                        MessageBox.Show("Giá trị trả về không hợp lệ.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    switch (result)
                    {
                        case 0: // Các vai trò khác
                            string role = GetUserRole(username);
                            OpenRoleForm(role, username);
                            this.Hide();
                            break;

                        case 1: // Admin
                            MessageBox.Show("Đăng nhập thành công (Admin)!", "Thông báo");

                            Form_Main mainForm = new Form_Main("Admin", username); // truyền role và username nếu cần
                            mainForm.Show();

                            this.Hide(); // Ẩn form đăng nhập
                            break;                           
                           

                   
                        case 2:
                            MessageBox.Show("Sai mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case 3:
                            MessageBox.Show("Tài khoản không tồn tại!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        default:
                            MessageBox.Show("Kết quả không xác định: " + result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("Lỗi SQL Server: " + sqlex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetUserRole(string username)
        {
            string role = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT MAQUYEN FROM TAIKHOAN WHERE TENDANGNHAP = @u", conn))
                {
                    cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = username;
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        role = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy quyền: " + ex.Message);
            }
            return role;
        }

        private void OpenRoleForm(string role, string username)
        {
            switch (role)
            {
                case "Q0002": // Quản lý
                    MessageBox.Show("Đăng nhập thành công (Quản lý)!");
                    new Form_QuanLy().Show();
                    this.Hide();
                    break;

                case "Q0003": // Giáo viên
                    MessageBox.Show("Đăng nhập thành công (Giáo viên)!");
                    new Form_GiaoVien().Show();
                    this.Hide();
                    break;

                case "Q0004": // Học viên
                    MessageBox.Show("Đăng nhập thành công (Học viên)!");
                    Form_HocVien fHocVien = new Form_HocVien(username);
                    fHocVien.Show();
                    this.Hide();
                    break;

                default:
                    MessageBox.Show("Đăng nhập thành công (Người dùng không rõ quyền)!");
                    break;
            }
        }

        private void cbHienthimatkhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatkhau.UseSystemPasswordChar = !cbHienthimatkhau.Checked;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            new Form_DangKy().Show();
        }
    }
}
