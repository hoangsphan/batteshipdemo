using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Models
{
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
}
