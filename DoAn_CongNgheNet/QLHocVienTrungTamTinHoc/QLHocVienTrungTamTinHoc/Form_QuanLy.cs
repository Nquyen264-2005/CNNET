using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing; 

namespace QLHocVienTrungTamTinHoc
{
    // Giả định Form_QuanLy là Form chứa các controls quản lý Tuyển Dụng
    public partial class Form_QuanLy : Form
    {
        // Chuỗi kết nối của bạn
        private const string connectionString = "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        // Biến trạng thái: true nếu đang chọn dòng để sửa, false là chế độ Thêm mới
        private bool isEditing = false;



        public Form_QuanLy()
        {
            InitializeComponent();

            // GÁN CÁC SỰ KIỆN NẾU CHƯA GÁN TRONG DESIGNER
            this.Load += new EventHandler(Form_QuanLy_Load);

            // Sự kiện DataGridView
            dataGridViewtuyendung.CellClick += new DataGridViewCellEventHandler(dataGridViewtuyendung_CellClick);

            // Sự kiện nút chức năng
            btnthemtuyendung.Click += new EventHandler(btnthemtuyendung_Click);
            btnsuatuyendung.Click += new EventHandler(btnsuatuyendung_Click);
            btnxoatuyendung.Click += new EventHandler(btnxoatuyendung_Click);
            btntimkiemtuyendung.Click += new EventHandler(btntimkiemtuyendung_Click);

            
       
            btnDangXuat.Click += new EventHandler(btnDangXuat_Click);
            //btnNhapMoi.Click += new EventHandler(btnNhapMoi_Click); // Giả định có nút này
        }

        // --- HỖ TRỢ: TẠO MÃ TUYỂN DỤNG TỰ ĐỘNG (TDxxx) ---
        private string GenerateNewMaTD()
        {
            string newMaTD = "TD001"; // Mã mặc định nếu CSDL rỗng
            string query = "SELECT MAX(MATUYENDUNG) FROM TUYENDUNG";

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
                            string lastMaTD = result.ToString();
                            int lastNumber;

                            if (lastMaTD.Length > 2 && lastMaTD.StartsWith("TD") &&
                                int.TryParse(lastMaTD.Substring(2), out lastNumber))
                            {
                                int newNumber = lastNumber + 1;
                                newMaTD = "TD" + newNumber.ToString("D3");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo mã Tuyển Dụng mới: " + ex.Message,
                                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return newMaTD;
        }

        // --- HỖ TRỢ: CHUYỂN VỀ CHẾ ĐỘ THÊM MỚI ---
        private void ClearTuyendungInputs()
        {
            // Reset trạng thái
            isEditing = false;

            // Tự động tạo mã mới và GÁN vào TextBox
            txtmatuyendung.Text = GenerateNewMaTD();

            // Cài đặt trạng thái ReadOnly cho các Controls
            txtmatuyendung.ReadOnly = true;
            txtmanhansu.ReadOnly = false;
            maskedTextBoxngaytuyendung.ReadOnly = false;
            txtmota.ReadOnly = false;

            // Xóa dữ liệu cũ
            txtmanhansu.Clear();
            maskedTextBoxngaytuyendung.Clear();
            txtmota.Clear();
            txtmanhansu.Focus();

            // Quản lý nút (Thêm: ON, Sửa/Cập Nhật: OFF)
            btnthemtuyendung.Enabled = true;
            btnsuatuyendung.Text = "Sửa"; // Đảm bảo nút Sửa hiện đúng tên
            btnsuatuyendung.Enabled = false; // Vô hiệu hóa Sửa/Cập nhật
            btnxoatuyendung.Enabled = false; // Vô hiệu hóa Xóa
        }

        // --- Phương thức Load và Hiển thị Dữ liệu ---
        private void LoadDataTuyendung()
        {
            string query = "SELECT MATUYENDUNG, MANHANSU, NGAYTUYENDUNG, MOTA FROM TUYENDUNG";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewtuyendung.DataSource = dt;

                    // Thiết lập tiêu đề cột
                    dataGridViewtuyendung.Columns["MATUYENDUNG"].HeaderText = "Mã Tuyển Dụng";
                    dataGridViewtuyendung.Columns["MANHANSU"].HeaderText = "Mã Nhân Sự";
                    dataGridViewtuyendung.Columns["NGAYTUYENDUNG"].HeaderText = "Ngày Tuyển Dụng";
                    dataGridViewtuyendung.Columns["MOTA"].HeaderText = "Mô Tả";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu Tuyển dụng: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- SỰ KIỆN LOAD FORM ---
        private void Form_QuanLy_Load(object sender, EventArgs e)
        {
            LoadDataTuyendung();
            ClearTuyendungInputs(); // Chuẩn bị sẵn sàng cho thao tác Thêm mới
        }

        // --- SỰ KIỆN CLICK VÀO DATAGRIDVIEW (HIỂN THỊ CHI TIẾT) ---
        private void dataGridViewtuyendung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewtuyendung.Rows.Count)
            {
                DataGridViewRow row = dataGridViewtuyendung.Rows[e.RowIndex];

                // 1. Gán giá trị vào các controls
                txtmatuyendung.ReadOnly = false; // Mở khóa tạm để gán giá trị

                txtmatuyendung.Text = row.Cells["MATUYENDUNG"].Value.ToString();
                txtmanhansu.Text = row.Cells["MANHANSU"].Value.ToString();

                // Xử lý giá trị ngày tháng từ CSDL
                if (row.Cells["NGAYTUYENDUNG"].Value != DBNull.Value)
                {
                    // Đảm bảo ép kiểu và định dạng chuẩn (dd/MM/yyyy)
                    DateTime ngayTD = Convert.ToDateTime(row.Cells["NGAYTUYENDUNG"].Value);
                    maskedTextBoxngaytuyendung.Text = ngayTD.ToString("dd/MM/yyyy");
                }
                else
                {
                    maskedTextBoxngaytuyendung.Clear();
                }

                // Gán giá trị cho Mô Tả
                txtmota.Text = (row.Cells["MOTA"].Value != DBNull.Value) ? row.Cells["MOTA"].Value.ToString() : string.Empty;

                // 2. Thiết lập trạng thái controls/button (Chế độ SỬA/CẬP NHẬT)
                isEditing = true;
                txtmatuyendung.ReadOnly = true; // Khóa Mã TD lại (không cho sửa khóa chính)

                txtmanhansu.ReadOnly = false;
                maskedTextBoxngaytuyendung.ReadOnly = false;
                txtmota.ReadOnly = false;

                // Cho phép Sửa và Xóa, Vô hiệu hóa Thêm
                btnthemtuyendung.Enabled = false;
                btnsuatuyendung.Enabled = true;
                btnxoatuyendung.Enabled = true;

                // Đổi tên nút Sửa thành Cập Nhật (để nhấn mạnh chức năng tiếp theo)
                btnsuatuyendung.Text = "Cập Nhật";

                txtmanhansu.Focus();
            }
        }

        // --- Chức năng THÊM MỚI Tuyển dụng ---
        private void btnthemtuyendung_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu và kiểm tra
            string maTD = txtmatuyendung.Text.Trim();
            string maNS = txtmanhansu.Text.Trim();
            string moTa = txtmota.Text.Trim();
            DateTime ngayTD;

            if (string.IsNullOrEmpty(maTD) || string.IsNullOrEmpty(maNS) || !DateTime.TryParseExact(maskedTextBoxngaytuyendung.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayTD))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã Nhân Sự và Ngày Tuyển Dụng hợp lệ (dd/MM/yyyy).", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO TUYENDUNG (MATUYENDUNG, MANHANSU, NGAYTUYENDUNG, MOTA) VALUES (@MaTD, @MaNS, @NgayTD, @MoTa)";

            // 2. Thực thi SQL
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaTD", maTD);
                        cmd.Parameters.AddWithValue("@MaNS", maNS);
                        cmd.Parameters.AddWithValue("@NgayTD", ngayTD.Date);
                        cmd.Parameters.AddWithValue("@MoTa", moTa);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm Tuyển dụng thành công! Mã: " + maTD, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataTuyendung();
                            ClearTuyendungInputs(); // Reset form cho lần thêm mới tiếp theo
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            MessageBox.Show("Mã Tuyển Dụng đã tồn tại. Đã tự động tạo mã mới.", "Lỗi Trùng Lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ClearTuyendungInputs();
                        }
                        else if (ex.Number == 547)
                        {
                            MessageBox.Show("Mã Nhân Sự (" + maNS + ") không tồn tại trong hệ thống. Vui lòng kiểm tra lại.", "Lỗi Khóa Ngoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi CSDL khi thêm: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // --- Chức năng CẬP NHẬT Tuyển dụng (Gán cho nút btnsuatuyendung) ---
        private void btnsuatuyendung_Click(object sender, EventArgs e)
        {
            // Chỉ chạy Cập nhật khi đang ở chế độ Sửa (isEditing = true)
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng chọn Tuyển Dụng cần Sửa từ bảng dữ liệu trước.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Lấy dữ liệu và kiểm tra
            string maTD = txtmatuyendung.Text.Trim();
            string maNS = txtmanhansu.Text.Trim();
            string moTa = txtmota.Text.Trim();
            DateTime ngayTD;

            if (string.IsNullOrEmpty(maTD) || string.IsNullOrEmpty(maNS) || !DateTime.TryParseExact(maskedTextBoxngaytuyendung.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayTD))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ (dd/MM/yyyy).", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE TUYENDUNG SET MANHANSU = @MaNS, NGAYTUYENDUNG = @NgayTD, MOTA = @MoTa WHERE MATUYENDUNG = @MaTD";

            // 2. Thực thi SQL
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaNS", maNS);
                        cmd.Parameters.AddWithValue("@NgayTD", ngayTD.Date);
                        cmd.Parameters.AddWithValue("@MoTa", moTa);
                        cmd.Parameters.AddWithValue("@MaTD", maTD);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật Tuyển dụng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataTuyendung();
                            ClearTuyendungInputs(); // RESET VỀ CHẾ ĐỘ THÊM MỚI
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Mã Tuyển Dụng để cập nhật hoặc không có dữ liệu thay đổi.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 547)
                        {
                            MessageBox.Show("Mã Nhân Sự (" + maNS + ") không tồn tại trong hệ thống. Vui lòng kiểm tra lại.", "Lỗi Khóa Ngoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi CSDL khi cập nhật: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // --- Chức năng XÓA Tuyển dụng ---
        private void btnxoatuyendung_Click(object sender, EventArgs e)
        {
            string maTD = txtmatuyendung.Text.Trim();

            if (string.IsNullOrEmpty(maTD) || !isEditing)
            {
                MessageBox.Show("Vui lòng chọn Tuyển Dụng cần xóa từ bảng dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Tuyển dụng có mã " + maTD + "?", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "DELETE FROM TUYENDUNG WHERE MATUYENDUNG = @MaTD";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@MaTD", maTD);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa Tuyển dụng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataTuyendung();
                                ClearTuyendungInputs(); // Reset form sau khi xóa
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy Mã Tuyển Dụng để xóa.", "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi CSDL khi xóa: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // --- Chức năng TÌM KIẾM ĐA TRƯỜNG Tuyển dụng ---
        private void btntimkiemtuyendung_Click(object sender, EventArgs e)
        {
            string keyword = txttimkiemtuyendung.Text.Trim();

            // Xóa dữ liệu controls hiện tại và chuẩn bị cho chế độ Thêm nếu keyword trống
            ClearTuyendungInputs();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadDataTuyendung(); // Nếu trống thì load lại toàn bộ
                return;
            }

            // Tìm kiếm theo Mã TD, Mã NS, Ngày TD (dạng chuỗi dd/MM/yyyy) hoặc Mô Tả
            string query = @"SELECT MATUYENDUNG, MANHANSU, NGAYTUYENDUNG, MOTA 
                              FROM TUYENDUNG 
                              WHERE MATUYENDUNG LIKE @Keyword 
                              OR MANHANSU LIKE @Keyword 
                              OR CONVERT(NVARCHAR, NGAYTUYENDUNG, 103) LIKE @Keyword 
                              OR MOTA LIKE @Keyword";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewtuyendung.DataSource = dt;

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

        // --- CHỨC NĂNG BỔ SUNG: Nút Nhập Mới (Reset form về chế độ Thêm mới) ---
        private void btnNhapMoi_Click(object sender, EventArgs e)
        {
            ClearTuyendungInputs();
            LoadDataTuyendung(); // Load lại toàn bộ bảng
        }

        // --- Chuyển form (Navigation Buttons) ---

        // * Lưu ý: Các Form_XXX phải được định nghĩa trong project của bạn

        private void btnquanlyhocvien_Click(object sender, EventArgs e)
        {
            Form_Quanlyhocvien form = new Form_Quanlyhocvien();
            form.Show();
            this.Hide();
        }

        private void btnquanlygiaovien_Click(object sender, EventArgs e)
        {
            Form_Quanlygiaovien form = new Form_Quanlygiaovien();
            form.Show();
            this.Hide();
        }

        private void btnquanlynhansu_Click(object sender, EventArgs e)
        {
            Form_Quanlynhansu form = new Form_Quanlynhansu();
            form.Show();
            this.Hide();
        }

        private void btnkhuyenmai_Click(object sender, EventArgs e)
        {
            Form_KhuyenMai form = new Form_KhuyenMai();
            form.Show();
            this.Hide();
        }
        private void btnbtnKhoaHoc_Click(object sender, EventArgs e)
        {
            Form_Quanlykhoahoc form = new Form_Quanlykhoahoc();
            form.Show();
            this.Hide();
        }
 
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            // Trở về Form Đăng Nhập
            Form_DangNhap form = new Form_DangNhap();
            form.Show();
            this.Close(); // Đóng form hiện tại
        }
    }
}