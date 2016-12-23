﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Biome : MonoBehaviour
    {
        public List<Chunk> chunks;
        public BiomeType type;

        private Dictionary<int, List<Chunk>> _sortedChunks;

        private void Awake()
        {
            chunks = new List<Chunk>();
            _sortedChunks = new Dictionary<int, List<Chunk>>();
        }

        public void Add(Chunk chunk)
        {
            chunks.Add(chunk);
        }

        public List<Chunk> GetChunksFromMinDistance(int minDistance)
        {
            List<Chunk> foundedChunks = new List<Chunk>();

            foreach (int distance in _sortedChunks.Keys)
            {
                if (distance >= minDistance)
                {
                    foundedChunks.AddRange(_sortedChunks[distance]);
                }
            }

            return foundedChunks;
        }

        public void Organize(List<List<Chunk>> allChunks)
        {
            List<Chunk> toSort = new List<Chunk>(chunks);

            // limit chunks (chunks near another biome)
            List<Chunk> limitChunks = new List<Chunk>();

            // at the first iteration, we set the chunk which are adjacent to
            // a chunk of another biome
            for (int i = 0; i < toSort.Count; i++)
            {
                if (Chunk.BelongToBiomeLimit(toSort[i], allChunks))
                {
                    limitChunks.Add(toSort[i]);
                    toSort[i].SetDistanceToLimit(0);
                    toSort.RemoveAt(i);
                    i--;
                }
            }

            // then we check if no limit chunk were found
            if (limitChunks.Count == 0)
            {
                // we add all the chunks at the max pos
                _sortedChunks.Add(int.MaxValue, toSort);

                // we actualize the distance to limit in each chunk
                foreach (Chunk chunk in toSort)
                {
                    chunk.SetDistanceToLimit(int.MaxValue);
                }

                // we stop the function
                return;
            }

            //else, we add the limit chunks to the dictionary
            _sortedChunks.Add(0, limitChunks);

            int currentDistanceToLimit = 1;

            // then we iterate until all chunks are sorted
            while (toSort.Count > 0)
            {
                List<Chunk> chunksForCurrentDistance = new List<Chunk>();

                for (int i = 0; i < toSort.Count; i++)
                {
                    if (IsNearDeterminedDistance(toSort[i].GetX(),
                                                 toSort[i].GetY(),
                                                 allChunks))
                    {
                        chunksForCurrentDistance.Add(toSort[i]);
                        toSort.RemoveAt(i);
                        i--;
                    }
                }

                if (chunksForCurrentDistance.Count > 0)
                {
                    // set the distance in the chunk
                    foreach (Chunk chunk in chunksForCurrentDistance)
                    {
                        chunk.SetDistanceToLimit(currentDistanceToLimit);
                    }

                    // create an entry into the dictionary
                    _sortedChunks.Add(currentDistanceToLimit, chunksForCurrentDistance);
                }
                else
                {
                    Debug.LogError("No chunk found during an iteration !");
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

        private bool IsNearDeterminedDistance(int x, int y, List<List<Chunk>> allChunks)
        {
            // check at the left
            int currentDistance;

            if (x > 0)
            {
                currentDistance = allChunks[x - 1][y].GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check top
            if (allChunks.Count > 0 && y < allChunks[0].Count - 1)
            {
                currentDistance = allChunks[x][y + 1].GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check right
            if (x < allChunks.Count - 1)
            {
                currentDistance = allChunks[x + 1][y].GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check bot
            if (y > 0)
            {
                currentDistance = allChunks[x][y - 1].GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}