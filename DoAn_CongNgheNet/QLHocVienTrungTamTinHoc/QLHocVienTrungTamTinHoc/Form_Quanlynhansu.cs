using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_Quanlynhansu : Form
    {
        // Chuỗi kết nối CSDL (Kiểm tra lại tên Server của bạn)
        private const string connectionString = "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        // Biến trạng thái: true nếu đang chọn dòng để sửa, false là chế độ Thêm mới
        private bool isEditing = false;


        public Form_Quanlynhansu()
        {
            InitializeComponent();
            InitializeCustomEvents();
        }

        private void InitializeCustomEvents()
        {
            this.Load += new EventHandler(Form_Quanlynhansu_Load);
            dataGridViewnhansu.CellClick += new DataGridViewCellEventHandler(dataGridViewnhansu_CellClick);

            // Chức năng CRUD
            btnthemnhansu.Click += new EventHandler(btnthemnhansu_Click);
            btnsuanhansu.Click += new EventHandler(btnsuanhansu_Click);
            btnxoanhansu.Click += new EventHandler(btnxoanhansu_Click);
            btntimkiemnhansu.Click += new EventHandler(btntimkiemnhansu_Click);
            btnCapnhatlaibangnhansu.Click += new EventHandler(btnCapnhatlaibangnhansu_Click);

            // Điều hướng
            btnDangXuat.Click += new EventHandler(btnDangXuat_Click);
        }

        // ======================================================================
        //                              HÀM HỖ TRỢ
        // ======================================================================

        // Hỗ trợ: TẠO MÃ NHÂN SỰ TỰ ĐỘNG (NSxxx)
        private string GenerateNewMaNS()
        {
            string newMaNS = "NS001";
            string query = "SELECT MAX(MANHANSU) FROM NHANSU";

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
                            string lastMaNS = result.ToString();
                            int lastNumber;

                            if (lastMaNS.Length > 2 && lastMaNS.StartsWith("NS") &&
                                int.TryParse(lastMaNS.Substring(2), out lastNumber))
                            {
                                int newNumber = lastNumber + 1;
                                newMaNS = "NS" + newNumber.ToString("D3");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo mã Nhân sự mới: " + ex.Message,
                                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return newMaNS;
        }


        // Hỗ trợ: CHUYỂN VỀ CHẾ ĐỘ THÊM MỚI (Reset Form)
        private void ClearNhansuInputs()
        {
            isEditing = false;
            txtmanhansu.Text = GenerateNewMaNS();
            txtmanhansu.ReadOnly = true;
            txtmataikhoan.Clear();
            txtHoten.Clear();
            maskedTextBoxngaysinh.Clear();
            comboBoxchucdanh.SelectedIndex = -1;
            txtsodienthoai.Clear();
            txtemail.Clear();
            txtdiachi.Clear();
            txtmota.Clear();
            txtHoten.Focus();

            btnthemnhansu.Enabled = true;
            btnsuanhansu.Text = "Sửa";
            btnsuanhansu.Enabled = false;
            btnxoanhansu.Enabled = false;
        }

        // Load và Hiển thị Dữ liệu Nhân sự
        private void LoadDataNhansu()
        {
            string query = "SELECT MANHANSU, MATAIKHOAN, HOTEN, NGAYSINH, CHUCDANH, DIACHI, MOTA, EMAIL, SODIENTHOAI FROM NHANSU";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewnhansu.DataSource = dt;

                    // Thiết lập tiêu đề cột (Dựa trên ảnh)
                    dataGridViewnhansu.Columns["MANHANSU"].HeaderText = "Mã Nhân sự";
                    dataGridViewnhansu.Columns["MATAIKHOAN"].HeaderText = "Mã Tài khoản";
                    dataGridViewnhansu.Columns["HOTEN"].HeaderText = "Họ Tên";
                    dataGridViewnhansu.Columns["NGAYSINH"].HeaderText = "Ngày Sinh";
                    dataGridViewnhansu.Columns["CHUCDANH"].HeaderText = "Chức danh";
                    dataGridViewnhansu.Columns["DIACHI"].HeaderText = "Địa chỉ";
                    dataGridViewnhansu.Columns["MOTA"].HeaderText = "Mô tả";
                    dataGridViewnhansu.Columns["EMAIL"].HeaderText = "Email";
                    dataGridViewnhansu.Columns["SODIENTHOAI"].HeaderText = "SĐT";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu Nhân sự: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ======================================================================
        //                                SỰ KIỆN FORM VÀ CRUD
        // ======================================================================

        private void Form_Quanlynhansu_Load(object sender, EventArgs e)
        {
            LoadDataNhansu();
            ClearNhansuInputs();
        }

        // Sự kiện KÍCH VÀO DATAGRIDVIEW (Hiển thị chi tiết để Sửa/Xóa)
        private void dataGridViewnhansu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewnhansu.Rows.Count)
            {
                DataGridViewRow row = dataGridViewnhansu.Rows[e.RowIndex];

                txtmanhansu.ReadOnly = false;
                txtmanhansu.Text = row.Cells["MANHANSU"].Value.ToString();

                txtmataikhoan.Text = (row.Cells["MATAIKHOAN"].Value != DBNull.Value) ? row.Cells["MATAIKHOAN"].Value.ToString() : string.Empty;
                txtHoten.Text = row.Cells["HOTEN"].Value.ToString();
                comboBoxchucdanh.Text = (row.Cells["CHUCDANH"].Value != DBNull.Value) ? row.Cells["CHUCDANH"].Value.ToString() : string.Empty;
                txtsodienthoai.Text = (row.Cells["SODIENTHOAI"].Value != DBNull.Value) ? row.Cells["SODIENTHOAI"].Value.ToString() : string.Empty;
                txtemail.Text = (row.Cells["EMAIL"].Value != DBNull.Value) ? row.Cells["EMAIL"].Value.ToString() : string.Empty;
                txtdiachi.Text = (row.Cells["DIACHI"].Value != DBNull.Value) ? row.Cells["DIACHI"].Value.ToString() : string.Empty;
                txtmota.Text = (row.Cells["MOTA"].Value != DBNull.Value) ? row.Cells["MOTA"].Value.ToString() : string.Empty;

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
                txtmanhansu.ReadOnly = true;

                btnthemnhansu.Enabled = false;
                btnsuanhansu.Enabled = true;
                btnxoanhansu.Enabled = true;

                btnsuanhansu.Text = "Cập Nhật";
                txtHoten.Focus();
            }
        }

        // Chức năng: THÊM MỚI Nhân sự
        private void btnthemnhansu_Click(object sender, EventArgs e)
        {

        }

        // Chức năng: CẬP NHẬT (SỬA) Nhân sự
        private void btnsuanhansu_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng chọn Nhân sự cần Sửa từ bảng dữ liệu trước.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNS = txtmanhansu.Text.Trim();
            string hoTen = txtHoten.Text.Trim();
            string chucDanh = comboBoxchucdanh.Text.Trim();
            DateTime ngaySinh;

            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(chucDanh) ||
                !DateTime.TryParseExact(maskedTextBoxngaysinh.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngaySinh))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên, Chức danh và Ngày Sinh hợp lệ (dd/MM/yyyy).", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maTK = string.IsNullOrEmpty(txtmataikhoan.Text.Trim()) ? null : txtmataikhoan.Text.Trim();
            string soDT = string.IsNullOrEmpty(txtsodienthoai.Text.Trim()) ? null : txtsodienthoai.Text.Trim();
            string email = string.IsNullOrEmpty(txtemail.Text.Trim()) ? null : txtemail.Text.Trim();
            string diaChi = string.IsNullOrEmpty(txtdiachi.Text.Trim()) ? null : txtdiachi.Text.Trim();
            string moTa = string.IsNullOrEmpty(txtmota.Text.Trim()) ? null : txtmota.Text.Trim();

            string query = "UPDATE NHANSU SET MATAIKHOAN = @MaTK, HOTEN = @HoTen, NGAYSINH = @NgaySinh, CHUCDANH = @ChucDanh, DIACHI = @DiaChi, MOTA = @MoTa, EMAIL = @Email, SODIENTHOAI = @SDT WHERE MANHANSU = @MaNS";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaNS", maNS);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh.Date);
                        cmd.Parameters.AddWithValue("@ChucDanh", chucDanh);

                        cmd.Parameters.AddWithValue("@MaTK", (object)maTK ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SDT", (object)soDT ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)diaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MoTa", (object)moTa ?? DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật Nhân sự thành công! Mã: " + maNS, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataNhansu();
                            ClearNhansuInputs();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Mã Nhân sự để cập nhật hoặc không có dữ liệu thay đổi.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 547)
                        {
                            MessageBox.Show("Mã Tài khoản (" + maTK + ") không tồn tại. Vui lòng kiểm tra lại.", "Lỗi Khóa Ngoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi CSDL khi cập nhật: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // Chức năng: XÓA Nhân sự
        private void btnxoanhansu_Click(object sender, EventArgs e)
        {
            string maNS = txtmanhansu.Text.Trim();

            if (string.IsNullOrEmpty(maNS) || !isEditing)
            {
                MessageBox.Show("Vui lòng chọn Nhân sự cần xóa từ bảng dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nhân sự có thể có ràng buộc khóa ngoại với các bảng như TUYENDUNG, v.v.
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Nhân sự có mã " + maNS + "?\n(Lưu ý: Thao tác này có thể bị lỗi nếu Nhân sự đang có bản ghi liên quan)", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "DELETE FROM NHANSU WHERE MANHANSU = @MaNS";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@MaNS", maNS);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa Nhân sự thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataNhansu();
                                ClearNhansuInputs();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy Mã Nhân sự để xóa.", "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 547) // Lỗi Khóa ngoại
                            {
                                MessageBox.Show("Không thể xóa Nhân sự này vì đã có dữ liệu liên quan (ví dụ: trong bảng Tuyển dụng). Vui lòng xóa dữ liệu liên quan trước.", "Lỗi Khóa Ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("Lỗi CSDL khi xóa: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        // Chức năng: TÌM KIẾM Nhân sự (Tìm kiếm bất kỳ nội dung nào trong bảng)
        private void btntimkiemnhansu_Click(object sender, EventArgs e)
        {
            string keyword = txttimkiemnhansu.Text.Trim();
            ClearNhansuInputs();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadDataNhansu();
                return;
            }

            string query = @"SELECT MANHANSU, MATAIKHOAN, HOTEN, NGAYSINH, CHUCDANH, DIACHI, MOTA, EMAIL, SODIENTHOAI 
                             FROM NHANSU 
                             WHERE MANHANSU LIKE @Keyword 
                             OR HOTEN LIKE @Keyword 
                             OR CHUCDANH LIKE @Keyword 
                             OR DIACHI LIKE @Keyword 
                             OR EMAIL LIKE @Keyword
                             OR SODIENTHOAI LIKE @Keyword
                             OR MOTA LIKE @Keyword
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
                    dataGridViewnhansu.DataSource = dt;

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
        private void btnCapnhatlaibangnhansu_Click(object sender, EventArgs e)
        {
            LoadDataNhansu();
            ClearNhansuInputs();
            MessageBox.Show("Đã tải lại toàn bộ danh sách Nhân sự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txttimkiemnhansu.Clear();
        }

        // ======================================================================
        //                              ĐIỀU HƯỚNG
        // ======================================================================
      
      
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn Đăng xuất?", "Xác nhận Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form_DangNhap form = new Form_DangNhap();
                form.Show();
                this.Close();
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}