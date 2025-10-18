using battleship.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Models
{
    public class GameState
    {
        public Board PlayerBoard { get; set; }
        public Board OpponentBoard { get; set; }
        public bool IsPlayerTurn { get; set; }
        public int PlayerTurns { get; set; }
        public int OpponentTurns { get; set; }
        public bool IsVsBot { get; set; }
        public BotAI Bot { get; set; }

        public GameState(bool vsBot, Random randomInstance)
        {
            PlayerBoard = new Board();
            OpponentBoard = new Board();
            IsPlayerTurn = true;
            PlayerTurns = 0;
            OpponentTurns = 0;
            IsVsBot = vsBot;
            if (vsBot)
                Bot = new BotAI(randomInstance);
        }
    }
}
