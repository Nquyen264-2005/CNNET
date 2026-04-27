using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_KhuyenMai : Form
    {
        // Chuỗi kết nối CSDL (Kiểm tra lại tên Server của bạn)
        private string connectionString =
                    "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";
        // Biến trạng thái: true nếu đang chọn dòng để sửa, false là chế độ Thêm mới
        private bool isEditing = false;

        public Form_KhuyenMai()
        {
            InitializeComponent();
            InitializeCustomEvents();
        }

        private void InitializeCustomEvents()
        {
            this.Load += new EventHandler(Form_KhuyenMai_Load);
            dataGridViewkhuyenmai.CellClick += new DataGridViewCellEventHandler(dataGridViewkhuyenmai_CellClick);

            // Chức năng CRUD
            btnthemkhuyenmai.Click += new EventHandler(btnthemkhuyenmai_Click);
            btnsuakhuyenmai.Click += new EventHandler(btnsuakhuyenmai_Click);
            btnxoakhuyenmai.Click += new EventHandler(btnxoakhuyenmai_Click);
            btntimkiemkhuyenmai.Click += new EventHandler(btntimkiemkhuyenmai_Click);
            btnCapnhatlaibangkhuyenmai.Click += new EventHandler(btnCapnhatlaibangkhuyenmai_Click);

        
            btnDangXuat.Click += new EventHandler(btnDangXuat_Click);
        }

        // ======================================================================
        //                              HÀM HỖ TRỢ
        // ======================================================================

        // Hỗ trợ: TẠO MÃ KHUYẾN MẠI TỰ ĐỘNG (KMxxx)
        private string GenerateNewMaKM()
        {
            string newMaKM = "KM001";
            string query = "SELECT MAX(MAKHUYENMAI) FROM KHUYENMAI";

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
                            string lastMaKM = result.ToString();
                            int lastNumber;

                            if (lastMaKM.Length > 2 && lastMaKM.StartsWith("KM") &&
                                int.TryParse(lastMaKM.Substring(2), out lastNumber))
                            {
                                int newNumber = lastNumber + 1;
                                newMaKM = "KM" + newNumber.ToString("D3");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo mã Khuyến mại mới: " + ex.Message,
                                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return newMaKM;
        }


        // Hỗ trợ: CHUYỂN VỀ CHẾ ĐỘ THÊM MỚI (Reset Form)
        private void ClearKhuyenmaiInputs()
        {
            isEditing = false;
            txtmakhuyenmai.Text = GenerateNewMaKM();
            txtmakhuyenmai.ReadOnly = true;
            txttenkhuyenmai.Clear();
            txtgiamgia.Clear();
            txtloaigiamgia.Clear();
            maskedTextBoxngaybatdau.Clear();
            maskedTextBoxngayketthuc.Clear();
            txttrangthai.Clear();
            txttenkhuyenmai.Focus();

            btnthemkhuyenmai.Enabled = true;
            btnsuakhuyenmai.Text = "Sửa";
            btnsuakhuyenmai.Enabled = false;
            btnxoakhuyenmai.Enabled = false;
        }

        // Load và Hiển thị Dữ liệu Khuyến mại
        private void LoadDataKhuyenmai()
        {
            string query = "SELECT MAKHUYENMAI, TENKHUYENMAI, GIAMGIA, LOAIGIAMGIA, NGAYBATDAUKM, NGAYKETTHUCKM, TRANGTHAI FROM KHUYENMAI";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewkhuyenmai.DataSource = dt;

                    // Thiết lập tiêu đề cột
                    dataGridViewkhuyenmai.Columns["MAKHUYENMAI"].HeaderText = "Mã KM";
                    dataGridViewkhuyenmai.Columns["TENKHUYENMAI"].HeaderText = "Tên KM";
                    dataGridViewkhuyenmai.Columns["GIAMGIA"].HeaderText = "Giảm giá";
                    dataGridViewkhuyenmai.Columns["LOAIGIAMGIA"].HeaderText = "Loại GG";
                    dataGridViewkhuyenmai.Columns["NGAYBATDAUKM"].HeaderText = "Ngày BĐ";
                    dataGridViewkhuyenmai.Columns["NGAYKETTHUCKM"].HeaderText = "Ngày KT";
                    dataGridViewkhuyenmai.Columns["TRANGTHAI"].HeaderText = "Trạng thái";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu Khuyến mại: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ======================================================================
        //                                SỰ KIỆN FORM VÀ CRUD
        // ======================================================================

        private void Form_KhuyenMai_Load(object sender, EventArgs e)
        {
            LoadDataKhuyenmai();
            ClearKhuyenmaiInputs();
        }

        // Sự kiện KÍCH VÀO DATAGRIDVIEW (Hiển thị chi tiết để Sửa/Xóa)
        private void dataGridViewkhuyenmai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewkhuyenmai.Rows.Count)
            {
                DataGridViewRow row = dataGridViewkhuyenmai.Rows[e.RowIndex];

                txtmakhuyenmai.ReadOnly = false;
                txtmakhuyenmai.Text = row.Cells["MAKHUYENMAI"].Value.ToString();

                txttenkhuyenmai.Text = row.Cells["TENKHUYENMAI"].Value.ToString();
                txtgiamgia.Text = row.Cells["GIAMGIA"].Value.ToString();
                txtloaigiamgia.Text = row.Cells["LOAIGIAMGIA"].Value.ToString();
                txttrangthai.Text = row.Cells["TRANGTHAI"].Value.ToString();

                if (row.Cells["NGAYBATDAUKM"].Value != DBNull.Value)
                {
                    DateTime ngayBD = Convert.ToDateTime(row.Cells["NGAYBATDAUKM"].Value);
                    maskedTextBoxngaybatdau.Text = ngayBD.ToString("dd/MM/yyyy");
                }
                else
                {
                    maskedTextBoxngaybatdau.Clear();
                }

                if (row.Cells["NGAYKETTHUCKM"].Value != DBNull.Value)
                {
                    DateTime ngayKT = Convert.ToDateTime(row.Cells["NGAYKETTHUCKM"].Value);
                    maskedTextBoxngayketthuc.Text = ngayKT.ToString("dd/MM/yyyy");
                }
                else
                {
                    maskedTextBoxngayketthuc.Clear();
                }

                isEditing = true;
                txtmakhuyenmai.ReadOnly = true;

                btnthemkhuyenmai.Enabled = false;
                btnsuakhuyenmai.Enabled = true;
                btnxoakhuyenmai.Enabled = true;

                btnsuakhuyenmai.Text = "Cập Nhật";
                txttenkhuyenmai.Focus();
            }
        }

        // Chức năng: THÊM MỚI Khuyến mại
        private void btnthemkhuyenmai_Click(object sender, EventArgs e)
        {
            string maKM = txtmakhuyenmai.Text.Trim();
            string tenKM = txttenkhuyenmai.Text.Trim();
            string loaiGG = txtloaigiamgia.Text.Trim();
            string trangThai = txttrangthai.Text.Trim();
            decimal giamGia;
            DateTime ngayBD, ngayKT;

            // Đã sửa lỗi: Dùng txtgiamgia để parse decimal
            if (string.IsNullOrEmpty(tenKM) || string.IsNullOrEmpty(loaiGG) || string.IsNullOrEmpty(trangThai) ||
                !decimal.TryParse(txtgiamgia.Text.Trim(), out giamGia) ||
                !DateTime.TryParseExact(maskedTextBoxngaybatdau.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayBD) ||
                !DateTime.TryParseExact(maskedTextBoxngayketthuc.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayKT))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên KM, Giảm giá (số), Loại giảm giá, Trạng thái và Ngày BĐ/KT hợp lệ (dd/MM/yyyy).", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (loaiGG == "Phần trăm" && (giamGia < 0 || giamGia > 100))
            {
                MessageBox.Show("Nếu là 'Phần trăm', Giảm giá phải nằm trong khoảng 0 đến 100.", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ngayBD >= ngayKT)
            {
                MessageBox.Show("Ngày bắt đầu phải trước Ngày kết thúc.", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO KHUYENMAI (MAKHUYENMAI, TENKHUYENMAI, GIAMGIA, LOAIGIAMGIA, NGAYBATDAUKM, NGAYKETTHUCKM, TRANGTHAI) VALUES (@MaKM, @TenKM, @GiamGia, @LoaiGG, @NgayBD, @NgayKT, @TrangThai)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaKM", maKM);
                        cmd.Parameters.AddWithValue("@TenKM", tenKM);
                        cmd.Parameters.AddWithValue("@GiamGia", giamGia);
                        cmd.Parameters.AddWithValue("@LoaiGG", loaiGG);
                        cmd.Parameters.AddWithValue("@NgayBD", ngayBD.Date);
                        cmd.Parameters.AddWithValue("@NgayKT", ngayKT.Date);
                        cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm Khuyến mại thành công! Mã: " + maKM, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataKhuyenmai();
                            ClearKhuyenmaiInputs();
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            MessageBox.Show("Mã Khuyến mại đã tồn tại. Đã tự động tạo mã mới.", "Lỗi Trùng Lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ClearKhuyenmaiInputs();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi CSDL khi thêm: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // Chức năng: CẬP NHẬT (SỬA) Khuyến mại
        private void btnsuakhuyenmai_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng chọn Khuyến mại cần Sửa từ bảng dữ liệu trước.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKM = txtmakhuyenmai.Text.Trim();
            string tenKM = txttenkhuyenmai.Text.Trim();
            string loaiGG = txtloaigiamgia.Text.Trim();
            string trangThai = txttrangthai.Text.Trim();
            decimal giamGia;
            DateTime ngayBD, ngayKT;

            // Đã sửa lỗi: Dùng txtgiamgia để parse decimal
            if (string.IsNullOrEmpty(tenKM) || string.IsNullOrEmpty(loaiGG) || string.IsNullOrEmpty(trangThai) ||
                !decimal.TryParse(txtgiamgia.Text.Trim(), out giamGia) ||
                !DateTime.TryParseExact(maskedTextBoxngaybatdau.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayBD) ||
                !DateTime.TryParseExact(maskedTextBoxngayketthuc.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayKT))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (loaiGG == "Phần trăm" && (giamGia < 0 || giamGia > 100))
            {
                MessageBox.Show("Nếu là 'Phần trăm', Giảm giá phải nằm trong khoảng 0 đến 100.", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ngayBD >= ngayKT)
            {
                MessageBox.Show("Ngày bắt đầu phải trước Ngày kết thúc.", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE KHUYENMAI SET TENKHUYENMAI = @TenKM, GIAMGIA = @GiamGia, LOAIGIAMGIA = @LoaiGG, NGAYBATDAUKM = @NgayBD, NGAYKETTHUCKM = @NgayKT, TRANGTHAI = @TrangThai WHERE MAKHUYENMAI = @MaKM";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaKM", maKM);
                        cmd.Parameters.AddWithValue("@TenKM", tenKM);
                        cmd.Parameters.AddWithValue("@GiamGia", giamGia);
                        cmd.Parameters.AddWithValue("@LoaiGG", loaiGG);
                        cmd.Parameters.AddWithValue("@NgayBD", ngayBD.Date);
                        cmd.Parameters.AddWithValue("@NgayKT", ngayKT.Date);
                        cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật Khuyến mại thành công! Mã: " + maKM, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataKhuyenmai();
                            ClearKhuyenmaiInputs();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Mã Khuyến mại để cập nhật hoặc không có dữ liệu thay đổi.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Lỗi CSDL khi cập nhật: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Chức năng: XÓA Khuyến mại
        private void btnxoakhuyenmai_Click(object sender, EventArgs e)
        {
            string maKM = txtmakhuyenmai.Text.Trim();

            if (string.IsNullOrEmpty(maKM) || !isEditing)
            {
                MessageBox.Show("Vui lòng chọn Khuyến mại cần xóa từ bảng dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Khuyến mại có thể có ràng buộc khóa ngoại với bảng GhiDanh/Hóa đơn
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Khuyến mại có mã " + maKM + "?\n(Lưu ý: Thao tác này có thể bị lỗi nếu KM đã được áp dụng trong Ghi danh/Hóa đơn)", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "DELETE FROM KHUYENMAI WHERE MAKHUYENMAI = @MaKM";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@MaKM", maKM);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa Khuyến mại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataKhuyenmai();
                                ClearKhuyenmaiInputs();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy Mã Khuyến mại để xóa.", "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 547)
                            {
                                MessageBox.Show("Không thể xóa Khuyến mại này vì đã có dữ liệu liên quan. Vui lòng xóa dữ liệu liên quan trước.", "Lỗi Khóa Ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Chức năng: TÌM KIẾM Khuyến mại (Tìm kiếm bất kỳ nội dung nào trong bảng)
        private void btntimkiemkhuyenmai_Click(object sender, EventArgs e)
        {
            string keyword = txttimkiemkhuyenmai.Text.Trim();
            ClearKhuyenmaiInputs();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadDataKhuyenmai();
                return;
            }

            string query = @"SELECT MAKHUYENMAI, TENKHUYENMAI, GIAMGIA, LOAIGIAMGIA, NGAYBATDAUKM, NGAYKETTHUCKM, TRANGTHAI 
                             FROM KHUYENMAI 
                             WHERE MAKHUYENMAI LIKE @Keyword 
                             OR TENKHUYENMAI LIKE @Keyword 
                             OR LOAIGIAMGIA LIKE @Keyword 
                             OR TRANGTHAI LIKE @Keyword 
                             OR CONVERT(VARCHAR, GIAMGIA) LIKE @Keyword 
                             OR CONVERT(VARCHAR, NGAYBATDAUKM, 103) LIKE @Keyword
                             OR CONVERT(VARCHAR, NGAYKETTHUCKM, 103) LIKE @Keyword";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewkhuyenmai.DataSource = dt;

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
        private void btnCapnhatlaibangkhuyenmai_Click(object sender, EventArgs e)
        {
            LoadDataKhuyenmai();
            ClearKhuyenmaiInputs();
            MessageBox.Show("Đã tải lại toàn bộ danh sách Khuyến mại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txttimkiemkhuyenmai.Clear();
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
    }
}