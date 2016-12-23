using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    public class ChunkManager : MonoBehaviour
    {
        public bool drawDistance = true;

        public Chunk[] chunkPrefabs;
        public ZoneManager zoneManager;

        private List<List<Chunk>> _chunks;

        public void Construct(int size)
        {
            Vector2 mapSize;
            mapSize.x = size;
            mapSize.y = size; 

            _chunks = new List<List<Chunk>>();

            Vector2 offset = (mapSize - new Vector2(1, 1)) / 2;

            for (int i = 0; i < mapSize.x; i++)
            {
                _chunks.Add(new List<Chunk>());

                for (int j = 0; j < mapSize.y; j++)
                {
                    if (chunkPrefabs.Length == 0)
                    {
                        Debug.LogError("No chunkPrefabs set");
                    }

                    Chunk prefab = chunkPrefabs[0];

                    _chunks[i].Add(InstantiateTile(prefab, i, j));

                    float x = i * _chunks[i][j].transform.lossyScale.x - offset.x;
                    float y = j * _chunks[i][j].transform.lossyScale.y - offset.y;

                    _chunks[i][j].transform.position = new Vector2(x, y);
                }
            }
        }

        public Chunk Get(int x, int y)
        {
            if (0 <= x && x < _chunks.Count
                && 0 <= y && y < _chunks[0].Count)
            {
                return _chunks[(int)x][(int)y];
            }

            return null;
        }

        public uint GetLength(uint index)
        {
            if (index == 0)
                return (uint) _chunks.Count;

            return (uint) _chunks[0].Count;
        }

        private Chunk InstantiateTile(Chunk prefab, int x, int y)
        {
            Chunk chunk = Instantiate<Chunk>(prefab);
            chunk.transform.parent = gameObject.transform;
            chunk.name = prefab.name + " (" + x + ", " + y + ")";
            chunk.SetPositionInGrid(x, y);

            return chunk;
        }

        public List<List<Chunk>> GetChunks()
        {
            return _chunks;
        }
    }
}