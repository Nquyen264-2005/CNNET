using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions; // Thêm thư viện này để kiểm tra định dạng Email

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_DangKy : Form
    {
        // Chuỗi kết nối (Cần thay đổi nếu Data Source khác)
        // **LƯU Ý:** Đảm bảo chuỗi kết nối này khớp với chuỗi trong Form_DangNhap.cs
        private readonly string connStr =
            @"Data Source=LAPTOP-OH9DSGKI\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        // Khai báo Form_DangNhap để có thể hiển thị lại khi cần
        private Form_DangNhap _formDangNhap;
        private int result;

        // ============================
        // 🏗️ CONSTRUCTOR
        // ============================

        // Constructor nhận Form_DangNhap để liên kết (sử dụng khi mở từ Form Đăng nhập)
        public Form_DangKy(Form_DangNhap formDangNhap)
        {
            InitializeComponent();
            _formDangNhap = formDangNhap;
        }

        // Constructor mặc định
        public Form_DangKy()
        {
            InitializeComponent();
        }

        // ============================
        // 🚀 SỰ KIỆN LOAD FORM
        // ============================
        private void Form1_DangKycs_Load(object sender, EventArgs e)
        {
            // Ẩn mật khẩu mặc định khi form load
            txtMatkhau.UseSystemPasswordChar = true;
            txtNhaplaimatkhau.UseSystemPasswordChar = true;
        }

        // ============================
        // 👁️ HIỆN / ẨN MẬT KHẨU (cbHienthimatkhau)
        // ============================
        private void cbHienthimatkhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatkhau.UseSystemPasswordChar = !cbHienthimatkhau.Checked;
            txtNhaplaimatkhau.UseSystemPasswordChar = !cbHienthimatkhau.Checked;
        }

        // ============================
        // 🔐 NÚT ĐĂNG KÝ (btnDangky)
        // ============================
        private void btnDangky_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các controls
            string hoTen = txtHovaten.Text.Trim();
            string email = txtEmail.Text.Trim();
            string sdt = txtSodienthoai.Text.Trim();
            string taiKhoan = txtTendangNhap.Text.Trim();
            string matKhau = txtMatkhau.Text;
            string nhapLaiMK = txtNhaplaimatkhau.Text;

            // 1. Xác thực đầu vào
            if (!ValidateInput(hoTen, email, sdt, taiKhoan, matKhau, nhapLaiMK))
            {
                return;
            }

            // 2. Thực hiện đăng ký bằng Stored Procedure
            ExecuteRegistrationWithSP(hoTen, email, sdt, taiKhoan, matKhau);
        }

        // ============================
        // ➡️ NÚT ĐĂNG NHẬP NGAY (btnDangnhapngay)
        // ============================
        private void btnDangnhapngay_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form Đăng ký

            // Hiển thị lại Form Đăng nhập
            if (_formDangNhap != null)
            {
                _formDangNhap.Show();
            }
        }

        // ============================
        // 🛠️ HÀM HỖ TRỢ XÁC THỰC
        // ============================

        private bool ValidateInput(string hoTen, string email, string sdt, string taiKhoan, string matKhau, string nhapLaiMK)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(taiKhoan) ||
                string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(nhapLaiMK) ||
                string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc!", "Lỗi xác thực",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kiểm tra mật khẩu khớp
            if (matKhau != nhapLaiMK)
            {
                MessageBox.Show("Mật khẩu và Nhập lại mật khẩu không khớp!", "Lỗi xác thực",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kiểm tra độ dài mật khẩu tối thiểu
            if (matKhau.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Lỗi xác thực",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kiểm tra định dạng Email (sử dụng Regex cơ bản)
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Địa chỉ Email không hợp lệ!", "Lỗi xác thực",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ============================
        // 💾 HÀM THỰC HIỆN ĐĂNG KÝ BẰNG SP
        // ============================
        private void ExecuteRegistrationWithSP(string hoTen, string email, string sdt, string taiKhoan, string matKhau)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("SP_RegisterNewStudent", conn))
                {
                    // Thiết lập CommandType là StoredProcedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Thêm các tham số (Tham số phải khớp với SP trong SQL)
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 200).Value = hoTen;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 200).Value = email;
                    cmd.Parameters.Add("@SDT", SqlDbType.Char, 10).Value = sdt;
                    cmd.Parameters.Add("@TaiKhoan", SqlDbType.NVarChar, 100).Value = taiKhoan;
                    cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = matKhau;

                    conn.Open();
                    object scalarResult = cmd.ExecuteScalar();

                    if (scalarResult == null || scalarResult == DBNull.Value ||
                    !int.TryParse(scalarResult.ToString(), out result))
                    {
                        // Nếu kiểm tra thất bại, sử dụng lại logic thông báo lỗi của bạn
                        MessageBox.Show("Lỗi: Stored procedure không trả về mã kết quả hợp lệ.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    // Xử lý mã kết quả từ SP (0=Success, 1=Username exists, 2=Email exists)
                    switch (result)
                    {
                        case 0:
                            MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập ngay.",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Dọn dẹp form và chuyển về form Đăng nhập
                            txtHovaten.Clear();
                            txtEmail.Clear();
                            txtSodienthoai.Clear();
                            txtTendangNhap.Clear();
                            txtMatkhau.Clear();
                            txtNhaplaimatkhau.Clear();

                            this.Close();
                            if (_formDangNhap != null) _formDangNhap.Show();
                            break;
                        case 1:
                            MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.",
                                "Lỗi Đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case 2:
                            MessageBox.Show("Email đã được sử dụng cho một tài khoản khác.",
                                "Lỗi Đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        default:
                            MessageBox.Show("Lỗi đăng ký không xác định (Mã: {result}).",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("Lỗi kết nối SQL Server hoặc lỗi Stored Procedure: " + sqlex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}