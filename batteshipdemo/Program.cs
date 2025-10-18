using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBattleship
{
    // Model: Ship
    public class Ship
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public List<(int row, int col)> Positions { get; set; }
        public int Hits { get; set; }

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Positions = new List<(int, int)>();
            Hits = 0;
        }

        public bool IsSunk() => Hits >= Size;
    }

    // Model: Board
    public class Board
    {
        public char[,] Grid { get; private set; }
        public char[,] DisplayGrid { get; private set; }
        public List<Ship> Ships { get; private set; }
        private const int SIZE = 7;

        public Board()
        {
            Grid = new char[SIZE, SIZE];
            DisplayGrid = new char[SIZE, SIZE];
            Ships = new List<Ship>();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Grid[i, j] = '~';
                    DisplayGrid[i, j] = '~';
                }
            }
        }

        public bool PlaceShip(Ship ship, int row, int col, bool isHorizontal)
        {
            var positions = new List<(int, int)>();

            // Check if placement is valid
            for (int i = 0; i < ship.Size; i++)
            {
                int r = isHorizontal ? row : row + i;
                int c = isHorizontal ? col + i : col;

                if (r >= SIZE || c >= SIZE || Grid[r, c] != '~')
                    return false;

                positions.Add((r, c));
            }

            // Place ship
            foreach (var (r, c) in positions)
            {
                Grid[r, c] = 'S';
                ship.Positions.Add((r, c));
            }

            Ships.Add(ship);
            return true;
        }

        public void RandomPlaceShips()
        {
            var random = new Random();
            var shipSizes = new[] { 3, 2, 2 }; // Ship1(3), Ship2(2), Ship3(2)
            var shipNames = new[] { "Ship1", "Ship2", "Ship3" };

            for (int i = 0; i < shipSizes.Length; i++)
            {
                var ship = new Ship(shipNames[i], shipSizes[i]);
                bool placed = false;

                while (!placed)
                {
                    int row = random.Next(SIZE);
                    int col = random.Next(SIZE);
                    bool isHorizontal = random.Next(2) == 0;

                    placed = PlaceShip(ship, row, col, isHorizontal);
                }
            }
        }

        public (bool hit, bool sunk, string shipName) Attack(int row, int col)
        {
            if (row < 0 || row >= SIZE || col < 0 || col >= SIZE)
                return (false, false, "");

            if (DisplayGrid[row, col] != '~')
                return (false, false, ""); // Already attacked

            bool isHit = Grid[row, col] == 'S';
            DisplayGrid[row, col] = isHit ? 'X' : 'O';

            if (isHit)
            {
                foreach (var ship in Ships)
                {
                    if (ship.Positions.Contains((row, col)))
                    {
                        ship.Hits++;
                        return (true, ship.IsSunk(), ship.Name);
                    }
                }
            }

            return (isHit, false, "");
        }

        public bool AllShipsSunk() => Ships.All(s => s.IsSunk());

        public void Display(bool showShips = false)
        {
            Console.WriteLine("  0 1 2 3 4 5 6");
            for (int i = 0; i < SIZE; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < SIZE; j++)
                {
                    if (showShips)
                        Console.Write(Grid[i, j] + " ");
                    else
                        Console.Write(DisplayGrid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }

    // Game State Manager
    public class GameState
    {
        public Board PlayerABoard { get; set; }
        public Board PlayerBBoard { get; set; }
        public string CurrentPlayer { get; set; }
        public int PlayerATurns { get; set; }
        public int PlayerBTurns { get; set; }

        public GameState()
        {
            PlayerABoard = new Board();
            PlayerBBoard = new Board();
            CurrentPlayer = "A";
            PlayerATurns = 0;
            PlayerBTurns = 0;
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == "A" ? "B" : "A";
        }

        public Board GetCurrentPlayerBoard()
        {
            return CurrentPlayer == "A" ? PlayerABoard : PlayerBBoard;
        }

        public Board GetOpponentBoard()
        {
            return CurrentPlayer == "A" ? PlayerBBoard : PlayerABoard;
        }

        public void IncrementTurn()
        {
            if (CurrentPlayer == "A")
                PlayerATurns++;
            else
                PlayerBTurns++;
        }
    }

    // Main Game Controller
    class Program
    {
        static GameState gameState;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("     MINI BATTLESHIP 2 NGƯỜI (7×7)     ");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine();

            InitializeGame();
            GameLoop();
        }

        static void InitializeGame()
        {
            gameState = new GameState();

            Console.WriteLine("Đang đặt tàu ngẫu nhiên cho Người chơi A...");
            gameState.PlayerABoard.RandomPlaceShips();

            Console.WriteLine("Đang đặt tàu ngẫu nhiên cho Người chơi B...");
            gameState.PlayerBBoard.RandomPlaceShips();

            Console.WriteLine("\n✓ Đã đặt xong tất cả tàu!\n");
            Console.WriteLine("Mỗi người có 3 tàu: Ship1(3 ô), Ship2(2 ô), Ship3(2 ô)");
            Console.WriteLine("\nKý hiệu:");
            Console.WriteLine("  ~ = Chưa bắn");
            Console.WriteLine("  O = Trượt");
            Console.WriteLine("  X = Trúng");
            Console.WriteLine("\nNhấn Enter để bắt đầu...");
            Console.ReadLine();
        }

        static void GameLoop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"═══ LƯỢT CỦA NGƯỜI CHƠI {gameState.CurrentPlayer} ═══\n");

                var opponentBoard = gameState.GetOpponentBoard();

                Console.WriteLine("Bảng đối thủ (Bạn bắn vào đây):");
                opponentBoard.Display(false);

                Console.WriteLine("\n" + new string('─', 40));

                // Input attack
                int row, col;
                while (true)
                {
                    Console.Write($"\nNgười chơi {gameState.CurrentPlayer}, nhập tọa độ bắn (hàng cột, vd: 3 4): ");
                    var input = Console.ReadLine().Split(' ');

                    if (input.Length == 2 && int.TryParse(input[0], out row) && int.TryParse(input[1], out col))
                    {
                        if (row >= 0 && row < 7 && col >= 0 && col < 7)
                        {
                            if (opponentBoard.DisplayGrid[row, col] == '~')
                                break;
                            else
                                Console.WriteLine("⚠ Ô này đã bắn rồi! Chọn ô khác.");
                        }
                        else
                            Console.WriteLine("⚠ Tọa độ không hợp lệ! Nhập từ 0-6.");
                    }
                    else
                        Console.WriteLine("⚠ Nhập sai định dạng! Ví dụ: 3 4");
                }

                // Process attack
                var (hit, sunk, shipName) = opponentBoard.Attack(row, col);
                gameState.IncrementTurn();

                Console.WriteLine();
                if (hit)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"💥 TRÚNG! Bạn đã bắn trúng {shipName}!");
                    if (sunk)
                        Console.WriteLine($"⚓ {shipName} đã bị chìm!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("💧 TRƯỢT! Không trúng tàu nào.");
                    Console.ResetColor();
                }

                // Check win condition
                if (opponentBoard.AllShipsSunk())
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("═══════════════════════════════════════");
                    Console.WriteLine($"   🏆 NGƯỜI CHƠI {gameState.CurrentPlayer} THẮNG! 🏆   ");
                    Console.WriteLine("═══════════════════════════════════════");
                    Console.ResetColor();
                    Console.WriteLine($"\nSố lượt chơi:");
                    Console.WriteLine($"  Người chơi A: {gameState.PlayerATurns} lượt");
                    Console.WriteLine($"  Người chơi B: {gameState.PlayerBTurns} lượt");
                    Console.WriteLine("\nNhấn Enter để thoát...");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("\nNhấn Enter để chuyển lượt...");
                Console.ReadLine();

                // Clear screen before switching
                Console.Clear();
                Console.WriteLine($"Chuẩn bị cho Người chơi {(gameState.CurrentPlayer == "A" ? "B" : "A")}...");
                Console.WriteLine("Nhấn Enter khi sẵn sàng...");
                Console.ReadLine();

                gameState.SwitchPlayer();
            }
        }
    }
}