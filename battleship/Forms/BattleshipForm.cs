using battleship.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace battleship.Forms
{
    public partial class BattleshipForm : Form
    {
        private GameState gameState;
        private Button[,] enemyGridButtons;
        private Button[,] myGridButtons;
        private Label lblPlayer;
        private Label lblStatus;
        private Label lblTurns;
        private Panel enemyPanel;
        private Panel myPanel;
        private const int CELL_SIZE = 50;
        private const int GRID_SIZE = 7;

        public BattleshipForm(bool vsBot)
        {
            InitializeComponent(vsBot);
            InitializeGame(vsBot);
        }

        private void InitializeComponent(bool vsBot)
        {
            this.Text = vsBot ? "Mini Battleship - Chơi với BOT" : "Mini Battleship - 2 Người";
            this.Size = new Size(980, 680);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);

            Label lblTitle = new Label();
            lblTitle.Text = vsBot ? "⚓ CHIẾN ĐẤU VỚI BOT ⚓" : "⚓ MINI BATTLESHIP ⚓";
            lblTitle.Font = new Font("Arial", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(300, 15);
            this.Controls.Add(lblTitle);

            lblPlayer = new Label();
            lblPlayer.Font = new Font("Arial", 14, FontStyle.Bold);
            lblPlayer.ForeColor = Color.FromArgb(220, 20, 60);
            lblPlayer.AutoSize = true;
            lblPlayer.Location = new Point(380, 55);
            this.Controls.Add(lblPlayer);

            lblStatus = new Label();
            lblStatus.Font = new Font("Arial", 11);
            lblStatus.ForeColor = Color.FromArgb(0, 100, 0);
            lblStatus.AutoSize = false;
            lblStatus.Size = new Size(900, 25);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Location = new Point(40, 85);
            this.Controls.Add(lblStatus);

            lblTurns = new Label();
            lblTurns.Font = new Font("Arial", 10);
            lblTurns.ForeColor = Color.FromArgb(70, 70, 70);
            lblTurns.AutoSize = true;
            lblTurns.Location = new Point(390, 115);
            this.Controls.Add(lblTurns);

            Label lblEnemy = new Label();
            lblEnemy.Text = vsBot ? "🎯 BÀN ĐỐI THỦ (BOT)" : "🎯 BÀN PHÒNG THỦ(PLAYER)";
            lblEnemy.Font = new Font("Arial", 12, FontStyle.Bold);
            lblEnemy.ForeColor = Color.FromArgb(220, 20, 60);
            lblEnemy.AutoSize = true;
            lblEnemy.Location = new Point(100, 145);
            this.Controls.Add(lblEnemy);

            Label lblMy = new Label();
            lblMy.Text = vsBot ? "🛡️ BÀN CỦA BẠN" : "🛡️ BÀN TẤN CÔNG(PLAYER)";
            lblMy.Font = new Font("Arial", 12, FontStyle.Bold);
            lblMy.ForeColor = Color.FromArgb(60, 179, 113);
            lblMy.AutoSize = true;
            lblMy.Location = new Point(590, 145);
            this.Controls.Add(lblMy);

            enemyPanel = new Panel();
            enemyPanel.Location = new Point(40, 180);
            enemyPanel.Size = new Size(CELL_SIZE * GRID_SIZE + 30, CELL_SIZE * GRID_SIZE + 30);
            enemyPanel.BackColor = Color.FromArgb(176, 196, 222);
            enemyPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(enemyPanel);

            myPanel = new Panel();
            myPanel.Location = new Point(480, 180);
            myPanel.Size = new Size(CELL_SIZE * GRID_SIZE + 30, CELL_SIZE * GRID_SIZE + 30);
            myPanel.BackColor = Color.FromArgb(176, 196, 222);
            myPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(myPanel);

            CreateGrids();

            Label lblLegend = new Label();
            lblLegend.Text = "💧 Nước    🚢 Tàu    💥 Trúng    ❌ Trượt";
            lblLegend.Font = new Font("Arial", 10);
            lblLegend.AutoSize = true;
            lblLegend.Location = new Point(350, 610);
            this.Controls.Add(lblLegend);
        }

        private void CreateGrids()
        {
            enemyGridButtons = new Button[GRID_SIZE, GRID_SIZE];
            myGridButtons = new Button[GRID_SIZE, GRID_SIZE];

            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(CELL_SIZE, CELL_SIZE);
                    btn.Location = new Point(j * CELL_SIZE + 15, i * CELL_SIZE + 15);
                    btn.Font = new Font("Arial", 16, FontStyle.Bold);
                    btn.BackColor = Color.FromArgb(135, 206, 250);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(70, 130, 180);
                    btn.FlatAppearance.BorderSize = 2;
                    btn.Text = "💧";
                    btn.Tag = new Point(i, j);
                    btn.Click += EnemyGridButton_Click;
                    btn.Cursor = Cursors.Hand;
                    enemyGridButtons[i, j] = btn;
                    enemyPanel.Controls.Add(btn);
                }
            }

            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(CELL_SIZE, CELL_SIZE);
                    btn.Location = new Point(j * CELL_SIZE + 15, i * CELL_SIZE + 15);
                    btn.Font = new Font("Arial", 16, FontStyle.Bold);
                    btn.BackColor = Color.FromArgb(135, 206, 250);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(70, 130, 180);
                    btn.FlatAppearance.BorderSize = 2;
                    btn.Text = "💧";
                    btn.Enabled = false;
                    myGridButtons[i, j] = btn;
                    myPanel.Controls.Add(btn);
                }
            }
        }

        private void InitializeGame(bool vsBot)
        {
            var mainRandom = new Random();
            gameState = new GameState(vsBot, mainRandom);
            gameState.PlayerBoard.RandomPlaceShips(mainRandom);
            gameState.OpponentBoard.RandomPlaceShips(mainRandom);
            UpdateBoards();
            UpdateUI();
            lblStatus.Text = "Sẵn sàng! Click vào bàn đối thủ để bắn!";
        }

        private void EnemyGridButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            // (SỬA) Chỉ kiểm tra xem nút có bị vô hiệu hóa không
            // Chúng ta không kiểm tra IsPlayerTurn ở đây
            if (!btn.Enabled) return;

            Point pos = (Point)btn.Tag;
            int row = pos.X;
            int col = pos.Y;

            if (gameState.OpponentBoard.DisplayGrid[row, col] != '~')
            {
                lblStatus.Text = "⚠ Ô này đã bắn rồi! Chọn ô khác.";
                lblStatus.ForeColor = Color.Orange;
                return;
            }

            // Một cú click của con người luôn là 'isPlayerAttacking = true'
            ProcessAttack(row, col, true);
        }

        private async void ProcessAttack(int row, int col, bool isPlayerAttacking)
        {
            // (SỬA) Nếu là con người bắn, tắt các nút ngay lập tức
            if (isPlayerAttacking)
            {
                DisableEnemyButtons();
            }

            var targetBoard = isPlayerAttacking ? gameState.OpponentBoard : gameState.PlayerBoard;
            var (hit, sunk, shipName) = targetBoard.Attack(row, col);

            if (isPlayerAttacking)
                gameState.PlayerTurns++;
            else
                gameState.OpponentTurns++;

            UpdateBoards(); // Cập nhật bàn cờ

            // (SỬA) Sửa logic hiển thị tên người tấn công
            string attacker;
            if (gameState.IsVsBot)
            {
                attacker = isPlayerAttacking ? "Bạn" : "BOT";
            }
            else
            {
                // Dựa vào IsPlayerTurn để biết A hay B đang bắn
                attacker = gameState.IsPlayerTurn ? "Người chơi A" : "Người chơi B";
            }

            if (hit)
            {
                lblStatus.Text = $"💥 {attacker} đã bắn trúng {shipName}!";
                if (sunk)
                    lblStatus.Text += $" ⚓ {shipName} đã chìm!";
                lblStatus.ForeColor = Color.Red;

                if (gameState.IsVsBot && !isPlayerAttacking)
                {
                    gameState.Bot.ProcessResult(row, col, true, gameState.PlayerBoard);
                }
            }
            else
            {
                lblStatus.Text = $"💧 {attacker} đã bắn trượt!";
                lblStatus.ForeColor = Color.Blue;

                if (gameState.IsVsBot && !isPlayerAttacking)
                {
                    gameState.Bot.ProcessResult(row, col, false, gameState.PlayerBoard);
                }
            }

            // Kiểm tra thắng
            if (targetBoard.AllShipsSunk())
            {
                // ... (Logic thắng và hỏi chơi lại vẫn giữ nguyên) ...
                DisableAllButtons();
                string winner = isPlayerAttacking ? (gameState.IsVsBot ? "BẠN" : "NGƯỜI CHƠI A") : (gameState.IsVsBot ? "BOT" : "NGƯỜI CHƠI B");
                MessageBox.Show(
                    $"🏆 {winner} THẮNG! 🏆\n\n" +
                    $"Số lượt chơi:\n" +
                    $"{(gameState.IsVsBot ? "Bạn" : "Người chơi A")}: {gameState.PlayerTurns} lượt\n" +
                    $"{(gameState.IsVsBot ? "BOT" : "Người chơi B")}: {gameState.OpponentTurns} lượt",
                    "Kết thúc trò chơi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                var result = MessageBox.Show("Chơi lại?", "Game Over", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var mainRandom = new Random();
                    ResetGame(mainRandom);
                }
                else
                {
                    this.Close();
                }
                return;
            }

            // (SỬA) Logic chuyển lượt tập trung
            if (isPlayerAttacking) // Con người (A hoặc B) vừa bắn
            {
                await System.Threading.Tasks.Task.Delay(1500); // Chờ xem kết quả

                if (gameState.IsVsBot)
                {
                    gameState.IsPlayerTurn = false; // Chuyển sang lượt Bot
                    UpdateUI();
                    BotTurn(); // Gọi Bot
                }
                else // Chơi PvP
                {
                    gameState.IsPlayerTurn = !gameState.IsPlayerTurn; // Đảo lượt A <-> B
                    UpdateUI();
                    SwitchPlayer(); // Gọi hàm tráo bàn cờ
                }
            }
            else // Bot vừa bắn
            {
                gameState.IsPlayerTurn = true; // Trả lượt về cho người
                UpdateUI();
                EnableEnemyButtons(); // Kích hoạt lại nút cho người
            }
        }

        private void BotTurn()
        {
            // (SỬA) Hàm này không quản lý state, chỉ lấy nước đi và gọi ProcessAttack
            lblStatus.Text = "🤖 BOT đang suy nghĩ...";
            lblStatus.ForeColor = Color.Orange;
            // Các nút đã bị tắt bởi ProcessAttack

            System.Threading.Tasks.Task.Delay(500).ContinueWith(t =>
            {
                this.Invoke(new Action(() =>
                {
                    var (row, col) = gameState.Bot.GetNextMove(gameState.PlayerBoard);
                    lblStatus.Text = $"🤖 BOT bắn vào ({row}, {col})!";

                    System.Threading.Tasks.Task.Delay(800).ContinueWith(t2 =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            // ProcessAttack sẽ xử lý việc kích hoạt lại nút
                            ProcessAttack(row, col, false);
                        }));
                    });
                }));
            });
        }

        private void SwitchPlayer()
        {
            Form switchForm = new Form();
            switchForm.Text = "Chuyển lượt";
            switchForm.Size = new Size(400, 250);
            switchForm.StartPosition = FormStartPosition.CenterScreen;
            switchForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            switchForm.MaximizeBox = false;
            switchForm.MinimizeBox = false;
            switchForm.BackColor = Color.FromArgb(240, 248, 255);

            Label lblSwitch = new Label();

            // (SỬA) Hiển thị đúng tên người chơi tiếp theo
            lblSwitch.Text = gameState.IsPlayerTurn ? "Chuẩn bị cho\nNgười chơi A" : "Chuẩn bị cho\nNgười chơi B";

            lblSwitch.Font = new Font("Arial", 18, FontStyle.Bold);
            lblSwitch.ForeColor = Color.FromArgb(25, 25, 112);
            lblSwitch.AutoSize = true;
            lblSwitch.Location = new Point(100, 50);
            switchForm.Controls.Add(lblSwitch);

            Button btnReady = new Button();
            btnReady.Text = "Sẵn sàng!";
            btnReady.Font = new Font("Arial", 12, FontStyle.Bold);
            btnReady.Size = new Size(150, 50);
            btnReady.Location = new Point(125, 140);
            btnReady.BackColor = Color.FromArgb(60, 179, 113);
            btnReady.ForeColor = Color.White;
            btnReady.FlatStyle = FlatStyle.Flat;
            btnReady.Cursor = Cursors.Hand;
            btnReady.Click += (s, e) => switchForm.Close();
            switchForm.Controls.Add(btnReady);

            switchForm.ShowDialog();

            // Swap boards
            var temp = gameState.PlayerBoard;
            gameState.PlayerBoard = gameState.OpponentBoard;
            gameState.OpponentBoard = temp;

            UpdateBoards();
            lblStatus.Text = "Click vào bàn đối thủ để bắn!";
            lblStatus.ForeColor = Color.FromArgb(0, 100, 0);

            // (SỬA) Luôn luôn kích hoạt lại các nút cho người chơi tiếp theo
            EnableEnemyButtons();
        }

        private void UpdateBoards()
        {
            // ... (Hàm này đã đúng, giữ nguyên) ...
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    Button btn = enemyGridButtons[i, j];
                    char cell = gameState.OpponentBoard.DisplayGrid[i, j];
                    if (cell == '~') { btn.Text = "💧"; btn.BackColor = Color.FromArgb(135, 206, 250); }
                    else if (cell == 'X') { btn.Text = "💥"; btn.BackColor = Color.FromArgb(255, 69, 0); }
                    else if (cell == 'O') { btn.Text = "❌"; btn.BackColor = Color.FromArgb(100, 149, 237); }
                }
            }
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    Button btn = myGridButtons[i, j];
                    char gridCell = gameState.PlayerBoard.Grid[i, j];
                    char displayCell = gameState.PlayerBoard.DisplayGrid[i, j];
                    if (displayCell == 'X') { btn.Text = "💥"; btn.BackColor = Color.FromArgb(255, 69, 0); }
                    else if (displayCell == 'O') { btn.Text = "❌"; btn.BackColor = Color.FromArgb(100, 149, 237); }
                    else if (gameState.IsVsBot && gridCell == 'S') { btn.Text = "🚢"; btn.BackColor = Color.FromArgb(34, 139, 34); }
                    else { btn.Text = "💧"; btn.BackColor = Color.FromArgb(135, 206, 250); }
                }
            }
        }

        private void UpdateUI()
        {
            // (SỬA) Logic hiển thị lượt này giờ đã chính xác
            if (gameState.IsVsBot)
            {
                // true = Lượt BẠN, false = Lượt BOT
                lblPlayer.Text = gameState.IsPlayerTurn ? "🎯 Lượt: BẠN" : "🤖 Lượt: BOT";
                lblTurns.Text = $"Lượt đã chơi: Bạn={gameState.PlayerTurns} | BOT={gameState.OpponentTurns}";
            }
            else
            {
                // true = Lượt A, false = Lượt B
                lblPlayer.Text = gameState.IsPlayerTurn ? "🎯 Lượt: NGƯỜI CHƠI A" : "🎯 Lượt: NGƯỜI CHƠI B";
                lblTurns.Text = $"Lượt đã chơi: A={gameState.PlayerTurns} | B={gameState.OpponentTurns}";
            }
        }

        private void DisableAllButtons()
        {
            foreach (Button btn in enemyGridButtons) { btn.Enabled = false; }
        }

        private void DisableEnemyButtons()
        {
            foreach (Button btn in enemyGridButtons) { btn.Enabled = false; }
        }

        private void EnableEnemyButtons()
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (gameState.OpponentBoard.DisplayGrid[i, j] == '~')
                    {
                        enemyGridButtons[i, j].Enabled = true;
                    }
                }
            }
        }

        private void ResetGame(Random mainRandom)
        {
            gameState = new GameState(gameState.IsVsBot, mainRandom);
            gameState.PlayerBoard.RandomPlaceShips(mainRandom);
            gameState.OpponentBoard.RandomPlaceShips(mainRandom);
            UpdateBoards();
            UpdateUI();
            lblStatus.Text = "Game mới! Sẵn sàng chiến đấu!";
            lblStatus.ForeColor = Color.FromArgb(0, 100, 0);
            EnableEnemyButtons();
        }
    }
}