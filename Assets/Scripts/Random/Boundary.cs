using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    /*
    * \brief   An interval, with a min and a max value.
    *          It is useful for a random generation with
    *          forbidden boundary.
    */
    public class Boundary
    {
        private int _min;
        private uint _width;

        public int min
        {
            get { return _min; }
        }

        public int width
        {
            get { return (int)_width; }
        }

        public int max
        {
            get { return min + width - 1; }
        }

        public Boundary(int a_Min, uint a_Width)
        {
            _min = a_Min;
            _width = a_Width;
        }
    }
}
