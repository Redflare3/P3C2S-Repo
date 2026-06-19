using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ManageKPRLib;


namespace TapalosaGUI
{
    public class BankForm : Form
    {
        private readonly KprManager _kprManager;
        private ComboBox cbBank;
        private Button btnShow;
        private Label lblName;
        private Label lblRate;
        private ListBox lbSteps;

        public BankForm()
        {
            _kprManager = KprManager.Instance;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Informasi Bank Tapalosa";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(640, 420);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            var lblHeader = new Label
            {
                Text = "Info Bank KPR",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var lblSelect = new Label
            {
                Text = "Pilih Bank",
                Location = new Point(20, 70),
                AutoSize = true
            };

            cbBank = new ComboBox
            {
                Location = new Point(110, 66),
                Size = new Size(240, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbBank.Items.AddRange(new object[] { "BCA", "Mandiri", "BNI" });
            cbBank.SelectedIndex = 0;

            btnShow = new Button
            {
                Text = "Tampilkan Info",
                Location = new Point(370, 64),
                Size = new Size(230, 30),
                BackColor = Color.FromArgb(0, 153, 51),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnShow.FlatAppearance.BorderSize = 0;
            btnShow.Click += (sender, args) => ShowBankInfo();

            lblName = new Label
            {
                Text = "Nama Bank: -",
                Location = new Point(20, 120),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            lblRate = new Label
            {
                Text = "Suku Bunga: -",
                Location = new Point(20, 155),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            var lblSteps = new Label
            {
                Text = "Tahapan KPR:",
                Location = new Point(20, 190),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            lbSteps = new ListBox
            {
                Location = new Point(20, 220),
                Size = new Size(580, 140),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            this.Controls.Add(lblHeader);
            this.Controls.Add(lblSelect);
            this.Controls.Add(cbBank);
            this.Controls.Add(btnShow);
            this.Controls.Add(lblName);
            this.Controls.Add(lblRate);
            this.Controls.Add(lblSteps);
            this.Controls.Add(lbSteps);
        }

        private void ShowBankInfo()
        {
            string bank = cbBank.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(bank)) return;

            var info = _kprManager.CariTahapanBank(bank);
            lblName.Text = $"Nama Bank: {info.NamaBank}";
            lblRate.Text = $"Suku Bunga: {info.SukuBunga}%";
            lbSteps.Items.Clear();
            foreach (var step in info.Tahapan)
            {
                lbSteps.Items.Add(step);
            }
        }
    }
}
