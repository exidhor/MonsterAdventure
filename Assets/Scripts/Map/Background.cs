using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    public class Background : MonoBehaviour
    {
        public Tile[] tilePrefabs;

        private List<List<Tile>> _tiles;

        public void Construct(Vector2 mapSize, Vector2 tileSize)
        {
            _tiles = new List<List<Tile>>();

            Vector2 offset = (mapSize - new Vector2(1, 1)) / 2;

            for (int i = 0; i < mapSize.x; i++)
            {
                _tiles.Add(new List<Tile>());

                for (int j = 0; j < mapSize.y; j++)
                {
                    Tile prefab = tilePrefabs[0];

                    _tiles[i].Add(InstanciateTile(prefab, i, j));

                    float x = i*tileSize.x - offset.x;
                    float y = j*tileSize.y - offset.y;

                    _tiles[i][j].transform.position = new Vector2(x, y);
                }
            }
        }

        public Tile Get(uint x, uint y)
        {
            return _tiles[(int)x][(int)y];
        }

        public uint GetLength(uint index)
        {
            if (index == 0)
                return (uint) _tiles.Count;

            return (uint) _tiles[0].Count;
        }

        private Tile InstanciateTile(Tile prefab, int x, int y)
        {
            Tile tile = Instantiate<Tile>(prefab);
            tile.transform.parent = gameObject.transform;
            tile.name = prefab.name + " (" + x + ", " + y + ")";

            return tile;
        }
    }
}
