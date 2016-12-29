using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delaunay.Geo;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(MovableGrid))]
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
        public PlaceManager placeManager;
        public SectorManager sectorManager;

        // others
        private MovableGrid _movableGrid;

        private void Awake()
        {
            _movableGrid = GetComponent<MovableGrid>();
        }

        public void Construct()
        {
            // init bounds
            _bounds = new Rect(new Vector2(), new Vector2(mapSize, mapSize));
            _bounds.x -= mapSize/2f;
            _bounds.y -= mapSize/2f;

            sectorManager.Construct(_bounds);
            chunkManager.Construct((int)mapSize);
            zoneManager.Construct();
            biomeManager.Construct();
            placeManager.Construct();
        }

        public void Generate()
        {
            // We generate the biomes
            biomeManager.Generate(chunkManager.GetChunks(), (int)mapSize, random);

            // then generate zones
            zoneManager.Generate(chunkManager.GetChunks());

            // then generate bases
            placeManager.Generate();

            // we set the movable grid
            int coord = (int)mapSize/2 + 1;
            _movableGrid.SetPosition(coord, coord);
        }
    }
}
