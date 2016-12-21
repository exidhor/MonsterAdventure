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
        public RandomGenerator random;
        public VoronoiGenerator voronoi;
        public BiomeConfig biomeConfig;

        private Map _map;

        public Map Construct()
        {
            _map = InstaciateMap();

            noiseGenerator.Construct();

            Rect bounds = new Rect(-mapSize/2, -mapSize/2, mapSize, mapSize);

            voronoi.Construct(bounds, random);

            _map.Construct(bounds, tileSize, random, noiseGenerator, voronoi, biomeConfig);

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