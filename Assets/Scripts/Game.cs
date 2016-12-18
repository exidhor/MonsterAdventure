using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Game : MonoBehaviour
    {
        private Map _map;

        public void SetMap(Map map)
        {
            _map = map;
        }
    }
}
