using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyOtGame
{
    class Move
    {
        public Piece source;
        public Point dest;
        public int dir;
        public int count;
        public Player player;

        public Move(Piece source, Point dest, int dir, int count, Player player)
        {
            this.source = source;
            this.dest = dest;
            this.dir = dir;
            this.count = count;
            this.player = player;
        }
    }
}
