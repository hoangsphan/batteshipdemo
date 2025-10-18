using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Models
{
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

            for (int i = 0; i < ship.Size; i++)
            {
                int r = isHorizontal ? row : row + i;
                int c = isHorizontal ? col + i : col;

                if (r >= SIZE || c >= SIZE || Grid[r, c] != '~')
                    return false;

                positions.Add((r, c));
            }

            foreach (var (r, c) in positions)
            {
                Grid[r, c] = 'S';
                ship.Positions.Add((r, c));
            }

            Ships.Add(ship);
            return true;
        }

        public void RandomPlaceShips(Random random)
        {
            
            var shipSizes = new[] { 3, 2, 2 };
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
                return (false, false, "");

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
    }
}
