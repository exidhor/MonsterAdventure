using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Delaunay.Geo;
using UnityEngine;

namespace MonsterAdventure
{
    public class Map : MonoBehaviour
    {
        // params
        public uint mapSize;
        public uint tileSize;

        // global config
        private Rect _bounds;

        // utils generators
        public RandomGenerator random;

        // content managers
        public TileManager tileManager;
        public ZoneManager zoneManager;
        public BiomeManager biomeManager;
        public BaseManager baseManager;

        private void Awake()
        {
            // nothing
        }

        public void Construct()
        {
            tileManager.Construct((int)mapSize, (int)tileSize);
            zoneManager.Construct();
            biomeManager.Construct();
            baseManager.Construct();
        }

        public void Generate()
        {
            // We generate the biomes
            biomeManager.Generate(tileManager.GetTiles(), (int)mapSize, random);

            // then generate zones
            zoneManager.Generate(tileManager.GetTiles());

            // then generate bases
            baseManager.Generate();
        }

        private List<Tile> GetTileIn(Region region)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = region.left - (int)_bounds.x; i < region.right - _bounds.x; i++)
            {
                for (int j = region.bot - (int)_bounds.y; j < region.top - _bounds.y; j++)
                {
                    tiles.Add(tileManager.Get(i, j));
                }
            }

            return tiles;
        }
    }
}
