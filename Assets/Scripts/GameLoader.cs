using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class GameLoader : MonoBehaviour
    {
        public Map map;
        public RandomGenerator random;
        public Game game;

        void Start()
        {
            // construction 
            random.Construct();
            map.Construct();

            map.Generate();
        }
    }
}
