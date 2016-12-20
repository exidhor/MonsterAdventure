using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    public class MapGenerator : MonoBehaviour
    {
        public int mapSize; // number of tile
        public int tileSize;

        public Map mapPrefab;

        public NoiseGenerator noiseGenerator;
        //public SimplexNoiseEntry simplexNoiseEntry;
        public RandomGenerator random;

        private Map _map;

        public Map Construct()
        {
            _map = InstaciateMap();
            noiseGenerator.Construct();
            _map.Construct(mapSize, tileSize, random, noiseGenerator);

            return _map;
        }

        private Map InstaciateMap()
        {
            Map map = Instantiate<Map>(mapPrefab);
            map.name = "Map";
            map.transform.parent = gameObject.transform;

            return map;
        }
    }
}