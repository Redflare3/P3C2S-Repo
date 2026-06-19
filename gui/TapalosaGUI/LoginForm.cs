using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TapalosaGUI
{
    public class LoginForm : Form
    {
        private readonly ApiClient _apiClient;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblStatus;

        public LoginForm()
        {
            _apiClient = new ApiClient();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login Sistem Manajemen Tapalosa";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(420, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            var lblHeader = new Label
            {
                Text = "Login Sistem Manajemen Tapalosa",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            var lblUsername = new Label
            {
                Text = "Username",
                Location = new Point(30, 80),
                AutoSize = true
            };

            txtUsername = new TextBox
            {
                Location = new Point(30, 105),
                Width = 340
            };

            var lblPassword = new Label
            {
                Text = "Password",
                Location = new Point(30, 150),
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Location = new Point(30, 175),
                Width = 340,
                PasswordChar = '•'
            };

            btnLogin = new Button
            {
                Text = "Login",
                BackColor = Color.FromArgb(255, 140, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(30, 220),
                Size = new Size(340, 40),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += async (sender, e) => await PerformLoginAsync();

            lblStatus = new Label
            {
                Text = "Masukkan username dan password Anda.",
                Location = new Point(30, 270),
                AutoSize = false,
                Size = new Size(340, 30),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleLeft
            };

            this.Controls.Add(lblHeader);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblStatus);
            this.AcceptButton = btnLogin;
        }

        private async Task PerformLoginAsync()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblStatus.Text = "Username dan password tidak boleh kosong.";
                lblStatus.ForeColor = Color.Crimson;
                return;
            }

            btnLogin.Enabled = false;
            lblStatus.Text = "Memeriksa akun...";
            lblStatus.ForeColor = Color.DarkOrange;

            var result = await _apiClient.LoginAsync(username, password);
            if (result != null)
            {
                var dashboard = new DashboardForm(result.nama, result.role, result.idUser);
                this.Hide();
                dashboard.ShowDialog();
                this.Close();
            }
            else
            {
                lblStatus.Text = "Login gagal. Periksa username, password, atau backend Anda.";
                lblStatus.ForeColor = Color.Crimson;
                btnLogin.Enabled = true;
            }
        }
    }
}
