using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_Quanlygiaovien : Form
    {
        // Chuỗi kết nối CSDL (Kiểm tra lại tên Server của bạn)
        private const string connectionString = "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012 ;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        // Biến trạng thái: true nếu đang chọn dòng để sửa, false là chế độ Thêm mới
        private bool isEditing = false;


        public Form_Quanlygiaovien()
        {
            InitializeComponent();
            InitializeCustomEvents();
        }

        private void InitializeCustomEvents()
        {
            this.Load += new EventHandler(Form_Quanlygiaovien_Load);
            dataGridViewgiaovien.CellClick += new DataGridViewCellEventHandler(dataGridViewgiaovien_CellClick);

            // Chức năng CRUD
            btnthemgiaovien.Click += new EventHandler(btnthemgiaovien_Click);
            btnsuagiaovien.Click += new EventHandler(btnxoagiaovien_Click); // Đã sửa tên nút trong code
            btnxoagiaovien.Click += new EventHandler(btnsuagiaovien_Click); // Đã sửa tên nút trong code
            btntimkiemgiaovien.Click += new EventHandler(btntimkiemgiaovien_Click);
            btnCapnhatlaibanggiaovien.Click += new EventHandler(btnCapnhatlaibanggiaovien_Click);

           
            btnDangXuat.Click += new EventHandler(btnDangXuat_Click);
        }

        // ======================================================================
        //                              HÀM HỖ TRỢ
        // ======================================================================

        // Hỗ trợ: TẠO MÃ GIÁO VIÊN TỰ ĐỘNG (GVxxx)
        private string GenerateNewMaGV()
        {
            string newMaGV = "GV001";
            string query = "SELECT MAX(MAGIANGVIEN) FROM GIAOVIEN";

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
                            string lastMaGV = result.ToString();
                            int lastNumber;

                            if (lastMaGV.Length > 2 && lastMaGV.StartsWith("GV") &&
                                int.TryParse(lastMaGV.Substring(2), out lastNumber))
                            {
                                int newNumber = lastNumber + 1;
                                newMaGV = "GV" + newNumber.ToString("D3");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo mã Giáo viên mới: " + ex.Message,
                                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return newMaGV;
        }


        // Hỗ trợ: CHUYỂN VỀ CHẾ ĐỘ THÊM MỚI (Reset Form)
        private void ClearGiaovienInputs()
        {
            isEditing = false;
            txtmagiaovien.Text = GenerateNewMaGV();
            txtmagiaovien.ReadOnly = true;
            txtmataikhoan.Clear();
            txtHoten.Clear();
            txtsodienthoai.Clear();
            txtemail.Clear();
            txttrinhdo.Clear();
            txtchuyennganh.Clear();
            txtloaihopdong.Clear();
            txttrangthai.Clear();
            txtHoten.Focus();

            btnthemgiaovien.Enabled = true;
            btnsuagiaovien.Text = "Sửa";
            btnsuagiaovien.Enabled = false;
            btnxoagiaovien.Enabled = false;
        }

        // Load và Hiển thị Dữ liệu Giáo viên
        private void LoadDataGiaovien()
        {
            string query = "SELECT MAGIANGVIEN, MATAIKHOAN, HOTEN, EMAIL, SODIENTHOAI, TRINHDO, CHUYENNGANH, LOAIHOPDONG, TRANGTHAI FROM GIAOVIEN";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewgiaovien.DataSource = dt;

                    // Thiết lập tiêu đề cột
                    dataGridViewgiaovien.Columns["MAGIANGVIEN"].HeaderText = "Mã Giáo viên";
                    dataGridViewgiaovien.Columns["MATAIKHOAN"].HeaderText = "Mã Tài khoản";
                    dataGridViewgiaovien.Columns["HOTEN"].HeaderText = "Họ Tên";
                    dataGridViewgiaovien.Columns["EMAIL"].HeaderText = "Email";
                    dataGridViewgiaovien.Columns["SODIENTHOAI"].HeaderText = "SĐT";
                    dataGridViewgiaovien.Columns["TRINHDO"].HeaderText = "Trình độ";
                    dataGridViewgiaovien.Columns["CHUYENNGANH"].HeaderText = "Chuyên ngành";
                    dataGridViewgiaovien.Columns["LOAIHOPDONG"].HeaderText = "Loại HĐ";
                    dataGridViewgiaovien.Columns["TRANGTHAI"].HeaderText = "Trạng thái";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu Giáo viên: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ======================================================================
        //                                SỰ KIỆN FORM VÀ CRUD
        // ======================================================================

        private void Form_Quanlygiaovien_Load(object sender, EventArgs e)
        {
            LoadDataGiaovien();
            ClearGiaovienInputs();
        }

        // Sự kiện KÍCH VÀO DATAGRIDVIEW (Hiển thị chi tiết để Sửa/Xóa)
        private void dataGridViewgiaovien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewgiaovien.Rows.Count)
            {
                DataGridViewRow row = dataGridViewgiaovien.Rows[e.RowIndex];

                txtmagiaovien.ReadOnly = false;
                txtmagiaovien.Text = row.Cells["MAGIANGVIEN"].Value.ToString();

                txtmataikhoan.Text = (row.Cells["MATAIKHOAN"].Value != DBNull.Value) ? row.Cells["MATAIKHOAN"].Value.ToString() : string.Empty;
                txtHoten.Text = row.Cells["HOTEN"].Value.ToString();
                txtsodienthoai.Text = (row.Cells["SODIENTHOAI"].Value != DBNull.Value) ? row.Cells["SODIENTHOAI"].Value.ToString() : string.Empty;
                txtemail.Text = (row.Cells["EMAIL"].Value != DBNull.Value) ? row.Cells["EMAIL"].Value.ToString() : string.Empty;
                txttrinhdo.Text = (row.Cells["TRINHDO"].Value != DBNull.Value) ? row.Cells["TRINHDO"].Value.ToString() : string.Empty;
                txtchuyennganh.Text = (row.Cells["CHUYENNGANH"].Value != DBNull.Value) ? row.Cells["CHUYENNGANH"].Value.ToString() : string.Empty;
                txtloaihopdong.Text = (row.Cells["LOAIHOPDONG"].Value != DBNull.Value) ? row.Cells["LOAIHOPDONG"].Value.ToString() : string.Empty;
                txttrangthai.Text = (row.Cells["TRANGTHAI"].Value != DBNull.Value) ? row.Cells["TRANGTHAI"].Value.ToString() : string.Empty;

                isEditing = true;
                txtmagiaovien.ReadOnly = true;

                btnthemgiaovien.Enabled = false;
                btnsuagiaovien.Enabled = true;
                btnxoagiaovien.Enabled = true;

                btnsuagiaovien.Text = "Cập Nhật";
                txtHoten.Focus();
            }
        }

        // Chức năng: THÊM MỚI Giáo viên
        private void btnthemgiaovien_Click(object sender, EventArgs e)
        {
            string maGV = txtmagiaovien.Text.Trim();
            string hoTen = txtHoten.Text.Trim();
            string soDT = txtsodienthoai.Text.Trim();
            string trinhDo = txttrinhdo.Text.Trim();
            string chuyenNganh = txtchuyennganh.Text.Trim();
            string loaiHD = txtloaihopdong.Text.Trim();
            string trangThai = txttrangthai.Text.Trim();


            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(trinhDo) || string.IsNullOrEmpty(chuyenNganh) || string.IsNullOrEmpty(loaiHD) || string.IsNullOrEmpty(trangThai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên, Trình độ, Chuyên ngành, Loại hợp đồng và Trạng thái.", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maTK = string.IsNullOrEmpty(txtmataikhoan.Text.Trim()) ? null : txtmataikhoan.Text.Trim();
            string email = string.IsNullOrEmpty(txtemail.Text.Trim()) ? null : txtemail.Text.Trim();

            string query = "INSERT INTO GIAOVIEN (MAGIANGVIEN, MATAIKHOAN, HOTEN, EMAIL, SODIENTHOAI, TRINHDO, CHUYENNGANH, LOAIHOPDONG, TRANGTHAI) VALUES (@MaGV, @MaTK, @HoTen, @Email, @SDT, @TrinhDo, @ChuyenNganh, @LoaiHD, @TrangThai)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaGV", maGV);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@TrinhDo", trinhDo);
                        cmd.Parameters.AddWithValue("@ChuyenNganh", chuyenNganh);
                        cmd.Parameters.AddWithValue("@LoaiHD", loaiHD);
                        cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                        cmd.Parameters.AddWithValue("@MaTK", (object)maTK ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SDT", (object)soDT ?? DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm Giáo viên thành công! Mã: " + maGV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataGiaovien();
                            ClearGiaovienInputs();
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            MessageBox.Show("Mã Giáo viên đã tồn tại. Đã tự động tạo mã mới.", "Lỗi Trùng Lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ClearGiaovienInputs();
                        }
                        else if (ex.Number == 547)
                        {
                            MessageBox.Show("Mã Tài khoản (" + maTK + ") không tồn tại. Vui lòng kiểm tra lại.", "Lỗi Khóa Ngoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi CSDL khi thêm: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // Chức năng: CẬP NHẬT (SỬA) Giáo viên
        private void btnsuagiaovien_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng chọn Giáo viên cần Sửa từ bảng dữ liệu trước.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maGV = txtmagiaovien.Text.Trim();
            string hoTen = txtHoten.Text.Trim();
            string soDT = txtsodienthoai.Text.Trim();
            string trinhDo = txttrinhdo.Text.Trim();
            string chuyenNganh = txtchuyennganh.Text.Trim();
            string loaiHD = txtloaihopdong.Text.Trim();
            string trangThai = txttrangthai.Text.Trim();

            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(trinhDo) || string.IsNullOrEmpty(chuyenNganh) || string.IsNullOrEmpty(loaiHD) || string.IsNullOrEmpty(trangThai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc.", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maTK = string.IsNullOrEmpty(txtmataikhoan.Text.Trim()) ? null : txtmataikhoan.Text.Trim();
            string email = string.IsNullOrEmpty(txtemail.Text.Trim()) ? null : txtemail.Text.Trim();

            string query = "UPDATE GIAOVIEN SET MATAIKHOAN = @MaTK, HOTEN = @HoTen, EMAIL = @Email, SODIENTHOAI = @SDT, TRINHDO = @TrinhDo, CHUYENNGANH = @ChuyenNganh, LOAIHOPDONG = @LoaiHD, TRANGTHAI = @TrangThai WHERE MAGIANGVIEN = @MaGV";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaGV", maGV);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@TrinhDo", trinhDo);
                        cmd.Parameters.AddWithValue("@ChuyenNganh", chuyenNganh);
                        cmd.Parameters.AddWithValue("@LoaiHD", loaiHD);
                        cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                        cmd.Parameters.AddWithValue("@MaTK", (object)maTK ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SDT", (object)soDT ?? DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật Giáo viên thành công! Mã: " + maGV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataGiaovien();
                            ClearGiaovienInputs();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Mã Giáo viên để cập nhật hoặc không có dữ liệu thay đổi.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // Chức năng: XÓA Giáo viên
        private void btnxoagiaovien_Click(object sender, EventArgs e)
        {
            string maGV = txtmagiaovien.Text.Trim();

            if (string.IsNullOrEmpty(maGV) || !isEditing)
            {
                MessageBox.Show("Vui lòng chọn Giáo viên cần xóa từ bảng dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Giáo viên có thể có ràng buộc khóa ngoại với các bảng như LỚP HỌC (LopHoc), PHÂN CÔNG (PhanCong), v.v.
            // Cần xóa hoặc cập nhật các bản ghi phụ thuộc trước khi xóa Giáo viên.
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Giáo viên có mã " + maGV + "?\n(Lưu ý: Thao tác này có thể bị lỗi nếu Giáo viên đang có lớp học hoặc phân công giảng dạy)", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "DELETE FROM GIAOVIEN WHERE MAGIANGVIEN = @MaGV";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@MaGV", maGV);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa Giáo viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataGiaovien();
                                ClearGiaovienInputs();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy Mã Giáo viên để xóa.", "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 547) // Lỗi Khóa ngoại
                            {
                                MessageBox.Show("Không thể xóa Giáo viên này vì đã có dữ liệu liên quan (ví dụ: đang dạy lớp học). Vui lòng xóa dữ liệu liên quan trước.", "Lỗi Khóa Ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Chức năng: TÌM KIẾM Giáo viên (Tìm kiếm bất kỳ nội dung nào trong bảng)
        private void btntimkiemgiaovien_Click(object sender, EventArgs e)
        {
            string keyword = txttimkiemgiaovien.Text.Trim();
            ClearGiaovienInputs();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadDataGiaovien();
                return;
            }

            string query = @"SELECT MAGIANGVIEN, MATAIKHOAN, HOTEN, EMAIL, SODIENTHOAI, TRINHDO, CHUYENNGANH, LOAIHOPDONG, TRANGTHAI 
                             FROM GIAOVIEN 
                             WHERE MAGIANGVIEN LIKE @Keyword 
                             OR HOTEN LIKE @Keyword 
                             OR EMAIL LIKE @Keyword 
                             OR SODIENTHOAI LIKE @Keyword 
                             OR TRINHDO LIKE @Keyword
                             OR CHUYENNGANH LIKE @Keyword
                             OR LOAIHOPDONG LIKE @Keyword
                             OR TRANGTHAI LIKE @Keyword";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewgiaovien.DataSource = dt;

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
        private void btnCapnhatlaibanggiaovien_Click(object sender, EventArgs e)
        {
            LoadDataGiaovien();
            ClearGiaovienInputs();
            MessageBox.Show("Đã tải lại toàn bộ danh sách Giáo viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txttimkiemgiaovien.Clear();
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

        //private void btnquanlyhocvien_Click(object sender, EventArgs e)
        //{
        //    Form_Quanlyhocvien form = new Form_Quanlyhocvien();
        //    form.Show();
        //    this.Hide();
        //}

        //private void btnquanlynhansu_Click(object sender, EventArgs e)
        //{
        //    Form_Quanlynhansu form = new Form_Quanlynhansu();
        //    form.Show();
        //    this.Hide();
        //}

        //private void btnkhuyenmai_Click(object sender, EventArgs e)
        //{
        //    Form_KhuyenMai form = new Form_KhuyenMai();
        //    form.Show();
        //    this.Hide();
        //}
        //private void btnbtnKhoaHoc_Click(object sender, EventArgs e)
        //{
        //    Form_Quanlykhoahoc form = new Form_Quanlykhoahoc();
        //    form.Show();
        //    this.Hide();
      //  }
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn Đăng xuất?", "Xác nhận Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form_DangNhap form = new Form_DangNhap();
                form.Show();
                this.Close();
            }
        }
    }
}