using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Ship
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsVertical { get; set; }
        public int Size { get; set; }
        public int Position { get; set; }
        public List<int[]> Coordinates { get; set; }

        public Ship(int size)
        {
            Size = size;
            Coordinates = new List<int[]>();
        }
        public Ship(int size, int position)
        {
            Size = size;
            Position = position;
        }

        public Ship(int size, int x, int y, bool isVertical)
        {
            Size = size;
            X = x;
            Y = y;
            IsVertical = isVertical;
        }
    }
}
