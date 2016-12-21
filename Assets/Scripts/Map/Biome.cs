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

        public void Add(Tile tile)
        {
            tiles.Add(tile);
        }
    }
}