using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    public class MapGenerator : MonoBehaviour
    {
        public Vector2 mapSize; // number of tile
        public Vector2 tileSize;

        public int largestFeature;
        public double persistence;

        public Map mapPrefab;
        public RandomGenerator random;

        private Map _map;

        public Map Construct()
        {
            _map = InstaciateMap();
            _map.Construct(mapSize, tileSize, largestFeature, persistence, random);

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