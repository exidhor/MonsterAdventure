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
        public List<Tile> tiles;


        private void Awake()
        {
            // nothing
        }

        public void Construct(BiomeType type)
        {
            tiles = new List<Tile>();
            this.type = type;
        }

        public void Add(Tile tile)
        {
            tiles.Add(tile);
        }

        public void Absorb(Zone zone)
        {
            tiles.AddRange(zone.tiles);

            for (int i = 0; i < zone.tiles.Count; i++)
            {
                zone.tiles[i].SetZone(this);
            }
        }

        public void DisplayCubes()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.8F);
                Gizmos.DrawCube(tiles[i].transform.position, tiles[i].transform.lossyScale);
            }
        }
    }
}
