using System;
using System.Drawing;
using System.Windows.Forms;


namespace TapalosaGUI
{
    public class DashboardForm : Form
    {
        private readonly string _username;
        private readonly string _role;
        private readonly int _userId;

        public DashboardForm(string username, string role, int userId)
        {
            _username = username;
            _role = role;
            _userId = userId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Dashboard Tapalosa";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(480, 520);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var lblHeader = new Label
            {
                Text = "Selamat datang di Tapalosa",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            var lblSubtitle = new Label
            {
                Text = "Pilih fungsi yang ingin Anda buka di jendela terpisah.",
                AutoSize = true,
                Location = new Point(30, 60),
                ForeColor = Color.DimGray
            };

            var lblUser = new Label
            {
                Text = $"Pengguna: {_username} | Role: {_role}",
                AutoSize = true,
                Location = new Point(30, 95),
                ForeColor = Color.DimGray
            };

            var btnKatalog = new Button
            {
                Text = "Buka Katalog Rumah",
                Location = new Point(30, 135),
                Size = new Size(400, 45),
                BackColor = Color.FromArgb(255, 140, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnKatalog.FlatAppearance.BorderSize = 0;
            btnKatalog.Click += (sender, args) => new KatalogForm(_username, _role).Show();

            var btnKprList = new Button
            {
                Text = "Daftar Pengajuan KPR",
                Location = new Point(30, 190),
                Size = new Size(400, 45),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnKprList.FlatAppearance.BorderSize = 0;
            btnKprList.Click += (sender, args) => new KprListForm(_username, _role).ShowDialog();

            var btnKprAdd = new Button
            {
                Text = "Tambah Pengajuan KPR",
                Location = new Point(30, 245),
                Size = new Size(400, 45),
                BackColor = Color.FromArgb(0, 153, 51),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnKprAdd.FlatAppearance.BorderSize = 0;
            btnKprAdd.Click += (sender, args) => new KprAddForm(_username, _userId).ShowDialog();

            var btnKprUpdate = new Button
            {
                Text = "Update Status KPR",
                Location = new Point(30, 300),
                Size = new Size(400, 45),
                BackColor = Color.FromArgb(255, 140, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnKprUpdate.FlatAppearance.BorderSize = 0;
            btnKprUpdate.Click += (sender, args) => new KprUpdateForm(_username).ShowDialog();

            var btnBank = new Button
            {
                Text = "Buka Info Bank",
                Location = new Point(30, 355),
                Size = new Size(400, 45),
                BackColor = Color.FromArgb(0, 153, 51),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBank.FlatAppearance.BorderSize = 0;
            btnBank.Click += (sender, args) => new BankForm().Show();

            var btnClose = new Button
            {
                Text = "Tutup Aplikasi",
                Location = new Point(30, 410),
                Size = new Size(400, 45),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (sender, args) => this.Close();

            this.Controls.Add(lblHeader);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblUser);
            this.Controls.Add(btnKatalog);
            this.Controls.Add(btnKprList);
            this.Controls.Add(btnKprAdd);
            this.Controls.Add(btnKprUpdate);
            this.Controls.Add(btnBank);
            this.Controls.Add(btnClose);
        }
    }
}
