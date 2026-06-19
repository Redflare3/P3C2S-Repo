using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TapalosaCommon.Models;


namespace TapalosaGUI
{
    public class KprAddForm : Form
    {
        private readonly ApiClient _apiClient;
        private readonly string _username;
        private readonly int _userId;
        private TextBox txtPengajuanId;
        private TextBox txtRumahId;
        private Button btnSubmit;

        public KprAddForm(string username, int userId)
        {
            _username = username;
            _userId = userId;
            _apiClient = new ApiClient();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Tambah Pengajuan KPR";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(360, 280);
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            var lblHeader = new Label
            {
                Text = "Tambah Pengajuan KPR",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var lblPengajuanId = new Label
            {
                Text = "ID Pengajuan:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            txtPengajuanId = new TextBox
            {
                Location = new Point(20, 85),
                Width = 300
            };

            var lblRumahId = new Label
            {
                Text = "ID Rumah:",
                Location = new Point(20, 125),
                AutoSize = true
            };
            txtRumahId = new TextBox
            {
                Location = new Point(20, 150),
                Width = 300
            };

            btnSubmit = new Button
            {
                Text = "Tambah Pengajuan",
                Location = new Point(20, 190),
                Size = new Size(300, 36),
                BackColor = Color.FromArgb(0, 153, 51),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += async (sender, args) => await SubmitPengajuanAsync();

            this.Controls.Add(lblHeader);
            this.Controls.Add(lblPengajuanId);
            this.Controls.Add(txtPengajuanId);
            this.Controls.Add(lblRumahId);
            this.Controls.Add(txtRumahId);
            this.Controls.Add(btnSubmit);
        }

        private async Task SubmitPengajuanAsync()
        {
            if (!int.TryParse(txtPengajuanId.Text.Trim(), out int idPengajuan))
            {
                MessageBox.Show("ID Pengajuan harus angka.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtRumahId.Text.Trim(), out int idRumah))
            {
                MessageBox.Show("ID Rumah harus angka.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pengajuan = new PengajuanKpr
            {
                idPengajuan = idPengajuan,
                idUser = _userId,
                idRumah = idRumah,
                tanggalPengajuan = DateTime.Now,
                tanggalUpdate = DateTime.Now,
                statusPengajuan = "MenungguPersetujuan"
            };

            bool success = await _apiClient.TambahPengajuanKprAsync(_username, pengajuan);
            if (success)
            {
                MessageBox.Show("Pengajuan berhasil dibuat.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Gagal membuat pengajuan. Periksa backend.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
