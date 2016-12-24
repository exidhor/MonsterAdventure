using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Chunk : MonoBehaviour
    {
        public static uint size;
        public static Tile tilePrefab;

        private BiomeType _biomeType;
        private Zone _zone;
        private int _coordsInGrid_x;
        private int _coordsInGrid_y;

        private int _distanceToLimit;

        private List<List<Tile>> _tiles;

        private bool _isActive = false;

        private void Awake()
        {
            _biomeType = BiomeType.None;
            _zone = null;
            _coordsInGrid_x = 0;
            _coordsInGrid_y = 0;
            _distanceToLimit = -1;

            transform.localScale = new Vector2(size, size);
        }

        public void Generate(Sprite sprite)
        {
            Vector2 chunkSize = new Vector2(size, size);
            Vector2 offset = (chunkSize - new Vector2(1, 1)) / 2;

            _tiles = new List<List<Tile>>();

            for (int i = 0; i < size; i++)
            {
                _tiles.Add(new List<Tile>());

                for (int j = 0; j < size; j++)
                {
                    _tiles[i].Add(InstantiateTile(i, j, offset));
                    _tiles[i][j].SetSprite(sprite);
                }
            }
        }

        public void SetActive(bool state)
        {
            _isActive = state;

            // todo

            if (_isActive)
            {
                // retrieve needed tile and object
            }
            else
            {
                // return go to pool allocator
            }
        }

        public void SetPositionInGrid(int x, int y)
        {
            _coordsInGrid_x = x;
            _coordsInGrid_y = y;
        }

        public void SetBiomeType(BiomeType biomeType)
        {
            _biomeType = biomeType;
        }

        public int GetX()
        {
            return _coordsInGrid_x;
        }

        public int GetY()
        {
            return _coordsInGrid_y;
        }

        public void SetZone(Zone zone)
        {
            _zone = zone;
        }

        public BiomeType GetBiomeType()
        {
            return _biomeType;
        }

        public Zone GetZone()
        {
            return _zone;
        }

        public void SetDistanceToLimit(int distanceToLimit)
        {
            _distanceToLimit = distanceToLimit;
        }

        public int GetDistanceToLimit()
        {
            return _distanceToLimit;
        }

        public int GetDistanceFrom(Chunk chunk)
        {
            int dist_x = Math.Abs(chunk._coordsInGrid_x - _coordsInGrid_x);
            int dist_y = Math.Abs(chunk._coordsInGrid_y - _coordsInGrid_y);

            return dist_x + dist_y;
        }

        public static bool BelongToBiomeLimit(Chunk chunk, List<List<Chunk>> tiles)
        {
            BiomeType currentType = chunk.GetBiomeType();
            int current_x = chunk._coordsInGrid_x;
            int current_y = chunk._coordsInGrid_y;

            // check at the left
            if (current_x > 0 
                && tiles[current_x - 1][current_y].GetBiomeType() != currentType)
            {
                return true;
            }

            // check top
            if (tiles.Count > 0
                && current_y < tiles[0].Count - 1
                && tiles[current_x][current_y + 1].GetBiomeType() != currentType)
            {
                return true;
            }

            // check right
            if (current_x < tiles.Count - 1
                && tiles[current_x + 1][current_y].GetBiomeType() != currentType)
            {
                return true;
            }

            // check bot
            if (current_y > 0
                && tiles[current_x][current_y - 1].GetBiomeType() != currentType)
            {
                return true;
            }

            return false;
        }

        private Tile InstantiateTile(int x, int y, Vector2 offset)
        {
            Tile tile = Instantiate<Tile>(tilePrefab);
            tile.transform.parent = gameObject.transform;

            Vector2 position = tile.transform.parent.position;

            position += new Vector2(x, y) - offset;

            tile.transform.position = position;
            tile.name = tilePrefab.name + " (" + x + ", " + y + ")";

            return tile;
        }
    }
}