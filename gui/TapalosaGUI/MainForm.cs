using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using KatalogRumahLib;
using ManageKPRLib;
using TapalosaCommon.Models;

namespace TapalosaGUI
{
    public class MainForm : Form
    {
        private readonly ApiClient _apiClient;
        private readonly KatalogRumah _katalog;
        private readonly KprManager _kprManager;

        // Current Session State
        private string _currentUser = "Not Logged In";
        private string _currentRole = "None";
        private int _currentUserId = 0;

        // Visual Controls
        private TabControl tabControl;
        private TabPage tabLogin;
        private TabPage tabKatalog;
        private TabPage tabKPR;
        private TabPage tabBank;

        // Header and Status
        private Panel headerPanel;
        private Label lblTitle;
        private Label lblSessionInfo;

        // Login Controls
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnLogout;
        private Label lblLoginStatus;

        // Katalog Controls
        private DataGridView dgvKatalog;
        private TextBox txtNewRumahId;
        private TextBox txtNewRumahBlok;
        private TextBox txtNewRumahHarga;
        private Button btnAddRumah;
        private TextBox txtManageRumahId;
        private ComboBox cbRumahStatus;
        private Button btnGetRumahById;
        private Button btnUpdateRumahStatus;
        private Button btnDeleteRumah;
        private TextBox txtSearchKategori;
        private Button btnSearchKategori;
        private Label lblSearchKategoriResult;

        // KPR Controls
        private DataGridView dgvKpr;
        private TextBox txtNewKprId;
        private TextBox txtNewKprRumahId;
        private Button btnApplyKpr;
        private TextBox txtSearchKprId;
        private Button btnSearchPengajuan;
        private Button btnDeletePengajuan;
        private Button btnCheckRumahByPengajuan;
        private Label lblPengajuanSearchResult;
        private ComboBox cbKprAction;
        private TextBox txtKprProcessId;
        private Button btnProcessKpr;

        // Bank Controls
        private ComboBox cbBank;
        private Button btnSearchBank;
        private Label lblBankResultName;
        private Label lblBankResultBunga;
        private ListBox lbBankResultTahapan;

        // Theme Colors
        private readonly Color ThemeOrange = Color.FromArgb(255, 140, 0);
        private readonly Color ThemeYellow = Color.FromArgb(255, 193, 7);
        private readonly Color ThemeLightBg = Color.WhiteSmoke;
        private readonly Color ThemeTextDark = Color.FromArgb(60, 60, 60);

        public MainForm()
        {
            _apiClient = new ApiClient();
            _katalog = new KatalogRumah();
            _kprManager = KprManager.Instance;

            InitializeComponent();
            UpdateSessionUI();
        }

        private void InitializeComponent()
        {
            this.Text = "Tapalosa System - Portal Utama GUI";
            this.Size = new Size(1000, 720); // Enlarged for better layout
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeLightBg;
            this.ForeColor = ThemeTextDark;
            this.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);

            // 1. Header Panel (With Gradient Background)
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                Padding = new Padding(15)
            };
            headerPanel.Paint += HeaderPanel_Paint; // Custom gradient paint

            lblTitle = new Label
            {
                Text = "TAPALOSA PORTAL UTAMA",
                Font = new Font("Segoe UI Black", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(20, 15)
            };

            lblSessionInfo = new Label
            {
                Text = "Session: Not Logged In",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Italic),
                ForeColor = Color.WhiteSmoke,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(22, 50)
            };

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblSessionInfo);
            this.Controls.Add(headerPanel);

            // 2. Tab Control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 85),
                Padding = new Point(15, 8),
                Font = new Font("Segoe UI Semibold", 10F)
            };
            this.Controls.Add(tabControl);

            // 3. Tab Pages
            tabLogin = new TabPage { Text = "Login & Sesi", BackColor = Color.White };
            SetupLoginTab();
            tabControl.TabPages.Add(tabLogin);

            tabKatalog = new TabPage { Text = "Katalog Rumah", BackColor = ThemeLightBg };
            SetupKatalogTab();
            tabControl.TabPages.Add(tabKatalog);

            tabKPR = new TabPage { Text = "Pengajuan KPR", BackColor = ThemeLightBg };
            SetupKPRTab();
            tabControl.TabPages.Add(tabKPR);

            tabBank = new TabPage { Text = "Informasi Bank", BackColor = ThemeLightBg };
            SetupBankTab();
            tabControl.TabPages.Add(tabBank);
        }

        // Gradient Background for Header
        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                headerPanel.ClientRectangle, ThemeOrange, ThemeYellow, LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
            }
        }

        // Helper to Create Modern Flat Buttons
        private Button CreateStyledButton(string text, Point loc, Size size, Color bgColor)
        {
            return new Button
            {
                Text = text,
                Location = loc,
                Size = size,
                BackColor = bgColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.75F),
                Cursor = Cursors.Hand
            };
        }

        private void SetupLoginTab()
        {
            var gpLogin = new GroupBox
            {
                Text = "🔑 Form Login Pengguna",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = ThemeOrange,
                Size = new Size(420, 280),
                Location = new Point(280, 120), // Centered roughly
                Padding = new Padding(15),
                BackColor = Color.White
            };

            var lblUser = new Label { Text = "Username:", Location = new Point(30, 60), AutoSize = true, ForeColor = ThemeTextDark };
            txtUsername = new TextBox
            {
                Location = new Point(130, 58),
                Width = 240,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblPass = new Label { Text = "Password:", Location = new Point(30, 110), AutoSize = true, ForeColor = ThemeTextDark };
            txtPassword = new TextBox
            {
                Location = new Point(130, 108),
                Width = 240,
                PasswordChar = '•',
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnLogin = CreateStyledButton("Login", new Point(130, 160), new Size(115, 40), ThemeOrange);
            btnLogin.Click += async (s, e) => await PerformLogin();

            btnLogout = CreateStyledButton("Logout", new Point(255, 160), new Size(115, 40), Color.Gray);
            btnLogout.Enabled = false;
            btnLogout.Click += (s, e) => PerformLogout();

            lblLoginStatus = new Label
            {
                Text = "Silakan login untuk mengakses fitur lainnya.",
                ForeColor = Color.Orange,
                Location = new Point(20, 220),
                Width = 380,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic)
            };

            gpLogin.Controls.Add(lblUser);
            gpLogin.Controls.Add(txtUsername);
            gpLogin.Controls.Add(lblPass);
            gpLogin.Controls.Add(txtPassword);
            gpLogin.Controls.Add(btnLogin);
            gpLogin.Controls.Add(btnLogout);
            gpLogin.Controls.Add(lblLoginStatus);

            tabLogin.Controls.Add(gpLogin);
        }

        private void SetupKatalogTab()
        {
            // Grid of houses
            var lblGrid = new Label { Text = "Daftar Rumah (Katalog API):", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = ThemeOrange };
            dgvKatalog = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(500, 310),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.LightGray,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black, SelectionBackColor = ThemeYellow, SelectionForeColor = Color.Black },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = ThemeOrange, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 10F) },
                EnableHeadersVisualStyles = false
            };

            var btnRefreshKatalog = CreateStyledButton("Refresh Data", new Point(20, 370), new Size(500, 35), Color.MediumSeaGreen);
            btnRefreshKatalog.Click += async (s, e) => await RefreshKatalogData();

            // Add new house form
            var gpAdd = new GroupBox { Text = "🏠 Tambah Rumah Baru", ForeColor = ThemeOrange, Font = new Font("Segoe UI Semibold", 10F), Location = new Point(540, 20), Size = new Size(420, 220), BackColor = Color.White };

            var lblId = new Label { Text = "ID Rumah:", Location = new Point(20, 45), AutoSize = true, ForeColor = ThemeTextDark };
            txtNewRumahId = new TextBox { Location = new Point(130, 42), Width = 260, BorderStyle = BorderStyle.FixedSingle };

            var lblBlok = new Label { Text = "Blok:", Location = new Point(20, 85), AutoSize = true, ForeColor = ThemeTextDark };
            txtNewRumahBlok = new TextBox { Location = new Point(130, 82), Width = 260, BorderStyle = BorderStyle.FixedSingle };

            var lblHarga = new Label { Text = "Harga:", Location = new Point(20, 125), AutoSize = true, ForeColor = ThemeTextDark };
            txtNewRumahHarga = new TextBox { Location = new Point(130, 122), Width = 260, BorderStyle = BorderStyle.FixedSingle };

            btnAddRumah = CreateStyledButton("Tambah Rumah", new Point(130, 165), new Size(260, 35), ThemeOrange);
            btnAddRumah.Click += async (s, e) => await AddNewRumah();

            gpAdd.Controls.Add(lblId);
            gpAdd.Controls.Add(txtNewRumahId);
            gpAdd.Controls.Add(lblBlok);
            gpAdd.Controls.Add(txtNewRumahBlok);
            gpAdd.Controls.Add(lblHarga);
            gpAdd.Controls.Add(txtNewRumahHarga);
            gpAdd.Controls.Add(btnAddRumah);

            // Manage House
            var gpManageRumah = new GroupBox { Text = "⚙️ Kelola Rumah", ForeColor = ThemeOrange, Font = new Font("Segoe UI Semibold", 10F), Location = new Point(540, 250), Size = new Size(420, 230), BackColor = Color.White };

            var lblManageId = new Label { Text = "ID Rumah:", Location = new Point(20, 45), AutoSize = true, ForeColor = ThemeTextDark };
            txtManageRumahId = new TextBox { Location = new Point(130, 42), Width = 260, BorderStyle = BorderStyle.FixedSingle };

            btnGetRumahById = CreateStyledButton("Cari Rumah", new Point(130, 75), new Size(260, 30), Color.DeepSkyBlue);
            btnGetRumahById.Click += async (s, e) => await GetRumahById();

            var lblStatus = new Label { Text = "Status:", Location = new Point(20, 123), AutoSize = true, ForeColor = ThemeTextDark };
            cbRumahStatus = new ComboBox { Location = new Point(130, 120), Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            cbRumahStatus.Items.AddRange(new object[] { "Tersedia", "Tidak Tersedia" });
            cbRumahStatus.SelectedIndex = 0;

            btnUpdateRumahStatus = CreateStyledButton("Update Status", new Point(130, 155), new Size(125, 35), ThemeYellow);
            btnUpdateRumahStatus.ForeColor = ThemeTextDark; // Dark text for yellow button
            btnUpdateRumahStatus.Click += async (s, e) => await UpdateRumahStatusAsync();

            btnDeleteRumah = CreateStyledButton("Hapus Rumah", new Point(265, 155), new Size(125, 35), Color.Crimson);
            btnDeleteRumah.Click += async (s, e) => await DeleteRumahAsync();

            gpManageRumah.Controls.Add(lblManageId);
            gpManageRumah.Controls.Add(txtManageRumahId);
            gpManageRumah.Controls.Add(btnGetRumahById);
            gpManageRumah.Controls.Add(lblStatus);
            gpManageRumah.Controls.Add(cbRumahStatus);
            gpManageRumah.Controls.Add(btnUpdateRumahStatus);
            gpManageRumah.Controls.Add(btnDeleteRumah);

            // Search Category Bottom
            var gpSearch = new GroupBox { Text = "🔍 Pencarian Kategori Rumah", ForeColor = ThemeOrange, Font = new Font("Segoe UI Semibold", 10F), Location = new Point(20, 490), Size = new Size(940, 90), BackColor = Color.White };
            var lblCat = new Label { Text = "Kategori (Gold/Platinum/Diamond):", Location = new Point(20, 35), AutoSize = true, ForeColor = ThemeTextDark };
            txtSearchKategori = new TextBox { Location = new Point(260, 32), Width = 200, BorderStyle = BorderStyle.FixedSingle };

            btnSearchKategori = CreateStyledButton("Cari Info", new Point(480, 30), new Size(100, 30), ThemeYellow);
            btnSearchKategori.ForeColor = ThemeTextDark;
            btnSearchKategori.Click += (s, e) => SearchLocalKategori();

            lblSearchKategoriResult = new Label
            {
                Text = "Hasil pencarian akan tampil di sini...",
                Location = new Point(600, 35),
                Width = 320,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            gpSearch.Controls.Add(lblCat);
            gpSearch.Controls.Add(txtSearchKategori);
            gpSearch.Controls.Add(btnSearchKategori);
            gpSearch.Controls.Add(lblSearchKategoriResult);

            tabKatalog.Controls.Add(lblGrid);
            tabKatalog.Controls.Add(dgvKatalog);
            tabKatalog.Controls.Add(btnRefreshKatalog);
            tabKatalog.Controls.Add(gpAdd);
            tabKatalog.Controls.Add(gpManageRumah);
            tabKatalog.Controls.Add(gpSearch);
        }

        private void SetupKPRTab()
        {
            var lblGrid = new Label { Text = "Daftar Pengajuan KPR:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = ThemeOrange };
            dgvKpr = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(500, 250),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.LightGray,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black, SelectionBackColor = ThemeYellow, SelectionForeColor = Color.Black },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = ThemeOrange, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 10F) },
                EnableHeadersVisualStyles = false
            };

            var btnRefreshKpr = CreateStyledButton("Refresh Data", new Point(20, 310), new Size(500, 35), Color.MediumSeaGreen);
            btnRefreshKpr.Click += async (s, e) => await RefreshKprData();

            // Submit new KPR
            var gpApply = new GroupBox { Text = "📝 Ajukan KPR Baru", ForeColor = ThemeOrange, Font = new Font("Segoe UI Semibold", 10F), Location = new Point(540, 20), Size = new Size(420, 200), BackColor = Color.White };

            var lblId = new Label { Text = "ID Pengajuan:", Location = new Point(20, 45), AutoSize = true, ForeColor = ThemeTextDark };
            txtNewKprId = new TextBox { Location = new Point(140, 42), Width = 250, BorderStyle = BorderStyle.FixedSingle };

            var lblRumahId = new Label { Text = "ID Rumah:", Location = new Point(20, 85), AutoSize = true, ForeColor = ThemeTextDark };
            txtNewKprRumahId = new TextBox { Location = new Point(140, 82), Width = 250, BorderStyle = BorderStyle.FixedSingle };

            btnApplyKpr = CreateStyledButton("Ajukan KPR", new Point(140, 130), new Size(250, 40), ThemeOrange);
            btnApplyKpr.Click += async (s, e) => await ApplyNewKpr();

            gpApply.Controls.Add(lblId);
            gpApply.Controls.Add(txtNewKprId);
            gpApply.Controls.Add(lblRumahId);
            gpApply.Controls.Add(txtNewKprRumahId);
            gpApply.Controls.Add(btnApplyKpr);

            // Manage Pengajuan
            var gpManagePengajuan = new GroupBox { Text = "🔎 Kelola Pengajuan", ForeColor = ThemeOrange, Font = new Font("Segoe UI Semibold", 10F), Location = new Point(540, 230), Size = new Size(420, 220), BackColor = Color.White };
            var lblSearchId = new Label { Text = "ID Pengajuan:", Location = new Point(20, 45), AutoSize = true, ForeColor = ThemeTextDark };
            txtSearchKprId = new TextBox { Location = new Point(140, 42), Width = 250, BorderStyle = BorderStyle.FixedSingle };

            btnSearchPengajuan = CreateStyledButton("Cari Pengajuan", new Point(140, 75), new Size(250, 30), Color.DeepSkyBlue);
            btnSearchPengajuan.Click += async (s, e) => await SearchPengajuanById();

            btnCheckRumahByPengajuan = CreateStyledButton("Cek Detail Rumah", new Point(140, 110), new Size(120, 35), ThemeYellow);
            btnCheckRumahByPengajuan.ForeColor = ThemeTextDark;
            btnCheckRumahByPengajuan.Click += async (s, e) => await CheckRumahByPengajuanId();

            btnDeletePengajuan = CreateStyledButton("Hapus", new Point(270, 110), new Size(120, 35), Color.Crimson);
            btnDeletePengajuan.Click += async (s, e) => await DeletePengajuanAsync();

            lblPengajuanSearchResult = new Label
            {
                Text = "Hasil pencarian akan ditampilkan di sini.",
                Location = new Point(20, 165),
                Size = new Size(380, 40),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic)
            };

            gpManagePengajuan.Controls.Add(lblSearchId);
            gpManagePengajuan.Controls.Add(txtSearchKprId);
            gpManagePengajuan.Controls.Add(btnSearchPengajuan);
            gpManagePengajuan.Controls.Add(btnDeletePengajuan);
            gpManagePengajuan.Controls.Add(btnCheckRumahByPengajuan);
            gpManagePengajuan.Controls.Add(lblPengajuanSearchResult);

            // Automata 
            var gpAutomata = new GroupBox { Text = "🔄 Proses Status KPR (Admin)", ForeColor = ThemeOrange, Font = new Font("Segoe UI Semibold", 10F), Location = new Point(20, 370), Size = new Size(500, 140), BackColor = Color.White };

            var lblPid = new Label { Text = "ID Pengajuan:", Location = new Point(20, 45), AutoSize = true, ForeColor = ThemeTextDark };
            txtKprProcessId = new TextBox { Location = new Point(140, 42), Width = 100, BorderStyle = BorderStyle.FixedSingle };

            var lblAct = new Label { Text = "Aksi:", Location = new Point(260, 45), AutoSize = true, ForeColor = ThemeTextDark };
            cbKprAction = new ComboBox
            {
                Location = new Point(310, 42),
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbKprAction.Items.AddRange(new object[] { "Setujui", "Tolak", "Batalkan" });
            cbKprAction.SelectedIndex = 0;

            btnProcessKpr = CreateStyledButton("Jalankan Transisi State", new Point(140, 85), new Size(330, 35), ThemeOrange);
            btnProcessKpr.Click += async (s, e) => await ProcessKprStatus();

            gpAutomata.Controls.Add(lblPid);
            gpAutomata.Controls.Add(txtKprProcessId);
            gpAutomata.Controls.Add(lblAct);
            gpAutomata.Controls.Add(cbKprAction);
            gpAutomata.Controls.Add(btnProcessKpr);

            tabKPR.Controls.Add(lblGrid);
            tabKPR.Controls.Add(dgvKpr);
            tabKPR.Controls.Add(btnRefreshKpr);
            tabKPR.Controls.Add(gpApply);
            tabKPR.Controls.Add(gpManagePengajuan);
            tabKPR.Controls.Add(gpAutomata);
        }

        private void SetupBankTab()
        {
            var gpBank = new GroupBox { Text = "🏦 Informasi Bunga & Tahapan KPR Bank", Font = new Font("Segoe UI Semibold", 11F), ForeColor = ThemeOrange, Location = new Point(40, 30), Size = new Size(900, 500), BackColor = Color.White };

            var lblSelect = new Label { Text = "Pilih Bank KPR:", Location = new Point(220, 50), AutoSize = true, ForeColor = ThemeTextDark };
            cbBank = new ComboBox
            {
                Location = new Point(350, 47),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cbBank.Items.AddRange(new object[] { "BCA", "Mandiri", "BNI" });
            cbBank.SelectedIndex = 0;

            btnSearchBank = CreateStyledButton("Cari Info Bank", new Point(570, 45), new Size(130, 32), ThemeOrange);
            btnSearchBank.Click += (s, e) => SearchBankInfo();

            var panelResult = new Panel { Location = new Point(40, 110), Size = new Size(820, 360), BackColor = ThemeLightBg };

            var lblR1 = new Label { Text = "Nama Bank  :", Location = new Point(30, 30), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = ThemeTextDark };
            lblBankResultName = new Label { Text = "-", Location = new Point(160, 30), AutoSize = true, ForeColor = ThemeOrange, Font = new Font("Segoe UI Black", 12F) };

            var lblR2 = new Label { Text = "Suku Bunga :", Location = new Point(30, 70), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = ThemeTextDark };
            lblBankResultBunga = new Label { Text = "-", Location = new Point(160, 70), AutoSize = true, ForeColor = Color.MediumSeaGreen, Font = new Font("Segoe UI Black", 12F) };

            var lblR3 = new Label { Text = "Tahapan KPR:", Location = new Point(30, 120), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = ThemeTextDark };
            lbBankResultTahapan = new ListBox
            {
                Location = new Point(160, 120),
                Size = new Size(620, 210),
                BackColor = Color.White,
                ForeColor = ThemeTextDark,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular)
            };

            panelResult.Controls.Add(lblR1);
            panelResult.Controls.Add(lblBankResultName);
            panelResult.Controls.Add(lblR2);
            panelResult.Controls.Add(lblBankResultBunga);
            panelResult.Controls.Add(lblR3);
            panelResult.Controls.Add(lbBankResultTahapan);

            gpBank.Controls.Add(lblSelect);
            gpBank.Controls.Add(cbBank);
            gpBank.Controls.Add(btnSearchBank);
            gpBank.Controls.Add(panelResult);

            tabBank.Controls.Add(gpBank);
        }

        // Logic Actions

        private void UpdateSessionUI()
        {
            lblSessionInfo.Text = $"User: {_currentUser} | Role: {_currentRole}";

            bool isLoggedIn = _currentUser != "Not Logged In" && _currentRole != "None";

            // Enable tabs on login
            tabKatalog.Enabled = isLoggedIn;
            tabKPR.Enabled = isLoggedIn;
            tabBank.Enabled = isLoggedIn;

            btnLogin.Enabled = !isLoggedIn;
            btnLogout.Enabled = isLoggedIn;

            txtUsername.Enabled = !isLoggedIn;
            txtPassword.Enabled = !isLoggedIn;

            if (isLoggedIn)
            {
                lblLoginStatus.Text = $"Login Berhasil! Sesi aktif: {_currentUser}";
                lblLoginStatus.ForeColor = Color.MediumSeaGreen;
            }
            else
            {
                lblLoginStatus.Text = "Silakan login untuk mengakses portal.";
                lblLoginStatus.ForeColor = Color.Orange;
            }
        }

        private async Task PerformLogin()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username dan Password tidak boleh kosong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblLoginStatus.Text = "Menghubungi Backend API...";
            lblLoginStatus.ForeColor = Color.Gray;

            var res = await _apiClient.LoginAsync(username, password);
            if (res != null)
            {
                _currentUser = res.nama;
                _currentRole = res.role;
                _currentUserId = res.idUser;
                UpdateSessionUI();

                // Auto-refresh data
                await RefreshKatalogData();
                await RefreshKprData();
                tabControl.SelectedTab = tabKPR;
            }
            else
            {
                lblLoginStatus.Text = "Login gagal. Periksa username/password atau backend service (Port 5064).";
                lblLoginStatus.ForeColor = Color.Crimson;
                MessageBox.Show("Login Gagal! Pastikan database user.json dimuat dengan benar dan Backend (Port 5064) sudah aktif.", "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PerformLogout()
        {
            _currentUser = "Not Logged In";
            _currentRole = "None";
            _currentUserId = 0;
            txtUsername.Text = "";
            txtPassword.Text = "";
            UpdateSessionUI();

            dgvKatalog.DataSource = null;
            dgvKpr.DataSource = null;
            tabControl.SelectedTab = tabLogin;
        }

        private async Task RefreshKatalogData()
        {
            if (_currentUser == "Not Logged In") return;

            var rumahList = await _apiClient.GetKatalogRumahAsync(_currentRole, _currentUser);
            if (rumahList != null)
            {
                dgvKatalog.DataSource = rumahList;
            }
            else
            {
                MessageBox.Show("Gagal mengambil data dari Katalog API (Port 5064).", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task AddNewRumah()
        {
            if (!int.TryParse(txtNewRumahId.Text, out int id))
            {
                MessageBox.Show("ID Rumah harus angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string blok = txtNewRumahBlok.Text.Trim();
            if (string.IsNullOrEmpty(blok))
            {
                MessageBox.Show("Blok tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtNewRumahHarga.Text, out double harga))
            {
                MessageBox.Show("Harga tidak valid!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rumah = new Rumah { idRumah = id, blok = blok, harga = harga, statusKetersediaan = true };
            bool success = await _apiClient.TambahRumahAsync(rumah);
            if (success)
            {
                MessageBox.Show("Rumah baru berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNewRumahId.Text = "";
                txtNewRumahBlok.Text = "";
                txtNewRumahHarga.Text = "";
                await RefreshKatalogData();
            }
            else
            {
                MessageBox.Show("Gagal menambahkan rumah via API. Pastikan ID unik dan API Port 5064 hidup.", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchLocalKategori()
        {
            string category = txtSearchKategori.Text.Trim();
            if (string.IsNullOrEmpty(category))
            {
                lblSearchKategoriResult.Text = "Ketik kategori Gold, Platinum, atau Diamond.";
                lblSearchKategoriResult.ForeColor = Color.Orange;
                return;
            }

            string result = _katalog.CariInformasiRumah(category);
            lblSearchKategoriResult.Text = result;
            lblSearchKategoriResult.ForeColor = result.Contains("tidak ditemukan") ? Color.Crimson : Color.MediumSeaGreen;
        }

        private async Task RefreshKprData()
        {
            if (_currentUser == "Not Logged In") return;

            var pengajuanList = await _apiClient.GetPengajuanKprAsync(_currentRole, _currentUser);
            if (pengajuanList != null)
            {
                dgvKpr.DataSource = pengajuanList;
            }
            else
            {
                MessageBox.Show("Gagal mengambil data dari KPR API (Port 5064).", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ApplyNewKpr()
        {
            if (!int.TryParse(txtNewKprId.Text, out int idPengajuan))
            {
                MessageBox.Show("ID Pengajuan harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtNewKprRumahId.Text, out int idRumah))
            {
                MessageBox.Show("ID Rumah harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pengajuan = new PengajuanKpr
            {
                idPengajuan = idPengajuan,
                idUser = _currentUserId,
                idRumah = idRumah,
                tanggalPengajuan = DateTime.Now,
                tanggalUpdate = DateTime.Now,
                statusPengajuan = "MenungguPersetujuan"
            };

            bool success = await _apiClient.TambahPengajuanKprAsync(_currentUser, pengajuan);
            if (success)
            {
                MessageBox.Show("Pengajuan KPR Berhasil dikirim!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNewKprId.Text = "";
                txtNewKprRumahId.Text = "";
                await RefreshKprData();
            }
            else
            {
                MessageBox.Show("Gagal mengajukan KPR via API (Port 5064). Pastikan ID Pengajuan unik.", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ProcessKprStatus()
        {
            if (_currentRole.ToLower() != "admin")
            {
                MessageBox.Show("Hanya admin yang diperbolehkan memproses KPR!", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtKprProcessId.Text, out int idPengajuan))
            {
                MessageBox.Show("ID Pengajuan tidak valid!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string action = cbKprAction.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(action)) return;

            bool success = await _apiClient.UpdateStatusKprAsync(_currentUser, idPengajuan, action);
            if (success)
            {
                MessageBox.Show($"Sukses! State status pengajuan {idPengajuan} berhasil diupdate via Automata KPR API.", "Transisi Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtKprProcessId.Text = "";
                await RefreshKprData();
            }
            else
            {
                MessageBox.Show("Gagal melakukan transisi status. Kemungkinan rules automata dilanggar (misalnya, transisi dari 'Ditolak' tidak diperbolehkan).", "Automata Rule Violated", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task GetRumahById()
        {
            if (!int.TryParse(txtManageRumahId.Text, out int idRumah))
            {
                MessageBox.Show("ID Rumah harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rumah = await _apiClient.GetRumahByIdAsync(idRumah);
            if (rumah != null)
            {
                dgvKatalog.DataSource = new List<Rumah> { rumah };
                MessageBox.Show($"Rumah ditemukan: ID {rumah.idRumah}, Blok {rumah.blok}.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Rumah tidak ditemukan atau API gagal merespon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateRumahStatusAsync()
        {
            if (!int.TryParse(txtManageRumahId.Text, out int idRumah))
            {
                MessageBox.Show("ID Rumah harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool statusKetersediaan = cbRumahStatus.SelectedItem?.ToString() == "Tersedia";
            bool success = await _apiClient.UpdateRumahStatusAsync(idRumah, statusKetersediaan);
            if (success)
            {
                MessageBox.Show("Status rumah berhasil diperbarui.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RefreshKatalogData();
            }
            else
            {
                MessageBox.Show("Gagal memperbarui status rumah. Pastikan ID rumah valid dan backend hidup.", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task DeleteRumahAsync()
        {
            if (!int.TryParse(txtManageRumahId.Text, out int idRumah))
            {
                MessageBox.Show("ID Rumah harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Hapus rumah dengan ID {idRumah}?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            bool success = await _apiClient.DeleteRumahAsync(idRumah);
            if (success)
            {
                MessageBox.Show("Rumah berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtManageRumahId.Text = "";
                await RefreshKatalogData();
            }
            else
            {
                MessageBox.Show("Gagal menghapus rumah. Pastikan ID rumah valid dan backend hidup.", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SearchPengajuanById()
        {
            if (!int.TryParse(txtSearchKprId.Text, out int idPengajuan))
            {
                MessageBox.Show("ID Pengajuan harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pengajuan = await _apiClient.GetPengajuanKprByIdAsync(idPengajuan);
            if (pengajuan != null)
            {
                dgvKpr.DataSource = new List<PengajuanKpr> { pengajuan };
                lblPengajuanSearchResult.Text = $"Ditemukan: Pengajuan {idPengajuan}, status {pengajuan.statusPengajuan}.";
                lblPengajuanSearchResult.ForeColor = Color.MediumSeaGreen;
            }
            else
            {
                lblPengajuanSearchResult.Text = "Pengajuan tidak ditemukan atau API gagal merespon.";
                lblPengajuanSearchResult.ForeColor = Color.Crimson;
            }
        }

        private async Task DeletePengajuanAsync()
        {
            if (!int.TryParse(txtSearchKprId.Text, out int idPengajuan))
            {
                MessageBox.Show("ID Pengajuan harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Hapus pengajuan KPR ID {idPengajuan}?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            bool success = await _apiClient.DeletePengajuanKprAsync(idPengajuan);
            if (success)
            {
                MessageBox.Show("Pengajuan KPR berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearchKprId.Text = "";
                lblPengajuanSearchResult.Text = "Hasil pencarian akan ditampilkan di sini.";
                await RefreshKprData();
            }
            else
            {
                MessageBox.Show("Gagal menghapus pengajuan. Pastikan ID valid dan backend hidup.", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CheckRumahByPengajuanId()
        {
            if (!int.TryParse(txtSearchKprId.Text, out int idPengajuan))
            {
                MessageBox.Show("ID Pengajuan harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rumah = await _apiClient.GetRumahByPengajuanIdAsync(idPengajuan);
            if (rumah != null)
            {
                MessageBox.Show($"Rumah terkait pengajuan: ID {rumah.idRumah}, Blok {rumah.blok}.", "Info Rumah", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Gagal mendapatkan rumah untuk pengajuan ini. Pastikan ID valid dan backend hidup.", "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchBankInfo()
        {
            string bankName = cbBank.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(bankName)) return;

            try
            {
                var info = _kprManager.CariTahapanBank(bankName);
                lblBankResultName.Text = info.NamaBank;
                lblBankResultBunga.Text = $"{info.SukuBunga}% per Tahun";

                lbBankResultTahapan.Items.Clear();
                foreach (var tahap in info.Tahapan)
                {
                    lbBankResultTahapan.Items.Add(tahap);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Bank", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}