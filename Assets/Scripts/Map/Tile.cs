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
        private Biome _biome;

        private void Awake()
        {
            _biomeType = BiomeType.None;
            _biome = null;
        }

        public void SetBiomeType(BiomeType biomeType)
        {
            _biomeType = biomeType;
        }

        public void SetBiome(Biome biome)
        {
            _biome = biome;
        }

        public BiomeType GetBiomeType()
        {
            return _biomeType;
        }

        public Biome GetBiome()
        {
            return _biome;
        }
    }
}
