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

        // global config
        private Rect _bounds;

        // utils generators
        public RandomGenerator random;

        // content managers
        public ChunkManager chunkManager;
        public ZoneManager zoneManager;
        public BiomeManager biomeManager;
        public BaseManager baseManager;

        private void Awake()
        {
            // nothing
        }

        public void Construct()
        {
            chunkManager.Construct((int)mapSize);
            zoneManager.Construct();
            biomeManager.Construct();
            baseManager.Construct();
        }

        public void Generate()
        {
            // We generate the biomes
            biomeManager.Generate(chunkManager.GetChunks(), (int)mapSize, random);

            // then generate zones
            zoneManager.Generate(chunkManager.GetChunks());

            // then generate bases
            baseManager.Generate();
        }
    }
}
