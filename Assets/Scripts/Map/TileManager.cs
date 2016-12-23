using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    public class TileManager : MonoBehaviour
    {
        public bool drawDistance = true;

        public Tile[] tilePrefabs;
        public ZoneManager zoneManager;

        private List<List<Tile>> _tiles;

        public void Construct(int size, int tileSize)
        {
            Vector2 mapSize;
            mapSize.x = size;
            mapSize.y = size; 

            _tiles = new List<List<Tile>>();

            Vector2 offset = (mapSize - new Vector2(1, 1)) / 2;

            for (int i = 0; i < mapSize.x; i++)
            {
                _tiles.Add(new List<Tile>());

                for (int j = 0; j < mapSize.y; j++)
                {
                    if (tilePrefabs.Length == 0)
                    {
                        Debug.LogError("No tilePrefabs set");
                    }

                    Tile prefab = tilePrefabs[0];

                    _tiles[i].Add(InstantiateTile(prefab, i, j));

                    float x = i*tileSize - offset.x;
                    float y = j*tileSize - offset.y;

                    _tiles[i][j].transform.position = new Vector2(x, y);
                }
            }
        }

        public Tile Get(int x, int y)
        {
            if (0 <= x && x < _tiles.Count
                && 0 <= y && y < _tiles[0].Count)
            {
                return _tiles[(int)x][(int)y];
            }

            return null;
        }

        public uint GetLength(uint index)
        {
            if (index == 0)
                return (uint) _tiles.Count;

            return (uint) _tiles[0].Count;
        }

        private Tile InstantiateTile(Tile prefab, int x, int y)
        {
            Tile tile = Instantiate<Tile>(prefab);
            tile.transform.parent = gameObject.transform;
            tile.name = prefab.name + " (" + x + ", " + y + ")";
            tile.SetPositionInGrid(x, y);

            return tile;
        }

        public List<List<Tile>> GetTiles()
        {
            return _tiles;
        }
    }
}