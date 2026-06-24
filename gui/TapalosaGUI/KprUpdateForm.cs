using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TapalosaGUI
{
    public class KprUpdateForm : Form
    {
        private readonly ApiClient _apiClient;
        private readonly string _username;
        private TextBox txtPengajuanId;
        private ComboBox cbStatus;
        private Button btnSubmit;

        public KprUpdateForm(string username)
        {
            _username = username;
            _apiClient = new ApiClient();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Update Status KPR";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(360, 300);
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            var lblHeader = new Label
            {
                Text = "Update Status Pengajuan",
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

            var lblStatus = new Label
            {
                Text = "Status Baru:",
                Location = new Point(20, 125),
                AutoSize = true
            };
            cbStatus = new ComboBox
            {
                Location = new Point(20, 150),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbStatus.Items.AddRange(new object[] { "Disetujui", "Ditolak", "Dibatalkan" });
            cbStatus.SelectedIndex = 0;

            btnSubmit = new Button
            {
                Text = "Perbarui Status",
                Location = new Point(20, 195),
                Size = new Size(300, 36),
                BackColor = Color.FromArgb(255, 140, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += async (sender, args) => await SubmitStatusAsync();

            this.Controls.Add(lblHeader);
            this.Controls.Add(lblPengajuanId);
            this.Controls.Add(txtPengajuanId);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cbStatus);
            this.Controls.Add(btnSubmit);
        }

        private async Task SubmitStatusAsync()
        {
            if (!int.TryParse(txtPengajuanId.Text.Trim(), out int idPengajuan))
            {
                MessageBox.Show("ID Pengajuan harus angka.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string status = cbStatus.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Pilih status baru.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string aksi = status switch
            {
                "Disetujui" => "Setujui",
                "Ditolak" => "Tolak",
                "Dibatalkan" => "Batalkan",
                _ => status
            };

            bool success = await _apiClient.UpdateStatusKprAsync(_username, idPengajuan, aksi);
            if (success)
            {
                MessageBox.Show("Status pengajuan berhasil diperbarui.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Gagal memperbarui status. Periksa backend.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
