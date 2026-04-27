
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_PhanQuyen : Form
    {
        private string connectionString =
            "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        // Khai báo biến toàn cục để ánh xạ TENQUYEN → MAQUYEN
        private Dictionary<string, string> quyenMap = new Dictionary<string, string>();

         private string username;

        public Form_PhanQuyen(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        //private void Form_Admin_Load(object sender, EventArgs e)
        //{
        //    lblTenAdmin.Text = $"Xin chào {username}";
        //}

        public Form_PhanQuyen()
        {
            InitializeComponent();
        }
      

        private void Form_Admin_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MAQUYEN, TENQUYEN FROM QUYENNGUOIDUNG";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string maQuyen = reader["MAQUYEN"].ToString();
                    string tenQuyen = reader["TENQUYEN"].ToString();

                    cbRole.Items.Add(tenQuyen);         // Hiển thị tên quyền
                    quyenMap[tenQuyen] = maQuyen;       // Ánh xạ tên → mã
                }
            }

                if (cbRole.Items.Count > 0)
                    cbRole.SelectedIndex = 0;
            


            if (cbRole.Items.Count > 0)
                cbRole.SelectedIndex = 0;

            // Chỉ Admin mới được tạo tài khoản
            if (Form_DangNhap.CurrentRole != "Admin")
            {
                btnTaoTaiKhoan.Enabled = true;
            }
        }

        // === SAO LƯU CSDL ===
        private void btnChonBackup_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Backup files (*.bak)|*.bak";
            sfd.Title = "Chọn nơi lưu file sao lưu";
            sfd.FileName = "QLHocVienTinHoc_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".bak";

            if (sfd.ShowDialog() == DialogResult.OK)
                txtLuuTruBackup.Text = sfd.FileName;
        }

        private void btnThucHienBackup_Click(object sender, EventArgs e)
        {
            string path = txtLuuTruBackup.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Vui lòng chọn nơi lưu file .bak", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string backupQuery = "BACKUP DATABASE QLHocVienTinHoc TO DISK = N'{path}'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(backupQuery, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sao lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sao lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // === KHÔI PHỤC CSDL ===
        private void btnChonRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Backup files (*.bak)|*.bak";
            ofd.Title = "Chọn file .bak để khôi phục";

            if (ofd.ShowDialog() == DialogResult.OK)
                txtDuongDanRestore.Text = ofd.FileName;
        }

        private void btnThucHienRestore_Click(object sender, EventArgs e)
        {
            string path = txtDuongDanRestore.Text.Trim();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                MessageBox.Show("Vui lòng chọn file .bak hợp lệ", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string restoreQuery = @"
                ALTER DATABASE QLHocVienTinHoc SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE QLHocVienTinHoc FROM DISK = N'{path}' WITH REPLACE;
                ALTER DATABASE QLHocVienTinHoc SET MULTI_USER;
                ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(restoreQuery, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Khôi phục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khôi phục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //tạo tài khoản
        private void btnTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            string maTK = txtMaTaiKhoan.Text.Trim();
            string login = txtLogin.Text.Trim();
            string matkhau = txtMatKhau.Text.Trim();
            string hoten = txtTenUser.Text.Trim();
            string tenQuyen = cbRole.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(maTK) || string.IsNullOrEmpty(login) ||
                string.IsNullOrEmpty(matkhau) || string.IsNullOrEmpty(hoten) || string.IsNullOrEmpty(tenQuyen))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy mã quyền từ ánh xạ tên quyền
            string maQuyen = quyenMap.ContainsKey(tenQuyen) ? quyenMap[tenQuyen] : "";
            if (string.IsNullOrEmpty(maQuyen))
            {
                MessageBox.Show("Không tìm thấy mã quyền tương ứng!", "Lỗi phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra trùng tên đăng nhập
                    string checkQuery = "SELECT COUNT(*) FROM TAIKHOAN WHERE TENDANGNHAP = @login";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@login", login);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Thêm tài khoản mới
                    string insertQuery = @"INSERT INTO TAIKHOAN (MATAIKHOAN, TENDANGNHAP, MATKHAU, HOTEN, MAQUYEN)
                                   VALUES (@maTK, @login, @matkhau, @hoten, @maQuyen)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTK", maTK);
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@matkhau", matkhau);
                        cmd.Parameters.AddWithValue("@hoten", hoten);
                        cmd.Parameters.AddWithValue("@maQuyen", maQuyen);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Tạo tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // === ĐĂNG XUẤT ===
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            Form_DangNhap f = new Form_DangNhap();
            f.Show();
            this.Close();
        }
    }
}
