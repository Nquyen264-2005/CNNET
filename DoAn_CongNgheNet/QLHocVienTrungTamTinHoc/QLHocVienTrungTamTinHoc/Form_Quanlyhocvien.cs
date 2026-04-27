using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_Quanlyhocvien : Form
    {
        // Chuỗi kết nối CSDL (Kiểm tra lại tên Server của bạn)
        private const string connectionString = "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        // Sử dụng DataGridView mặc định (Giả định là dataGridViewhocvien)
        // Các Controls: txtmahocvien, txtmataikhoan, txtHoten, comboBoxgioitinhhocvien, 
        // maskedTextBoxngaysinh, txtemail, txtsodienthoai, txtdiachi, 
        // btnthemhocvien, btnxoahocvien, btnsuahocvien, btnCapnhatlaibanghocvien, 
        // txttimkiemhocvien, btntimkiemhocvien, btntuyendung, btnDangXuat, v.v.

        private bool isEditing = false;

        public Form_Quanlyhocvien()
        {
            InitializeComponent();
            InitializeCustomEvents();
        }

        private void InitializeCustomEvents()
        {
            this.Load += new EventHandler(Form_Quanlyhocvien_Load);
            dataGridViewhocvien.CellClick += new DataGridViewCellEventHandler(dataGridViewhocvien_CellClick);
            btnthemhocvien.Click += new EventHandler(btnthemhocvien_Click);
            btnsuahocvien.Click += new EventHandler(btnsuahocvien_Click);
            btnxoahocvien.Click += new EventHandler(btnxoahocvien_Click);
            btntimkiemhocvien.Click += new EventHandler(btntimkiemhocvien_Click);
            btnCapnhatlaibanghocvien.Click += new EventHandler(btnCapnhatlaibanghocvien_Click);

           
            btnDangXuat.Click += new EventHandler(btnDangXuat_Click);
        }

        // ======================================================================
        //                              HÀM HỖ TRỢ
        // ======================================================================

        // Hỗ trợ: TẠO MÃ HỌC VIÊN TỰ ĐỘNG (HVxxx)
        private string GenerateNewMaHV()
        {
            string newMaHV = "HV001";
            string query = "SELECT MAX(MAHOCVIEN) FROM HOCVIEN";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string lastMaHV = result.ToString();
                            int lastNumber;

                            if (lastMaHV.Length > 2 && lastMaHV.StartsWith("HV") &&
                                int.TryParse(lastMaHV.Substring(2), out lastNumber))
                            {
                                int newNumber = lastNumber + 1;
                                newMaHV = "HV" + newNumber.ToString("D3");
                            }
                        }
                    }
                }
                catch
                {
                    // Bỏ qua lỗi kết nối trong khi tạo mã
                }
            }
            return newMaHV;
        }


        // Hỗ trợ: CHUYỂN VỀ CHẾ ĐỘ THÊM MỚI (Reset Form)
        private void ClearHocvienInputs()
        {
            isEditing = false;
            txtmahocvien.Text = GenerateNewMaHV();
            txtmahocvien.ReadOnly = true;
            txtmataikhoan.Clear();
            txtHoten.Clear();
            comboBoxgioitinhhocvien.SelectedIndex = -1;
            maskedTextBoxngaysinh.Clear();
            txtemail.Clear();
            txtsodienthoai.Clear();
            txtdiachi.Clear();
            txtHoten.Focus();

            btnthemhocvien.Enabled = true;
            btnsuahocvien.Text = "Sửa";
            btnsuahocvien.Enabled = false;
            btnxoahocvien.Enabled = false;
        }

        // Load và Hiển thị Dữ liệu Học viên
        private void LoadDataHocvien()
        {
            string query = "SELECT MAHOCVIEN, MATAIKHOAN, HOTEN, GIOITINH, NGAYSINH, EMAIL, SODIENTHOAI, DIACHI FROM HOCVIEN";
            // ... (Logic kết nối, fill DataGridView và thiết lập tiêu đề cột)
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewhocvien.DataSource = dt;

                    // Thiết lập tiêu đề cột (Dựa trên ảnh)
                    dataGridViewhocvien.Columns["MAHOCVIEN"].HeaderText = "Mã Học viên";
                    dataGridViewhocvien.Columns["MATAIKHOAN"].HeaderText = "Mã Tài khoản";
                    dataGridViewhocvien.Columns["HOTEN"].HeaderText = "Họ Tên";
                    dataGridViewhocvien.Columns["GIOITINH"].HeaderText = "Giới Tính";
                    dataGridViewhocvien.Columns["NGAYSINH"].HeaderText = "Ngày Sinh";
                    dataGridViewhocvien.Columns["EMAIL"].HeaderText = "Email";
                    dataGridViewhocvien.Columns["SODIENTHOAI"].HeaderText = "SĐT";
                    dataGridViewhocvien.Columns["DIACHI"].HeaderText = "Địa Chỉ";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu Học viên: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ======================================================================
        //                                SỰ KIỆN CRUD
        // ======================================================================

        private void Form_Quanlyhocvien_Load(object sender, EventArgs e)
        {
            LoadDataHocvien();
            ClearHocvienInputs();
        }

        // Sự kiện KÍCH VÀO DATAGRIDVIEW
        private void dataGridViewhocvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ... (Logic gán dữ liệu từ DataGridView lên controls, tương tự Form_QuanLy)
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewhocvien.Rows.Count)
            {
                DataGridViewRow row = dataGridViewhocvien.Rows[e.RowIndex];

                txtmahocvien.ReadOnly = false;
                txtmahocvien.Text = row.Cells["MAHOCVIEN"].Value.ToString();

                txtmataikhoan.Text = (row.Cells["MATAIKHOAN"].Value != DBNull.Value) ? row.Cells["MATAIKHOAN"].Value.ToString() : string.Empty;
                txtHoten.Text = row.Cells["HOTEN"].Value.ToString();
                comboBoxgioitinhhocvien.Text = row.Cells["GIOITINH"].Value.ToString();
                txtemail.Text = (row.Cells["EMAIL"].Value != DBNull.Value) ? row.Cells["EMAIL"].Value.ToString() : string.Empty;
                txtsodienthoai.Text = (row.Cells["SODIENTHOAI"].Value != DBNull.Value) ? row.Cells["SODIENTHOAI"].Value.ToString() : string.Empty;
                txtdiachi.Text = (row.Cells["DIACHI"].Value != DBNull.Value) ? row.Cells["DIACHI"].Value.ToString() : string.Empty;

                if (row.Cells["NGAYSINH"].Value != DBNull.Value)
                {
                    DateTime ngaySinh = Convert.ToDateTime(row.Cells["NGAYSINH"].Value);
                    maskedTextBoxngaysinh.Text = ngaySinh.ToString("dd/MM/yyyy");
                }
                else
                {
                    maskedTextBoxngaysinh.Clear();
                }

                isEditing = true;
                txtmahocvien.ReadOnly = true;

                btnthemhocvien.Enabled = false;
                btnsuahocvien.Enabled = true;
                btnxoahocvien.Enabled = true;

                btnsuahocvien.Text = "Cập Nhật";
                txtHoten.Focus();
            }
        }

        // Chức năng: THÊM MỚI Học viên
        private void btnthemhocvien_Click(object sender, EventArgs e)
        {
            // ... (Logic Thêm Học viên tương tự Form_QuanLy)
        }

        // Chức năng: CẬP NHẬT (SỬA) Học viên
        private void btnsuahocvien_Click(object sender, EventArgs e)
        {
            // ... (Logic Sửa Học viên tương tự Form_QuanLy)
        }

        // Chức năng: XÓA Học viên (Đã FIX lỗi Khóa ngoại)
        private void btnxoahocvien_Click(object sender, EventArgs e)
        {
            string maHV = txtmahocvien.Text.Trim();

            if (string.IsNullOrEmpty(maHV) || !isEditing)
            {
                MessageBox.Show("Vui lòng chọn Học viên cần xóa từ bảng dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Học viên có mã " + maHV + "?\n(Lưu ý: Tất cả dữ liệu ghi danh của học viên này sẽ bị xóa theo)", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Sử dụng Transaction để đảm bảo tính toàn vẹn (Hoặc phải thiết lập Cascade Delete trong CSDL)
                string queryDeleteGhiDanh = "DELETE FROM GHIDANH WHERE MAHOCVIEN = @MaHV";
                string queryDeleteHocVien = "DELETE FROM HOCVIEN WHERE MAHOCVIEN = @MaHV";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        // 1. Xóa bản ghi trong bảng phụ thuộc (GHIDANH)
                        using (SqlCommand cmdDeleteGhiDanh = new SqlCommand(queryDeleteGhiDanh, conn, transaction))
                        {
                            cmdDeleteGhiDanh.Parameters.AddWithValue("@MaHV", maHV);
                            cmdDeleteGhiDanh.ExecuteNonQuery(); // Xóa các bản ghi phụ thuộc
                        }

                        // 2. Xóa bản ghi trong bảng chính (HOCVIEN)
                        using (SqlCommand cmdDeleteHocVien = new SqlCommand(queryDeleteHocVien, conn, transaction))
                        {
                            cmdDeleteHocVien.Parameters.AddWithValue("@MaHV", maHV);
                            int rowsAffected = cmdDeleteHocVien.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Xóa Học viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataHocvien();
                                ClearHocvienInputs();
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("Không tìm thấy Mã Học viên để xóa.", "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Lỗi CSDL khi xóa: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Chức năng: TÌM KIẾM Học viên (Tìm kiếm bất kỳ nội dung nào trong bảng)
        private void btntimkiemhocvien_Click(object sender, EventArgs e)
        {
            string keyword = txttimkiemhocvien.Text.Trim();
            ClearHocvienInputs();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadDataHocvien();
                return;
            }

            string query = @"SELECT MAHOCVIEN, MATAIKHOAN, HOTEN, GIOITINH, NGAYSINH, EMAIL, SODIENTHOAI, DIACHI 
                             FROM HOCVIEN 
                             WHERE MAHOCVIEN LIKE @Keyword 
                             OR HOTEN LIKE @Keyword 
                             OR SODIENTHOAI LIKE @Keyword 
                             OR GIOITINH LIKE @Keyword 
                             OR DIACHI LIKE @Keyword 
                             OR CONVERT(VARCHAR, NGAYSINH, 103) LIKE @Keyword";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewhocvien.DataSource = dt;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy kết quả nào cho từ khóa '" + keyword + "'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Chức năng: CẬP NHẬT LẠI BẢNG (Reset lưới và form)
        private void btnCapnhatlaibanghocvien_Click(object sender, EventArgs e)
        {
            LoadDataHocvien();
            ClearHocvienInputs();
            MessageBox.Show("Đã tải lại toàn bộ danh sách Học viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txttimkiemhocvien.Clear();
        }

        // ======================================================================
        //                              ĐIỀU HƯỚNG
        // ======================================================================

        private void btntuyendung_Click(object sender, EventArgs e)
        {
            Form_QuanLy form = new Form_QuanLy();
            form.Show();
            this.Hide();
        }

      
      
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            Form_DangNhap form = new Form_DangNhap();
            form.Show();
            this.Close();
        }
    }
}