using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO; // Cần cho chức năng Lưu file
using System.Text;
using System.Windows.Forms;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_Quanlykhoahoc : Form
    {
        // Chuỗi kết nối CSDL (Kiểm tra lại tên Server của bạn)
        private const string connectionString = "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        // Biến trạng thái: true nếu đang chọn dòng để sửa, false là chế độ Thêm mới
        private bool isEditing = false;

        // Dùng để theo dõi dòng dữ liệu đang in
        private int currentPrintingRow = 0;

        public Form_Quanlykhoahoc()
        {
            InitializeComponent();
            InitializeCustomEvents();
        }

        private void InitializeCustomEvents()
        {
            this.Load += new EventHandler(Form_Quanlykhoahoc_Load);
            dataGridViewkhoahoc.CellClick += new DataGridViewCellEventHandler(dataGridViewkhoahoc_CellClick);

            // Chức năng CRUD
            btnthemkhoahoc.Click += new EventHandler(btnthemkhoahoc_Click);
            btnsuakhoahoc.Click += new EventHandler(btnsuakhoahoc_Click);
            btnxoakhoahoc.Click += new EventHandler(btnxoakhoahoc_Click);
            btntimkiemkhoahoc.Click += new EventHandler(btntimkiemkhoahoc_Click);
            btnCapnhatlaibangkhoahoc.Click += new EventHandler(btnCapnhatlaibangkhoahoc_Click);

            // Chức năng In/Lưu
            btnin.Click += new EventHandler(btnin_Click);
            btnluu.Click += new EventHandler(btnluu_Click);

            // Cấu hình sự kiện cho PrintDocument
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);



        }

        // ======================================================================
        //                              HÀM HỖ TRỢ CHUNG
        // ======================================================================

        // Hỗ trợ: TẠO MÃ KHÓA HỌC TỰ ĐỘNG (KHxxx)
        private string GenerateNewMaKH()
        {
            string newMaKH = "KH001";
            string query = "SELECT MAX(MAKHOAHOC) FROM KHOAHOC";

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
                            string lastMaKH = result.ToString();
                            int lastNumber;
                            if (lastMaKH.Length > 2 && lastMaKH.StartsWith("KH") &&
                                int.TryParse(lastMaKH.Substring(2), out lastNumber))
                            {
                                int newNumber = lastNumber + 1;
                                newMaKH = "KH" + newNumber.ToString("D3");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo mã Khóa học mới: " + ex.Message,
                                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return newMaKH;
        }

        // Hỗ trợ: CHUYỂN VỀ CHẾ ĐỘ THÊM MỚI (Reset Form)
        private void ClearKhoahocInputs()
        {
            isEditing = false;
            txtmakhoahoc.Text = GenerateNewMaKH();
            txtmakhoahoc.ReadOnly = true;
            txttenkhoahoc.Clear();
            txttongsogio.Clear();
            txthocphi.Clear();
            txtmota.Clear();
            txtchinhanh.Clear();
            txttenkhoahoc.Focus();

            btnthemkhoahoc.Enabled = true;
            btnsuakhoahoc.Text = "Sửa";
            btnsuakhoahoc.Enabled = false;
            btnxoakhoahoc.Enabled = false;
        }

        // Load và Hiển thị Dữ liệu Khóa học
        private void LoadDataKhoahoc(string filter = "")
        {
            string query = "SELECT MAKHOAHOC, TENKHOAHOC, MOTA, TONGSOGIO, HOCPHI, CHINHANH FROM KHOAHOC " + filter;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewkhoahoc.DataSource = dt;

                    // Thiết lập tiêu đề cột
                    dataGridViewkhoahoc.Columns["MAKHOAHOC"].HeaderText = "Mã KH";
                    dataGridViewkhoahoc.Columns["TENKHOAHOC"].HeaderText = "Tên Khóa học";
                    dataGridViewkhoahoc.Columns["MOTA"].HeaderText = "Mô tả";
                    dataGridViewkhoahoc.Columns["TONGSOGIO"].HeaderText = "Tổng số giờ";
                    dataGridViewkhoahoc.Columns["HOCPHI"].HeaderText = "Học phí";
                    dataGridViewkhoahoc.Columns["CHINHANH"].HeaderText = "Chi nhánh";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu Khóa học: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ======================================================================
        //                                SỰ KIỆN FORM VÀ CRUD
        // ======================================================================

        private void Form_Quanlykhoahoc_Load(object sender, EventArgs e)
        {
            LoadDataKhoahoc();
            ClearKhoahocInputs();
        }

        // Sự kiện KÍCH VÀO DATAGRIDVIEW (Hiển thị chi tiết để Sửa/Xóa)
        private void dataGridViewkhoahoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewkhoahoc.Rows.Count)
            {
                DataGridViewRow row = dataGridViewkhoahoc.Rows[e.RowIndex];

                txtmakhoahoc.ReadOnly = false;
                txtmakhoahoc.Text = row.Cells["MAKHOAHOC"].Value.ToString();
                txttenkhoahoc.Text = row.Cells["TENKHOAHOC"].Value.ToString();
                txttongsogio.Text = row.Cells["TONGSOGIO"].Value.ToString();
                txthocphi.Text = row.Cells["HOCPHI"].Value.ToString();
                txtmota.Text = row.Cells["MOTA"].Value.ToString();
                txtchinhanh.Text = row.Cells["CHINHANH"].Value.ToString();

                isEditing = true;
                txtmakhoahoc.ReadOnly = true;

                btnthemkhoahoc.Enabled = false;
                btnsuakhoahoc.Enabled = true;
                btnxoakhoahoc.Enabled = true;

                btnsuakhoahoc.Text = "Cập Nhật";
                txttenkhoahoc.Focus();
            }
        }

        // Chức năng: THÊM MỚI Khóa học
        private void btnthemkhoahoc_Click(object sender, EventArgs e)
        {
            string maKH = txtmakhoahoc.Text.Trim();
            string tenKH = txttenkhoahoc.Text.Trim();
            string moTa = txtmota.Text.Trim();
            string chiNhanh = txtchinhanh.Text.Trim();
            int tongSoGio;
            decimal hocPhi;

            if (string.IsNullOrEmpty(tenKH) || string.IsNullOrEmpty(moTa) || string.IsNullOrEmpty(chiNhanh) ||
                !int.TryParse(txttongsogio.Text.Trim(), out tongSoGio) ||
                !decimal.TryParse(txthocphi.Text.Trim(), out hocPhi) || tongSoGio <= 0 || hocPhi < 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên KH, Mô tả, Chi nhánh, Tổng số giờ (số > 0) và Học phí (số >= 0) hợp lệ.", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO KHOAHOC (MAKHOAHOC, TENKHOAHOC, MOTA, TONGSOGIO, HOCPHI, CHINHANH) VALUES (@MaKH, @TenKH, @MoTa, @TongSoGio, @HocPhi, @ChiNhanh)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaKH", maKH);
                        cmd.Parameters.AddWithValue("@TenKH", tenKH);
                        cmd.Parameters.AddWithValue("@MoTa", moTa);
                        cmd.Parameters.AddWithValue("@TongSoGio", tongSoGio);
                        cmd.Parameters.AddWithValue("@HocPhi", hocPhi);
                        cmd.Parameters.AddWithValue("@ChiNhanh", chiNhanh);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm Khóa học thành công! Mã: " + maKH, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataKhoahoc();
                            ClearKhoahocInputs();
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            MessageBox.Show("Mã Khóa học đã tồn tại. Đã tự động tạo mã mới.", "Lỗi Trùng Lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ClearKhoahocInputs();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi CSDL khi thêm: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // Chức năng: CẬP NHẬT (SỬA) Khóa học
        private void btnsuakhoahoc_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng chọn Khóa học cần Sửa từ bảng dữ liệu trước.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKH = txtmakhoahoc.Text.Trim();
            string tenKH = txttenkhoahoc.Text.Trim();
            string moTa = txtmota.Text.Trim();
            string chiNhanh = txtchinhanh.Text.Trim();
            int tongSoGio;
            decimal hocPhi;

            if (string.IsNullOrEmpty(tenKH) || string.IsNullOrEmpty(moTa) || string.IsNullOrEmpty(chiNhanh) ||
                !int.TryParse(txttongsogio.Text.Trim(), out tongSoGio) ||
                !decimal.TryParse(txthocphi.Text.Trim(), out hocPhi) || tongSoGio <= 0 || hocPhi < 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE KHOAHOC SET TENKHOAHOC = @TenKH, MOTA = @MoTa, TONGSOGIO = @TongSoGio, HOCPHI = @HocPhi, CHINHANH = @ChiNhanh WHERE MAKHOAHOC = @MaKH";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MaKH", maKH);
                        cmd.Parameters.AddWithValue("@TenKH", tenKH);
                        cmd.Parameters.AddWithValue("@MoTa", moTa);
                        cmd.Parameters.AddWithValue("@TongSoGio", tongSoGio);
                        cmd.Parameters.AddWithValue("@HocPhi", hocPhi);
                        cmd.Parameters.AddWithValue("@ChiNhanh", chiNhanh);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật Khóa học thành công! Mã: " + maKH, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataKhoahoc();
                            ClearKhoahocInputs();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Mã Khóa học để cập nhật hoặc không có dữ liệu thay đổi.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Lỗi CSDL khi cập nhật: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Chức năng: XÓA Khóa học
        private void btnxoakhoahoc_Click(object sender, EventArgs e)
        {
            string maKH = txtmakhoahoc.Text.Trim();

            if (string.IsNullOrEmpty(maKH) || !isEditing)
            {
                MessageBox.Show("Vui lòng chọn Khóa học cần xóa từ bảng dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Khóa học có mã " + maKH + "?\n(Lưu ý: Thao tác này sẽ bị lỗi nếu Khóa học đã được Ghi danh/Mở lớp)", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "DELETE FROM KHOAHOC WHERE MAKHOAHOC = @MaKH";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@MaKH", maKH);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa Khóa học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataKhoahoc();
                                ClearKhoahocInputs();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy Mã Khóa học để xóa.", "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 547)
                            {
                                MessageBox.Show("Không thể xóa Khóa học này vì đã có dữ liệu liên quan. Vui lòng xóa dữ liệu liên quan trước.", "Lỗi Khóa Ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Chức năng: TÌM KIẾM Khóa học
        private void btntimkiemkhoahoc_Click(object sender, EventArgs e)
        {
            string keyword = txttimkiemkhoahoc.Text.Trim();
            ClearKhoahocInputs();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadDataKhoahoc();
                return;
            }

            string filter = string.Format("WHERE MAKHOAHOC LIKE '%{0}%' OR TENKHOAHOC LIKE N'%{0}%' OR CHINHANH LIKE N'%{0}%' OR MOTA LIKE N'%{0}%' OR CONVERT(VARCHAR, HOCPHI) LIKE '%{0}%'", keyword);

            LoadDataKhoahoc(filter);

            if (dataGridViewkhoahoc.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào cho từ khóa '" + keyword + "'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Chức năng: CẬP NHẬT LẠI BẢNG (Reset lưới và form)
        private void btnCapnhatlaibangkhoahoc_Click(object sender, EventArgs e)
        {
            LoadDataKhoahoc();
            ClearKhoahocInputs();
            MessageBox.Show("Đã tải lại toàn bộ danh sách Khóa học.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txttimkiemkhoahoc.Clear();
        }

        // ======================================================================
        //                              CHỨC NĂNG IN VÀ LƯU
        // ======================================================================

        private void btnin_Click(object sender, EventArgs e)
        {
            currentPrintingRow = 0; // Reset dòng in
            PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
            printPreviewDialog1.Document = printDocument1;

            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

           private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
        Graphics g = e.Graphics;
        int y = e.MarginBounds.Top;
        int x = e.MarginBounds.Left;
        int rowHeight = 25;

        // Font và Brush
        using (Font fontTitle = new Font("Arial", 14, FontStyle.Bold))
        using (Font fontHeader = new Font("Arial", 10, FontStyle.Bold))
        using (Font fontBody = new Font("Arial", 9, FontStyle.Regular))
        using (SolidBrush blackBrush = new SolidBrush(Color.Black))
        {
            // 1. Tiêu đề
            string title = "BÁO CÁO DANH SÁCH KHÓA HỌC";
            SizeF titleSize = g.MeasureString(title, fontTitle);
            g.DrawString(title, fontTitle, blackBrush, x + (e.MarginBounds.Width - titleSize.Width) / 2, y);
            y += (int)titleSize.Height + 15;

            // Định nghĩa độ rộng cột
            int[] colWidths = { 60, 180, 150, 80, 100, 150 };
            string[] headers = { "Mã KH", "Tên Khóa học", "Mô tả", "Tổng giờ", "Học phí", "Chi nhánh" };

            // 2. Header
            int currentX = x;
            for (int i = 0; i < headers.Length; i++)
            {
                Rectangle rect = new Rectangle(currentX, y, colWidths[i], rowHeight);
                g.DrawRectangle(Pens.Black, rect);
                g.DrawString(headers[i], fontHeader, blackBrush, rect,
                    new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                currentX += colWidths[i];
            }
            y += rowHeight;

            // 3. Dữ liệu
            while (currentPrintingRow < dataGridViewkhoahoc.Rows.Count)
            {
                DataGridViewRow dgvRow = dataGridViewkhoahoc.Rows[currentPrintingRow];
                if (dgvRow.IsNewRow)
                {
                    currentPrintingRow++;
                    continue;
                }

                if (y + rowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                currentX = x;
                for (int i = 0; i < headers.Length; i++)
                {
                    Rectangle rect = new Rectangle(currentX, y, colWidths[i], rowHeight);
                    g.DrawRectangle(Pens.Black, rect);

                    string cellValue = dgvRow.Cells[i].Value != null ? dgvRow.Cells[i].Value.ToString() : "";

                    decimal hp;
                    if (i == 4 && decimal.TryParse(cellValue, out hp))
                    {
                        cellValue = hp.ToString("N0") + " VNĐ";
                    }


                    g.DrawString(cellValue, fontBody, blackBrush, rect,
                        new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
                    currentX += colWidths[i];
                }

                y += rowHeight;
                currentPrintingRow++;
            }

            e.HasMorePages = false;
        }
    }


        private void btnluu_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.FileName = "DanhSachKhoaHoc_" + DateTime.Now.ToString("yyyyMMdd");

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportToCsv(saveFileDialog1.FileName);
                    MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Hỗ trợ: Xuất dữ liệu DataGridView ra file CSV
        private void ExportToCsv(string filePath)
        {
            var header = new StringBuilder();
            for (int i = 0; i < dataGridViewkhoahoc.Columns.Count; i++)
            {
                string headerValue = dataGridViewkhoahoc.Columns[i].HeaderText.Replace("\"", "\"\"");
                header.Append("\"" + headerValue + "\"" + (i == dataGridViewkhoahoc.Columns.Count - 1 ? "" : ","));
            }

            File.WriteAllText(filePath, header.ToString() + Environment.NewLine, Encoding.UTF8);

            var rows = new StringBuilder();
            foreach (DataGridViewRow row in dataGridViewkhoahoc.Rows)
            {
                if (row.IsNewRow) continue;
                for (int i = 0; i < dataGridViewkhoahoc.Columns.Count; i++)
                {
                    string value = row.Cells[i].Value != null ? row.Cells[i].Value.ToString() : string.Empty;
                    value = value.Replace("\"", "\"\"");
                    rows.Append("\"" + value + "\"" + (i == dataGridViewkhoahoc.Columns.Count - 1 ? "" : ","));
                }
                rows.Append(Environment.NewLine);
            }

            File.AppendAllText(filePath, rows.ToString(), Encoding.UTF8);
        }


        // ======================================================================
        //                              ĐIỀU HƯỚNG
        // ======================================================================

        // Bạn có thể giữ các hàm điều hướng dưới đây hoặc chỉnh sửa lại cho phù hợp với form đích

        private void btntuyendung_Click(object sender, EventArgs e)
        {
            // Giả định Form_QuanLy là form của Tuyển Dụng
            Form_QuanLy form = new Form_QuanLy();
            form.Show();
            this.Hide();
        }

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

        private void btnkhuyenmai_Click(object sender, EventArgs e)
        {
            Form_KhuyenMai form = new Form_KhuyenMai();
            form.Show();
            this.Hide();
        }

        private void btnquanlynhansu_Click(object sender, EventArgs e)
        {
            Form_Quanlynhansu form = new Form_Quanlynhansu();
            form.Show();
            this.Hide();
        }



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