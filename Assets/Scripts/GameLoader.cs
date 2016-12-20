using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class GameLoader : MonoBehaviour
    {
        public MapGenerator mapGenerator;
        public RandomGenerator random;
        public Game game;

        void Start()
        {
            // construction 
            random.Construct();
            Map map = mapGenerator.Construct();

            map.Generate();

            //game.SetMap(map);
        }
    }
}
