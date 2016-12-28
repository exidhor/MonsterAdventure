using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public struct Coords
    {
        public int abs;
        public int ord;

        public Coords(int abs, int ord)
        {
            this.abs = abs;
            this.ord = ord;
        }
    }
}
