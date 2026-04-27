using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_GiaoVien : Form
    {
        // Cập nhật chuỗi kết nối đúng với máy của bạn
        private readonly string connectionString =
            "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        public Form_GiaoVien()
        {
            InitializeComponent();

            // Gán sự kiện nếu chưa gán trong Designer
            this.Load += Form_GiaoVien_Load;
            btnThemDiem.Click += btnThemDiem_Click;
            btnSua.Click += btnSua_Click;
            btnXoadiem.Click += btnXoadiem_Click;
            btnDangXuat.Click += btnDangXuat_Click;
            dataGridViewdiem.SelectionChanged += dataGridViewdiem_SelectionChanged;
        }

        private void Form_GiaoVien_Load(object sender, EventArgs e)
        {
            LoadData();
            UpdateChart();
        }

        // ======================= DATA BINDING =======================

        private void LoadData()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT MAGHIDANH, MABAIKIEMTRA, DIEM, NGAYCHAM FROM DIEMSO ORDER BY MAGHIDANH, MABAIKIEMTRA", cn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewdiem.DataSource = dt;

                    if (dataGridViewdiem.Columns.Count > 0)
                    {
                        dataGridViewdiem.Columns["MAGHIDANH"].HeaderText = "Mã ghi danh";
                        dataGridViewdiem.Columns["MABAIKIEMTRA"].HeaderText = "Mã bài kiểm tra";
                        dataGridViewdiem.Columns["DIEM"].HeaderText = "Điểm";
                        dataGridViewdiem.Columns["NGAYCHAM"].HeaderText = "Ngày chấm";
                        dataGridViewdiem.Columns["NGAYCHAM"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                        dataGridViewdiem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ======================= CRUD HANDLERS =======================

        private void btnThemDiem_Click(object sender, EventArgs e)
        {
            int maghidanh, mabai; decimal diem;
            if (!ValidateInputs(out maghidanh, out mabai, out diem)) return;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    using (SqlCommand check = new SqlCommand(
                        "SELECT COUNT(*) FROM DIEMSO WHERE MAGHIDANH=@mag AND MABAIKIEMTRA=@mab", cn))
                    {
                        check.Parameters.AddWithValue("@mag", maghidanh);
                        check.Parameters.AddWithValue("@mab", mabai);

                        int cnt = Convert.ToInt32(check.ExecuteScalar());
                        if (cnt > 0)
                        {
                            MessageBox.Show("Bản ghi đã tồn tại. Nếu muốn chỉnh sửa, dùng Sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO DIEMSO (MAGHIDANH, MABAIKIEMTRA, DIEM) VALUES (@mag, @mab, @diem)", cn))
                    {
                        cmd.Parameters.AddWithValue("@mag", maghidanh);
                        cmd.Parameters.AddWithValue("@mab", mabai);
                        cmd.Parameters.AddWithValue("@diem", diem);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadData();
                UpdateChart();
                ClearInputs();
                MessageBox.Show("Thêm điểm thành công.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 547)
                    MessageBox.Show("Lỗi: Mã ghi danh hoặc Mã bài kiểm tra không tồn tại.", "Lỗi khóa ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Lỗi SQL: " + sqlex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            int maghidanh, mabai; decimal diem;
            if (!ValidateInputs(out maghidanh, out mabai, out diem)) return;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE DIEMSO SET DIEM=@diem, NGAYCHAM=SYSUTCDATETIME() WHERE MAGHIDANH=@mag AND MABAIKIEMTRA=@mab", cn))
                {
                    cmd.Parameters.AddWithValue("@mag", maghidanh);
                    cmd.Parameters.AddWithValue("@mab", mabai);
                    cmd.Parameters.AddWithValue("@diem", diem);

                    cn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    MessageBox.Show(rows > 0 ? "Sửa thành công." : "Không tìm thấy bản ghi.", "Kết quả", MessageBoxButtons.OK,
                        rows > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }

                LoadData();
                UpdateChart();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoadiem_Click(object sender, EventArgs e)
        {
            int maghidanh, mabai;
            if (!ValidateMaghiAndMabai(out maghidanh, out mabai)) return;

            var r = MessageBox.Show("Bạn có chắc muốn xóa điểm của Mã ghi danh=" + maghidanh +
                                    ", Mã bài=" + mabai + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM DIEMSO WHERE MAGHIDANH=@mag AND MABAIKIEMTRA=@mab", cn))
                {
                    cmd.Parameters.AddWithValue("@mag", maghidanh);
                    cmd.Parameters.AddWithValue("@mab", mabai);

                    cn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    MessageBox.Show(rows > 0 ? "Xóa thành công." : "Không tìm thấy bản ghi.", "Kết quả", MessageBoxButtons.OK,
                        rows > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }

                LoadData();
                UpdateChart();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            try
            {
                Form_DangNhap f = new Form_DangNhap();
                f.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chuyển về Form đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ======================= GRID BINDING =======================

        private void dataGridViewdiem_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewdiem.CurrentRow == null) return;

            var row = dataGridViewdiem.CurrentRow;

            if (row.Cells["MAGHIDANH"].Value != null && row.Cells["MAGHIDANH"].Value != DBNull.Value)
                txtmaghidanh.Text = row.Cells["MAGHIDANH"].Value.ToString();

            if (row.Cells["MABAIKIEMTRA"].Value != null && row.Cells["MABAIKIEMTRA"].Value != DBNull.Value)
                txtmabaikiemtra.Text = row.Cells["MABAIKIEMTRA"].Value.ToString();

            if (row.Cells["DIEM"].Value != null && row.Cells["DIEM"].Value != DBNull.Value)
            {
                decimal diem = Convert.ToDecimal(row.Cells["DIEM"].Value);
                txtdiem.Text = diem.ToString("0.00", CultureInfo.InvariantCulture);
            }
            else
            {
                txtdiem.Text = ""; // hoặc "Chưa có điểm"
            }
        }


        // ======================= VALIDATION =======================

        private bool ValidateInputs(out int maghidanh, out int mabai, out decimal diem)
        {
            maghidanh = 0; mabai = 0; diem = 0m;

            if (string.IsNullOrWhiteSpace(txtmaghidanh.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã ghi danh.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmaghidanh.Focus(); return false;
            }
            if (!int.TryParse(txtmaghidanh.Text.Trim(), out maghidanh))
            {
                MessageBox.Show("Mã ghi danh phải là số nguyên.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmaghidanh.Focus(); return false;
            }

            if (string.IsNullOrWhiteSpace(txtmabaikiemtra.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã bài kiểm tra.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmabaikiemtra.Focus(); return false;
            }
            if (!int.TryParse(txtmabaikiemtra.Text.Trim(), out mabai))
            {
                MessageBox.Show("Mã bài kiểm tra phải là số nguyên.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmabaikiemtra.Focus(); return false;
            }

            if (string.IsNullOrWhiteSpace(txtdiem.Text))
            {
                MessageBox.Show("Vui lòng nhập Điểm.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtdiem.Focus(); return false;
            }
            if (!decimal.TryParse(txtdiem.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out diem))
            {
                MessageBox.Show("Điểm không hợp lệ. Dùng dấu chấm cho phần thập phân nếu cần.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtdiem.Focus(); return false;
            }
            if (diem < 0 || diem > 100)
            {
                MessageBox.Show("Điểm phải trong khoảng 0 - 100.", "Sai giá trị", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtdiem.Focus(); return false;
            }

            return true;
        }

        private bool ValidateMaghiAndMabai(out int maghidanh, out int mabai)
        {
            maghidanh = 0; mabai = 0;

            if (string.IsNullOrWhiteSpace(txtmaghidanh.Text) || string.IsNullOrWhiteSpace(txtmabaikiemtra.Text))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập Mã ghi danh và Mã bài kiểm tra.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(txtmaghidanh.Text.Trim(), out maghidanh) || !int.TryParse(txtmabaikiemtra.Text.Trim(), out mabai))
            {
                MessageBox.Show("Mã phải là số nguyên.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ======================= UI HELPERS =======================

        private void ClearInputs()
        {
            txtmaghidanh.Clear();
            txtmabaikiemtra.Clear();
            txtdiem.Clear();
        }

        private void UpdateChart()
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT MAGHIDANH, MABAIKIEMTRA, DIEM FROM DIEMSO ORDER BY MAGHIDANH, MABAIKIEMTRA", cn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                chartDiem.Series.Clear();
                var series = chartDiem.Series.Add("Điểm");
                series.ChartType = SeriesChartType.Column;
                series.IsValueShownAsLabel = true;

                foreach (DataRow r in dt.Rows)
                {
                    string label = r["MAGHIDANH"].ToString() + "-" + r["MABAIKIEMTRA"].ToString();
                    decimal d = Convert.ToDecimal(r["DIEM"]);
                    series.Points.AddXY(label, (double)d);
                }

                if (chartDiem.ChartAreas.Count > 0)
                    chartDiem.ChartAreas[0].RecalculateAxesScale();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi cập nhật chart: " + ex.Message);
            }
        }

        private void dataGridViewdiem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo click không phải vào header
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewdiem.Rows[e.RowIndex];

                if (row.Cells["MAGHIDANH"].Value != null)
                    txtmaghidanh.Text = row.Cells["MAGHIDANH"].Value.ToString();

                if (row.Cells["MABAIKIEMTRA"].Value != null)
                    txtmabaikiemtra.Text = row.Cells["MABAIKIEMTRA"].Value.ToString();

                if (row.Cells["DIEM"].Value != null)
                    txtdiem.Text = Convert.ToDecimal(row.Cells["DIEM"].Value)
                                   .ToString(CultureInfo.InvariantCulture);
            }
        }





    }
}
