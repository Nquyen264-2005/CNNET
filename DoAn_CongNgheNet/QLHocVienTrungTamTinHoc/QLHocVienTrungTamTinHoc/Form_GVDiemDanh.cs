using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace QLHocVienTrungTamTinHoc
{
    public partial class Form_GVDiemDanh : Form
    {
        private string connectionString =
            "Data Source=LAPTOP-OH9DSGKI\\SQLPROD2012;Initial Catalog=QLHocVienTinHoc;Integrated Security=True;TrustServerCertificate=True";

        public Form_GVDiemDanh()
        {
            InitializeComponent();
            this.Load += Form_GVDiemDanh_Load;
            btnThemDiemdanh.Click += btnThemDiemdanh_Click;
            btnXoadiemdanh.Click += btnXoadiemdanh_Click;
            btnSuadiemdanh.Click += btnSuadiemdanh_Click;
            dataGridViewdiemdanh.SelectionChanged += dataGridViewdiemdanh_SelectionChanged;
        }

        private void Form_GVDiemDanh_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT MAGHIDANH, NGAYHOC, TRANGTHAI, GHICHU FROM DIEMDANH ORDER BY MAGHIDANH, NGAYHOC", cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewdiemdanh.DataSource = dt;
            }
        }

        private void btnThemDiemdanh_Click(object sender, EventArgs e)
        {
            int maghidanh; DateTime ngayhoc; string trangthai, ghichu;
            if (!ValidateInputs(out maghidanh, out ngayhoc, out trangthai, out ghichu)) return;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                using (SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM DIEMDANH WHERE MAGHIDANH=@mag AND NGAYHOC=@ngay", cn))
                {
                    check.Parameters.AddWithValue("@mag", maghidanh);
                    check.Parameters.AddWithValue("@ngay", ngayhoc);
                    if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Bản ghi đã tồn tại.");
                        return;
                    }
                }
                using (SqlCommand cmd = new SqlCommand("INSERT INTO DIEMDANH (MAGHIDANH, NGAYHOC, TRANGTHAI, GHICHU) VALUES (@mag,@ngay,@tt,@gh)", cn))
                {
                    cmd.Parameters.AddWithValue("@mag", maghidanh);
                    cmd.Parameters.AddWithValue("@ngay", ngayhoc);
                    cmd.Parameters.AddWithValue("@tt", trangthai);
                    cmd.Parameters.AddWithValue("@gh", ghichu);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadData(); ClearInputs();
            MessageBox.Show("Thêm thành công.");
        }

        private void btnXoadiemdanh_Click(object sender, EventArgs e)
        {
            int maghidanh; DateTime ngayhoc;
            if (!ValidateMaghiAndNgay(out maghidanh, out ngayhoc)) return;

            var r = MessageBox.Show("Bạn có chắc muốn xóa Mã ghi danh=" + maghidanh +
                                    " ngày " + ngayhoc.ToString("yyyy-MM-dd"),
                                    "Xác nhận", MessageBoxButtons.YesNo);
            if (r != DialogResult.Yes) return;

            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM DIEMDANH WHERE MAGHIDANH=@mag AND NGAYHOC=@ngay", cn))
            {
                cmd.Parameters.AddWithValue("@mag", maghidanh);
                cmd.Parameters.AddWithValue("@ngay", ngayhoc);
                cn.Open();
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show(rows > 0 ? "Xóa thành công." : "Không tìm thấy bản ghi.");
            }
            LoadData(); ClearInputs();
        }

        private void btnSuadiemdanh_Click(object sender, EventArgs e)
        {
            int maghidanh; DateTime ngayhoc; string trangthai, ghichu;
            if (!ValidateInputs(out maghidanh, out ngayhoc, out trangthai, out ghichu)) return;

            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UPDATE DIEMDANH SET TRANGTHAI=@tt, GHICHU=@gh WHERE MAGHIDANH=@mag AND NGAYHOC=@ngay", cn))
            {
                cmd.Parameters.AddWithValue("@mag", maghidanh);
                cmd.Parameters.AddWithValue("@ngay", ngayhoc);
                cmd.Parameters.AddWithValue("@tt", trangthai);
                cmd.Parameters.AddWithValue("@gh", ghichu);
                cn.Open();
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show(rows > 0 ? "Sửa thành công." : "Không tìm thấy bản ghi.");
            }
            LoadData(); ClearInputs();
        }

        private void dataGridViewdiemdanh_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewdiemdanh.CurrentRow == null) return;
            var row = dataGridViewdiemdanh.CurrentRow;
            txtmaghidanh.Text = row.Cells["MAGHIDANH"].Value.ToString();
            if (row.Cells["NGAYHOC"].Value != null && row.Cells["NGAYHOC"].Value != DBNull.Value)
            {
                DateTime ngayHoc = Convert.ToDateTime(row.Cells["NGAYHOC"].Value);
                maskedTextBoxngayhoc.Text = ngayHoc.ToString("yyyy-MM-dd");
            }
            else
            {
                maskedTextBoxngayhoc.Text = ""; // hoặc "Chưa có ngày học"
            }
            txttrangthai.Text = row.Cells["TRANGTHAI"].Value.ToString();
            txtghichu.Text = row.Cells["GHICHU"].Value.ToString();
        }

        private bool ValidateInputs(out int maghidanh, out DateTime ngayhoc, out string trangthai, out string ghichu)
        {
            maghidanh = 0; ngayhoc = DateTime.MinValue;
            trangthai = txttrangthai.Text.Trim();
            ghichu = txtghichu.Text.Trim();

            if (!int.TryParse(txtmaghidanh.Text.Trim(), out maghidanh)) return false;
            if (!DateTime.TryParse(maskedTextBoxngayhoc.Text.Trim(), out ngayhoc)) return false;
            return true;
        }

        private bool ValidateMaghiAndNgay(out int maghidanh, out DateTime ngayhoc)
        {
            maghidanh = 0; ngayhoc = DateTime.MinValue;
            if (!int.TryParse(txtmaghidanh.Text.Trim(), out maghidanh)) return false;
            if (!DateTime.TryParse(maskedTextBoxngayhoc.Text.Trim(), out ngayhoc)) return false;
            return true;
        }

        private void ClearInputs()
        {
            txtmaghidanh.Clear();
            maskedTextBoxngayhoc.Clear();
            txttrangthai.Clear();
            txtghichu.Clear();
        }
    }
}
