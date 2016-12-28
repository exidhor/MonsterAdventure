using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Represent an area unit.
    /// </summary>
    public class Chunk : MonoBehaviour
    {
        public static uint size;
        public static Tile tilePrefab;

        private BiomeType _biomeType;
        private Zone _zone;
        private Coords _coordsInGrid;

        private int _distanceToLimit;

        private List<List<Tile>> _tiles;

        private bool _isActive = false;

        /// <summary>
        /// Initialize the <see cref="Chunk" /> parameters
        /// </summary>
        private void Awake()
        {
            _biomeType = BiomeType.None;
            _zone = null;
            _distanceToLimit = -1;

            transform.localScale = new Vector2(size, size);
        }

        /// <summary>
        /// Generate the <see cref="Chunk" /> content. (currently, it's some <see cref="Tile" />s)
        /// </summary>
        /// <param name="sprite"></param>
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

        /// <summary>
        /// Set the <see cref="Chunk" /> position in the <see cref="Chunk" /> grid
        /// </summary>
        /// <param name="x">The abs position in the chunk grid</param>
        /// <param name="y">The ord position in the chunk grid</param>
        public void SetPositionInGrid(int x, int y)
        {
            _coordsInGrid.abs = x;
            _coordsInGrid.ord = y;
        }

        /// <summary>
        /// Set the <see cref="Chunk" /> position in the <see cref="Chunk" /> grid
        /// </summary>
        /// <param name="coords">The position in the chunk grid</param>
        public void SetPositionInGrid(Coords coords)
        {
            _coordsInGrid = coords;
        }

        /// <summary>
        /// Set the <see cref="BiomeType" /> of the <see cref="Chunk" />
        /// </summary>
        /// <param name="biomeType">The new type of the chunk</param>
        public void SetBiomeType(BiomeType biomeType)
        {
            _biomeType = biomeType;
        }

        /// <summary>
        /// Return the coord in the <see cref="Chunk" /> grid
        /// </summary>
        /// <returns>The coord in the chunk grid</returns>
        public Coords GetCoords()
        {
            return _coordsInGrid;
        }

        /// <summary>
        /// Set the membership to the <see cref="Zone" />
        /// </summary>
        /// <param name="zone">The zone</param>
        public void SetZone(Zone zone)
        {
            _zone = zone;
        }

        /// <summary>
        /// Return the <see cref="BiomeType" /> of the <see cref="Chunk" />
        /// </summary>
        /// <returns>The BiomeType of the Chunk</returns>
        public BiomeType GetBiomeType()
        {
            return _biomeType;
        }

        /// <summary>
        /// Return the <see cref="Zone" /> of the <see cref="Chunk" />
        /// </summary>
        /// <returns>The <see cref="Zone" /> which the Chunk belong to</returns>
        public Zone GetZone()
        {
            return _zone;
        }

        /// <summary>
        /// Set the distance to the limit.
        /// </summary>
        /// <remarks>(the limit is two adjacent <see cref="Chunk" />s
        /// which belong to different <see cref="Biome" />s).</remarks>
        /// <param name="distanceToLimit">The distance to the limit</param>
        public void SetDistanceToLimit(int distanceToLimit)
        {
            _distanceToLimit = distanceToLimit;
        }

        /// <summary>
        /// Return the distance to the limit 
        /// </summary>
        /// <remarks>(the limit is two adjacent <see cref="Chunk" />s
        /// which belong to different <see cref="Biome" />s).</remarks>
        /// <returns></returns>
        public int GetDistanceToLimit()
        {
            return _distanceToLimit;
        }

        public int GetDistanceFrom(Chunk chunk)
        {
            int dist_x = Math.Abs(chunk._coordsInGrid.abs - _coordsInGrid.abs);
            int dist_y = Math.Abs(chunk._coordsInGrid.ord - _coordsInGrid.ord);

            return dist_x + dist_y;
        }

        public static bool BelongToBiomeLimit(Chunk chunk, List<List<Chunk>> tiles)
        {
            BiomeType currentType = chunk.GetBiomeType();
            int current_x = chunk._coordsInGrid.abs;
            int current_y = chunk._coordsInGrid.ord;

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