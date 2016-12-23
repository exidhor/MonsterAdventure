using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Biome : MonoBehaviour
    {
        public List<Tile> tiles;
        public BiomeType type;

        private Dictionary<int, List<Tile>> _sortedTiles;

        private void Awake()
        {
            tiles = new List<Tile>();
            _sortedTiles = new Dictionary<int, List<Tile>>();
        }

        public void Add(Tile tile)
        {
            tiles.Add(tile);
        }

        public List<Tile> GetTilesFromMinDistance(int minDistance)
        {
            List<Tile> foundedTiles = new List<Tile>();

            foreach (int distance in _sortedTiles.Keys)
            {
                if (distance >= minDistance)
                {
                    foundedTiles.AddRange(_sortedTiles[distance]);
                }
            }

            return foundedTiles;
        }

        public void Organize(List<List<Tile>> allTiles)
        {
            List<Tile> toSort = new List<Tile>(tiles);

            // limit tiles (tile near another biome)
            List<Tile> limitTiles = new List<Tile>();

            // at the first iteration, we set the tile which are adjacent to
            // a tile of another biome
            for (int i = 0; i < toSort.Count; i++)
            {
                if (Tile.BelongToBiomeLimit(toSort[i], allTiles))
                {
                    limitTiles.Add(toSort[i]);
                    toSort[i].SetDistanceToLimit(0);
                    toSort.RemoveAt(i);
                    i--;
                }
            }

            // then we check if no limit tile were found
            if (limitTiles.Count == 0)
            {
                // we add all the tiles at the max pos
                _sortedTiles.Add(int.MaxValue, toSort);

                // we actualize the distance to limit in each tile
                foreach (Tile tile in toSort)
                {
                    tile.SetDistanceToLimit(int.MaxValue);
                }

                // we stop the function
                return;
            }

            //else, we add the limit tiles to the dictionary
            _sortedTiles.Add(0, limitTiles);

            int currentDistanceToLimit = 1;

            // then we iterate until all tiles are sorted
            while (toSort.Count > 0)
            {
                List<Tile> tilesForCurrentDistance = new List<Tile>();

                for (int i = 0; i < toSort.Count; i++)
                {
                    if (IsNearDeterminedDistance(toSort[i].GetX(),
                                                 toSort[i].GetY(),
                                                 allTiles))
                    {
                        tilesForCurrentDistance.Add(toSort[i]);
                        toSort.RemoveAt(i);
                        i--;
                    }
                }

                if (tilesForCurrentDistance.Count > 0)
                {
                    // set the distance in the tile
                    foreach (Tile tile in tilesForCurrentDistance)
                    {
                        tile.SetDistanceToLimit(currentDistanceToLimit);
                    }

                    // create an entry into the dictionary
                    _sortedTiles.Add(currentDistanceToLimit, tilesForCurrentDistance);
                }
                else
                {
                    Debug.LogError("No tile found during an iteration !");
                }
                
                // actualize the distance
                currentDistanceToLimit++;
            }

            // debug infos to display dictionary content
            //Debug.Log("Dictionary - " + type + " (" + _sortedTiles.Count + ") ------------- ");
            //foreach (int distance in _sortedTiles.Keys)
            //{
            //    Debug.Log("Distance : " + distance + ", size : " +  _sortedTiles[distance].Count);
            //}
        }

        private bool IsNearDeterminedDistance(int x, int y, List<List<Tile>> allTiles)
        {
            // check at the left
            int currentDistance;

            if (x > 0)
            {
                currentDistance = allTiles[x - 1][y].GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check top
            if (allTiles.Count > 0 && y < allTiles[0].Count - 1)
            {
                currentDistance = allTiles[x][y + 1].GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check right
            if (x < allTiles.Count - 1)
            {
                currentDistance = allTiles[x + 1][y].GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check bot
            if (y > 0)
            {
                currentDistance = allTiles[x][y - 1].GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}