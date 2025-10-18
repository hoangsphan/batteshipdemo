using battleship.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace battleship
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Mini Battleship - Menu";
            this.Size = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);

            Label lblTitle = new Label();
            lblTitle.Text = "⚓ MINI BATTLESHIP ⚓";
            lblTitle.Font = new Font("Arial", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(80, 40);
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Chọn chế độ chơi:";
            lblSubtitle.Font = new Font("Arial", 14);
            lblSubtitle.ForeColor = Color.FromArgb(70, 70, 70);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(160, 100);
            this.Controls.Add(lblSubtitle);

            Button btnVsBot = new Button();
            btnVsBot.Text = "🤖 Chơi với BOT";
            btnVsBot.Font = new Font("Arial", 14, FontStyle.Bold);
            btnVsBot.Size = new Size(280, 60);
            btnVsBot.Location = new Point(110, 160);
            btnVsBot.BackColor = Color.FromArgb(255, 140, 0);
            btnVsBot.ForeColor = Color.White;
            btnVsBot.FlatStyle = FlatStyle.Flat;
            btnVsBot.Cursor = Cursors.Hand;
            btnVsBot.Click += (s, e) => StartGame(true);
            this.Controls.Add(btnVsBot);

            Button btnVsPlayer = new Button();
            btnVsPlayer.Text = "👥 Chơi 2 Người";
            btnVsPlayer.Font = new Font("Arial", 14, FontStyle.Bold);
            btnVsPlayer.Size = new Size(280, 60);
            btnVsPlayer.Location = new Point(110, 240);
            btnVsPlayer.BackColor = Color.FromArgb(60, 179, 113);
            btnVsPlayer.ForeColor = Color.White;
            btnVsPlayer.FlatStyle = FlatStyle.Flat;
            btnVsPlayer.Cursor = Cursors.Hand;
            btnVsPlayer.Click += (s, e) => StartGame(false);
            this.Controls.Add(btnVsPlayer);
        }

        private void StartGame(bool vsBot)
        {
            this.Hide();
            BattleshipForm gameForm = new BattleshipForm(vsBot);
            gameForm.FormClosed += (s, e) => this.Show();
            gameForm.Show();
        }
    }
}

