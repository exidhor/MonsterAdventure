using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Zone : MonoBehaviour
    {
        public BiomeType type;
        public List<Chunk> chunks;


        private void Awake()
        {
            // nothing
        }

        public void Construct(BiomeType type)
        {
            chunks = new List<Chunk>();
            this.type = type;
        }

        public void Add(Chunk chunk)
        {
            chunks.Add(chunk);
        }

        public void Absorb(Zone zone)
        {
            chunks.AddRange(zone.chunks);

            for (int i = 0; i < zone.chunks.Count; i++)
            {
                zone.chunks[i].SetZone(this);
            }
        }

        public void DisplayCubes()
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.8F);
                Gizmos.DrawCube(chunks[i].transform.position, chunks[i].transform.lossyScale);
            }
        }
    }
}
