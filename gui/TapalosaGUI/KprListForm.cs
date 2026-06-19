using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TapalosaCommon.Models;


namespace TapalosaGUI
{
    public class KprListForm : Form
    {
        private readonly ApiClient _apiClient;
        private readonly string _username;
        private readonly string _role;
        private DataGridView dgvKpr;
        private Button btnRefresh;
        private Label lblMessage;

        public KprListForm(string username, string role)
        {
            _username = username;
            _role = role;
            _apiClient = new ApiClient();
            InitializeComponent();
            this.Shown += async (sender, e) => await LoadKprDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Daftar Pengajuan KPR";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(820, 520);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var lblHeader = new Label
            {
                Text = "Daftar Pengajuan KPR",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            dgvKpr = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(760, 380),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnRefresh = new Button
            {
                Text = "Muat Ulang Daftar",
                Location = new Point(20, 455),
                Size = new Size(160, 36),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += async (sender, args) => await LoadKprDataAsync();

            lblMessage = new Label
            {
                Text = "Data pengajuan akan muncul di tabel di atas.",
                Location = new Point(200, 461),
                Size = new Size(580, 30),
                ForeColor = Color.DimGray
            };

            this.Controls.Add(lblHeader);
            this.Controls.Add(dgvKpr);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(lblMessage);
        }

        private async Task LoadKprDataAsync()
        {
            btnRefresh.Enabled = false;
            lblMessage.Text = "Memuat data pengajuan...";
            lblMessage.ForeColor = Color.DarkOrange;

            var pengajuan = await _apiClient.GetPengajuanKprAsync(_role, _username);
            if (pengajuan != null)
            {
                dgvKpr.DataSource = pengajuan;
                lblMessage.Text = $"Menampilkan {pengajuan.Count} pengajuan.";
                lblMessage.ForeColor = Color.ForestGreen;
            }
            else
            {
                dgvKpr.DataSource = new List<PengajuanKpr>();
                lblMessage.Text = "Gagal memuat data KPR. Pastikan backend aktif.";
                lblMessage.ForeColor = Color.Crimson;
            }

            btnRefresh.Enabled = true;
        }
    }
}
