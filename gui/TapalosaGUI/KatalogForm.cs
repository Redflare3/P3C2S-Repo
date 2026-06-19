using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TapalosaCommon.Models;


namespace TapalosaGUI
{
    public class KatalogForm : Form
    {
        private readonly ApiClient _apiClient;
        private readonly string _username;
        private readonly string _role;
        private DataGridView dgvKatalog;
        private Button btnRefresh;
        private Label lblMessage;

        public KatalogForm(string username, string role)
        {
            _username = username;
            _role = role;
            _apiClient = new ApiClient();
            InitializeComponent();
            this.Shown += async (sender, e) => await LoadDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Katalog Rumah Tapalosa";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(760, 520);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            var lblHeader = new Label
            {
                Text = "Katalog Rumah",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            dgvKatalog = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(700, 360),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnRefresh = new Button
            {
                Text = "Muat Ulang Data",
                Location = new Point(20, 435),
                Size = new Size(200, 38),
                BackColor = Color.FromArgb(255, 140, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += async (sender, args) => await LoadDataAsync();

            lblMessage = new Label
            {
                Text = "Data akan ditampilkan di tabel di atas.",
                Location = new Point(240, 442),
                AutoSize = true,
                ForeColor = Color.DimGray
            };

            this.Controls.Add(lblHeader);
            this.Controls.Add(dgvKatalog);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(lblMessage);
        }

        private async Task LoadDataAsync()
        {
            btnRefresh.Enabled = false;
            lblMessage.Text = "Memuat katalog...";
            lblMessage.ForeColor = Color.DarkOrange;

            var katalog = await _apiClient.GetKatalogRumahAsync(_role, _username);
            if (katalog != null)
            {
                dgvKatalog.DataSource = katalog;
                lblMessage.Text = $"Menampilkan {katalog.Count} rumah.";
                lblMessage.ForeColor = Color.ForestGreen;
            }
            else
            {
                dgvKatalog.DataSource = new List<Rumah>();
                lblMessage.Text = "Gagal memuat data katalog. Pastikan backend berjalan.";
                lblMessage.ForeColor = Color.Crimson;
            }

            btnRefresh.Enabled = true;
        }
    }
}
