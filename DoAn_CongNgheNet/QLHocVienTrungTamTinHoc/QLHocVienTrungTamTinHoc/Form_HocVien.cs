using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Cần thiết cho Chart Control

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_HocVien : Form
    {
        private string userName;
        private const string ConnectionString = "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";
        // 🚨 THAY THẾ BẰNG CHUỖI KẾT NỐI THỰC TẾ CỦA BẠN 🚨

        // Trạng thái Form: false=Xem, true=Sửa
        private bool isEditing = false;

        // Sửa Constructor để nhận Tên đăng nhập
        public Form_HocVien(string username)
        {
            InitializeComponent();
            this.userName = username;
            this.Text = "Học Viên - " + username + " | Hệ thống QLHVTTH";

            // Thiết lập trạng thái ban đầu cho các nút Sửa/Cập nhật
            SetEditMode(false);

            // Xóa sự kiện mặc định (nếu có) và gán sự kiện mới
            // Đảm bảo sự kiện SelectionChanged chỉ được gán MỘT LẦN
            this.dataGridViewKhoahoc.SelectionChanged += new EventHandler(dataGridViewKhoahoc_SelectionChanged);
        }

        // --- HÀM TẢI DỮ LIỆU TỔNG QUÁT ---

        private void Form_HocVien_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Lỗi: Không tìm thấy thông tin đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadStudentProfile(userName);
            LoadStudentCourses(userName);
        }

        // --- 1. Tải Thông tin cá nhân (SP_GetStudentProfile) ---
        private void LoadStudentProfile(string tenDangNhap)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetStudentProfile", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Thông tin Tài khoản (groupBox1)
                            txttendangnhap.Text = reader["TENDANGNHAP"].ToString();
                            txthoten.Text = reader["HOTEN"].ToString();
                            txtemail.Text = reader["EMAIL"].ToString();
                            txtsodienthoai.Text = reader["SODIENTHOAI"].ToString();
                            txtquyentruycap.Text = reader["TENQUYEN"].ToString();
                            txtngaytaotaikhoan.Text = Convert.ToDateTime(reader["NGAYTAO"]).ToShortDateString();

                            // Thông tin chi tiết Học viên (groupctHocVien)
                            txtMahocvien.Text = reader["MAHOCVIEN"].ToString();
                            txtGioiTinh.Text = reader["GIOITINH"].ToString();
                            txtngaysinh.Text = reader["NGAYSINH"] == DBNull.Value ? "" : Convert.ToDateTime(reader["NGAYSINH"]).ToShortDateString();
                            txtdiachi.Text = reader["DIACHI"].ToString();
                            txtghichu.Text = reader["GHICHU"].ToString();

                            // Đặt tất cả các trường không được chỉnh sửa thành ReadOnly
                            SetProfileReadOnly(true);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin học viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải hồ sơ học viên: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- 2. Tải Danh sách Khóa học (SP_GetStudentCourses) ---
        private void LoadStudentCourses(string tenDangNhap)
        {
            DataTable dtCourses = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetStudentCourses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    conn.Open();
                    da.Fill(dtCourses);
                }

                if (dtCourses.Rows.Count == 1 && dtCourses.Columns.Contains("ErrorMessage"))
                {
                    MessageBox.Show(dtCourses.Rows[0]["ErrorMessage"].ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataGridViewKhoahoc.DataSource = null;
                }
                else
                {
                    dataGridViewKhoahoc.DataSource = dtCourses;
                    CustomizeCourseGrid();
                    // Tự động tải điểm thành phần của khóa học đầu tiên
                    if (dataGridViewKhoahoc.Rows.Count > 0)
                    {
                        dataGridViewKhoahoc.ClearSelection();
                        dataGridViewKhoahoc.Rows[0].Selected = true;
                        LoadDetailedScore();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khóa học: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridViewKhoahoc.DataSource = null;
            }
        }

        // --- 3. Tải và Vẽ Biểu đồ Điểm thành phần (SP_GetEnrollmentDetailedScore) ---
        private void LoadDetailedScore()
        {
            // Xóa dữ liệu cũ trên Chart
            chartDiemThanhPham.Series.Clear();
            chartDiemThanhPham.Titles.Clear();


            if (dataGridViewKhoahoc.SelectedRows.Count > 0)
            {
                // Lấy MAGHIDANH từ hàng đang chọn 
                if (dataGridViewKhoahoc.SelectedRows[0].Cells["MAGHIDANH"].Value == DBNull.Value)
                {
                    return; // Không có Mã Ghi Danh, không thể tải điểm
                }

                string maGhiDanh = dataGridViewKhoahoc.SelectedRows[0].Cells["MAGHIDANH"].Value.ToString();
                DataTable dtDetailedScores = new DataTable();

                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    using (SqlCommand cmd = new SqlCommand("SP_GetEnrollmentDetailedScore", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaGhiDanh", maGhiDanh);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        conn.Open();
                        da.Fill(dtDetailedScores);
                    }

                    // Gọi hàm vẽ biểu đồ
                    CustomizeDetailedScoreChart(dtDetailedScores);
                }
                catch (Exception ex)
                {
                    // Lỗi này chính là lỗi "Invalid object name DIEM_THANHPHAN"
                    MessageBox.Show("Lỗi khi tải điểm thành phần: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- Hàm đã sửa: Vẽ biểu đồ cột Điểm thành phần ---
        private void CustomizeDetailedScoreChart(DataTable dtScores)
        {
            // Cài đặt Chart Area ban đầu hoặc xóa Chart Area cũ
            chartDiemThanhPham.ChartAreas.Clear();
            ChartArea chartArea = new ChartArea();
            chartDiemThanhPham.ChartAreas.Add(chartArea);

            chartDiemThanhPham.Series.Clear();
            chartDiemThanhPham.Titles.Clear();


            if (dtScores == null || dtScores.Rows.Count == 0)
            {
                chartDiemThanhPham.Titles.Add("Không có dữ liệu điểm thành phần");
                return;
            }

            // Cấu hình Title
            chartDiemThanhPham.Titles.Add("Biểu Đồ Điểm Thành Phần");

            // Cấu hình Series (loại biểu đồ)
            Series series = new Series("Điểm Của Học Viên");
            series.ChartType = SeriesChartType.Column; // Loại biểu đồ cột
            series.IsValueShownAsLabel = true; // Hiển thị giá trị trên đỉnh cột
            series.Color = Color.SteelBlue; // Màu cột
            series["PointWidth"] = "0.7"; // Độ rộng cột

            // Thêm dữ liệu vào Series
            foreach (DataRow row in dtScores.Rows)
            {
                string tenThanhPhan = row["TENDIEMTHANHPHAN"].ToString();
                double diemSo = 0;
                double trongSo = 0; // Sửa từ HeSo thành TrongSo

                // Lấy DiemSo
                if (row["DIEMSO"] != DBNull.Value && Double.TryParse(row["DIEMSO"].ToString(), out diemSo))
                {
                    // Lấy TrongSo
                    if (row["TRONGSO"] != DBNull.Value && Double.TryParse(row["TRONGSO"].ToString(), out trongSo))
                    {
                        // Thêm điểm, sử dụng tên thành phần làm nhãn trục X
                        DataPoint point = series.Points.Add(diemSo);
                        point.AxisLabel = tenThanhPhan;
                        point.LegendText = "{tenThanhPhan} ({trongSo}%)"; // Hiển thị Trọng số trong Legend
                        point.ToolTip = "Điểm: {diemSo} | Trọng số: {trongSo}%"; // Hiển thị khi hover
                    }
                    else // Trường hợp không có Trọng số
                    {
                        DataPoint point = series.Points.Add(diemSo);
                        point.AxisLabel = tenThanhPhan;
                    }
                }
            }

            chartDiemThanhPham.Series.Add(series);

            // Cấu hình Chart Area (Trục X, Y)
            chartArea.AxisY.Maximum = 10; // Điểm tối đa
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Interval = 1; // Khoảng chia trên trục Y
            chartArea.AxisY.Title = "Điểm (0-10)";
            chartArea.AxisX.Title = "Bài Kiểm Tra (Thành Phần)";

            // Cấu hình trục X để các nhãn không bị chồng lên nhau
            chartArea.AxisX.LabelStyle.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
        }

        // --- CÁC HÀM HỖ TRỢ VÀ SỰ KIỆN DATA BINDING ---

        private void CustomizeCourseGrid()
        {
            if (dataGridViewKhoahoc.Columns.Count > 0)
            {
                dataGridViewKhoahoc.Columns["MAGHIDANH"].Visible = false;
                dataGridViewKhoahoc.Columns["HOCPHI"].Visible = false;
                dataGridViewKhoahoc.Columns["TENKHUYENMAI"].Visible = false;
                dataGridViewKhoahoc.Columns["GIAMGIA"].Visible = false;
                dataGridViewKhoahoc.Columns["LOAIGIAMGIA"].Visible = false;

                dataGridViewKhoahoc.Columns["TENKHOAHOC"].HeaderText = "Tên Khóa Học";
                dataGridViewKhoahoc.Columns["MALOPHOC"].HeaderText = "Mã Lớp";
                dataGridViewKhoahoc.Columns["DIEMTONGKET"].HeaderText = "Điểm TK";
                dataGridViewKhoahoc.Columns["TrangThaiGhiDanh"].HeaderText = "Trạng Thái";
                dataGridViewKhoahoc.Columns["TenGiangVien"].HeaderText = "Giảng Viên";

                dataGridViewKhoahoc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // Sự kiện khi chọn hàng khác trên danh sách khóa học
        private void dataGridViewKhoahoc_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem sự kiện này có được kích hoạt từ việc chọn hàng thực tế không
            if (dataGridViewKhoahoc.SelectedRows.Count > 0)
            {
                LoadDetailedScore();
            }

        }

        // --- 4. LOGIC SỬA/CẬP NHẬT ĐỊA CHỈ ---

        private void SetProfileReadOnly(bool readOnly)
        {
            // Các trường không bao giờ được sửa
            txttendangnhap.ReadOnly = true;
            txthoten.ReadOnly = true;
            txtemail.ReadOnly = true;
            txtsodienthoai.ReadOnly = true;
            txtquyentruycap.ReadOnly = true;
            txtngaytaotaikhoan.ReadOnly = true;
            txtMahocvien.ReadOnly = true;
            txtGioiTinh.ReadOnly = true;
            txtngaysinh.ReadOnly = true;
            txtghichu.ReadOnly = true;

            // Chỉ txtdiachi được phép thay đổi trạng thái
            txtdiachi.ReadOnly = readOnly;
        }

        private void SetEditMode(bool editMode)
        {
            isEditing = editMode;
            SetProfileReadOnly(!editMode);

            // Giả sử bạn dùng 1 nút, đổi text/enable
            btnSua.Enabled = !editMode;
            btnCapNhat.Enabled = editMode;

            if (editMode)
            {
                txtdiachi.Focus();
                MessageBox.Show("Chế độ sửa: Bạn chỉ được phép sửa Địa chỉ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Bật chế độ sửa
            SetEditMode(true);
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (!isEditing) return;

            string newAddress = txtdiachi.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateStudentAddress", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        // Giả sử SP_UpdateStudentAddress nhận MaHocVien và NewAddress
                        cmd.Parameters.AddWithValue("@MaHocVien", txtMahocvien.Text);
                        cmd.Parameters.AddWithValue("@NewAddress", newAddress);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected < 0)
                        {
                            MessageBox.Show("Cập nhật địa chỉ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Tắt chế độ sửa
                            SetEditMode(false);
                            // Cập nhật lại dữ liệu Profile để đảm bảo đồng bộ
                            LoadStudentProfile(this.userName);
                        }
                        else
                        {
                            MessageBox.Show("Không có bản ghi nào được cập nhật. Vui lòng kiểm tra lại Mã Học viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật địa chỉ: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            // Mở Form_DoiMatKhau
            Form_DoiMatKhau f = new Form_DoiMatKhau(this.userName);
            f.ShowDialog();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                // Đóng form Học viên hiện tại
                this.Hide();
                // Tạo mới Form Đăng nhập và hiển thị
                Form_DangNhap d = new Form_DangNhap();
                d.Show();
            }
        }
    }
}