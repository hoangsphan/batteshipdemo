using battleship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.AI
{
    public class BotAI
    {
        private Random random;
        private List<(int, int)> huntQueue = new List<(int, int)>();
        public BotAI(Random randomInstance)
        {
            this.random = randomInstance;
        }
        public (int row, int col) GetNextMove(Board opponentBoard)
        {
            if (huntQueue.Count > 0)
            {
                var move = huntQueue[0];
                huntQueue.RemoveAt(0);
                return move;
            }

            while (true)
            {
                int row = random.Next(7);
                int col = random.Next(7);

                if (opponentBoard.DisplayGrid[row, col] == '~')
                    return (row, col);
            }
        }

        public void ProcessResult(int row, int col, bool hit, Board opponentBoard)
        {
            if (hit)
            {
                var adjacents = new List<(int, int)>
                {
                    (row - 1, col), (row + 1, col),
                    (row, col - 1), (row, col + 1)
                };

                foreach (var (r, c) in adjacents)
                {
                    if (r >= 0 && r < 7 && c >= 0 && c < 7 &&
                        opponentBoard.DisplayGrid[r, c] == '~' &&
                        !huntQueue.Contains((r, c)))
                    {
                        huntQueue.Add((r, c));
                    }
                }
            }
        }
    }
}
