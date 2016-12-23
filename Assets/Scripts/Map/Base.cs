using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Base : MonoBehaviour
    {
        private int _coord_x;
        private int _coord_y;

        public BaseType _type;

        public void Construct(int x, int y)
        {
            _coord_x = x;
            _coord_y = y;
        }

        public int GetCoordX()
        {
            return _coord_x;
        }

        public int GetCoordY()
        {
            return _coord_y;
        }
    }
}