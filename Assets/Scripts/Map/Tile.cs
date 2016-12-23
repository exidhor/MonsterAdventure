using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Tile : MonoBehaviour
    {
        private BiomeType _biomeType;
        private Zone _zone;
        private int _coordsInGrid_x;
        private int _coordsInGrid_y;

        private int _distanceToLimit;

        private void Awake()
        {
            _biomeType = BiomeType.None;
            _zone = null;
            _coordsInGrid_x = 0;
            _coordsInGrid_y = 0;
            _distanceToLimit = -1;
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

        public int GetDistanceFrom(Tile tile)
        {
            int dist_x = Math.Abs(tile._coordsInGrid_x - _coordsInGrid_x);
            int dist_y = Math.Abs(tile._coordsInGrid_y - _coordsInGrid_y);

            return dist_x + dist_y;
        }

        public static bool BelongToBiomeLimit(Tile tile, List<List<Tile>> tiles)
        {
            BiomeType currentType = tile.GetBiomeType();
            int current_x = tile._coordsInGrid_x;
            int current_y = tile._coordsInGrid_y;

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
    }
}
